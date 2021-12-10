using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace ExpressionTrees.Task2.ExpressionMapping
{
    public class MappingGenerator
    {
        public Mapper<TSource, TDestination> Generate<TSource, TDestination>()
        {
            var matchingPropertyMaps = GetMatchingProperties<TSource, TDestination>();
            var mapFunction = BuildPropertyMapFunction<TSource, TDestination>(matchingPropertyMaps);

            return new Mapper<TSource, TDestination>(mapFunction);
        }

        private Func<TSource, TDestination> BuildPropertyMapFunction<TSource, TDestination>(IEnumerable<PropertyMap> propertyMaps)
        {
            var mapStatements = new List<Expression>();

            var destinationInstance = Expression.Variable(typeof(TDestination));
            var sourceInstance = Expression.Parameter(typeof(TSource));

            mapStatements.Add(Expression.Assign(destinationInstance, Expression.New(typeof(TDestination))));

            foreach (var propertyMap in propertyMaps)
            {
                var sourceProperty = Expression.Property(sourceInstance, propertyMap.SourceProperty);
                var targetProperty = Expression.Property(destinationInstance, propertyMap.DestinationProperty);

                var sourcePropertyValue = sourceProperty.Reduce();
                // for mismatching types
                if (sourceProperty.Type != targetProperty.Type)
                    sourcePropertyValue = Expression.Convert(sourcePropertyValue, targetProperty.Type);

                Expression statement = Expression.Assign(targetProperty, sourcePropertyValue);

                // for class/interface or nullable type
                if (!sourceProperty.Type.IsValueType || Nullable.GetUnderlyingType(sourceProperty.Type) != null)
                {
                    var valueNotNull = Expression.NotEqual(sourceProperty, Expression.Constant(null, sourceProperty.Type));
                    statement = Expression.IfThen(valueNotNull, statement);
                }
                mapStatements.Add(statement);
            }

            mapStatements.Add(destinationInstance);    // this is required to have the block expression to return the destination instance.
            Expression body = Expression.Block(new[] { destinationInstance }, mapStatements);
            // check if the source is not null
            if (!sourceInstance.Type.IsValueType)
            {
                var sourceNotNull = Expression.NotEqual(sourceInstance, Expression.Constant(null, sourceInstance.Type));
                body = Expression.Condition(sourceNotNull, body, Expression.Constant(null, destinationInstance.Type));
            }

            var lambda = Expression.Lambda<Func<TSource, TDestination>>(body, sourceInstance);

            return lambda.Compile();
        }

        private static IEnumerable<PropertyMap> GetMatchingProperties<TSource, TDestination>()
        {
            var properties = (from s in typeof(TSource).GetProperties()
                              from d in typeof(TDestination).GetProperties()
                              where s.Name == d.Name &&
                              s.CanRead &&
                              d.CanWrite &&
                              s.PropertyType.IsPublic &&
                              d.PropertyType.IsPublic &&
                              (
                                  (s.PropertyType.IsValueType &&
                                   d.PropertyType.IsValueType
                                  ) ||
                                  (s.PropertyType == typeof(string) &&
                                   d.PropertyType == typeof(string)
                                  )
                              )
                              select new PropertyMap
                              {
                                  SourceProperty = s,
                                  DestinationProperty = d
                              }).ToList();

            return properties;
        }
    }
}
