using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    public class IncDecExpressionVisitor : ExpressionVisitor
    {
        private IEnumerable<KeyValuePair<string, object>> _parameterValues;
        private List<ParameterExpression> _remainingParameters;
        private bool _isLambdaExpression;

        public Expression Transform(Expression expression)
        {
            _parameterValues = new List<KeyValuePair<string, object>>();
            _remainingParameters = new List<ParameterExpression>();
            _isLambdaExpression = false;

            return Visit(expression);
        }

        public Expression Transform(LambdaExpression expression, IEnumerable<KeyValuePair<string, object>> parameterValues)
        {
            _parameterValues = parameterValues ?? new List<KeyValuePair<string, object>>();
            _remainingParameters = expression.Parameters.ToList();
            _isLambdaExpression = true;
            
            var body = Visit(expression.Body) ?? expression.Body;
            return Expression.Lambda(body, _remainingParameters);
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
            if (_isLambdaExpression && _parameterValues.Any(x => x.Key == node.Name))
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
