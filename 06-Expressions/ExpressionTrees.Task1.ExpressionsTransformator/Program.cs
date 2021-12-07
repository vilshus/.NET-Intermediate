/*
 * Create a class based on ExpressionVisitor, which makes expression tree transformation:
 * 1. converts expressions like <variable> + 1 to increment operations, <variable> - 1 - into decrement operations.
 * 2. changes parameter values in a lambda expression to constants, taking the following as transformation parameters:
 *    - source expression;
 *    - dictionary: <parameter name: value for replacement>
 * The results could be printed in console or checked via Debugger using any Visualizer.
 */
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Expression Visitor for increment/decrement.");
            Console.WriteLine();

            // todo: feel free to add your code here
            var translator = new IncDecExpressionVisitor();
            Expression<Func<int, int, int, bool, int>> exp = (x, y, z, add5) => (x - 1) + (y + 1) + (z - 1) + (add5 ? 5 : 0);
            var translatedExp = translator.Translate(exp, new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("x", 5),
                new KeyValuePair<string, object>("y", 4),
                new KeyValuePair<string, object>("z", 7)
            });

            Console.WriteLine($"Original expression: {exp}");
            Console.WriteLine($"Translated expression: {translatedExp}");

            var c = ((LambdaExpression)translatedExp).Compile().DynamicInvoke(true);
            Console.WriteLine($"Result: {c}");

            Console.ReadLine();
        }
    }
}
