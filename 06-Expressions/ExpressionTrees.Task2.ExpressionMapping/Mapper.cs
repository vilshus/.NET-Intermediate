using System;
using System.ComponentModel;

namespace ExpressionTrees.Task2.ExpressionMapping
{
    public class Mapper<TSource, TDestination>
    {
        private readonly Func<TSource, TDestination> _mapFunction;

        internal Mapper(Func<TSource, TDestination> func)
        {
            _mapFunction = func;
        }

        public TDestination Map(TSource source)
        {
            var destination = _mapFunction(source);

            foreach (var destinationPropertyInfo in typeof(TDestination).GetProperties())
            {
                var sourcePropertyInfo = typeof(TSource).GetProperty(destinationPropertyInfo.Name);

                if (sourcePropertyInfo != null
                    && TryConvert(sourcePropertyInfo.GetValue(source), destinationPropertyInfo.PropertyType, out object value))
                {
                    destinationPropertyInfo.SetValue(destination, value);
                }
            }

            return destination;
        }

        private static bool TryConvert(object input, Type targetType, out object output)
        {
            output = null;

            if (input == null)
            {
                return false;
            }

            if (targetType == input.GetType())
            {
                output = input;
                return true;
            }

            try
            {
                output = TypeDescriptor.GetConverter(targetType).ConvertFromString(input.ToString());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
