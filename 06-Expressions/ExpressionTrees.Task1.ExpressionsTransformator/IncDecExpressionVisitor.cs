using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    public class IncDecExpressionVisitor : ExpressionVisitor
    {
        private IEnumerable<KeyValuePair<string, object>> _parameterValues;
        private List<ParameterExpression> _remainingParameters;

        public Expression Translate(Expression expression, IEnumerable<KeyValuePair<string, object>> parameterValues)
        {
            if (expression is LambdaExpression lambda)
            {
                _parameterValues = parameterValues;
                _remainingParameters = lambda.Parameters.ToList();
                var body = Visit(lambda.Body) ?? lambda.Body;
                return Expression.Lambda(body, _remainingParameters);
            }

            return Visit(expression);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.NodeType == ExpressionType.Add && IsIncDecExpression(node))
            {
                return Expression.Increment(Visit(node.Left) ?? node.Left);
            }

            if (node.NodeType == ExpressionType.Subtract && IsIncDecExpression(node))
            {
                return Expression.Decrement(Visit(node.Left) ?? node.Left);
            }

            return base.VisitBinary(node);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (_parameterValues.Any(x => x.Key == node.Name))
            {
                var parameterToChange = _parameterValues.Single(x => x.Key == node.Name);
                _remainingParameters.Remove(node);
                return Expression.Constant(parameterToChange.Value, parameterToChange.Value.GetType());
            }

            return base.VisitParameter(node);
        }

        private bool IsIncDecExpression(BinaryExpression node)
        {
            if (node.Left is ParameterExpression parameter && parameter.IsNumericType() &&
                node.Right is ConstantExpression constant && constant.Value is int constantValue && constantValue == 1)
            {
                return true;
            }

            return false;
        }
    }
}
