using System;

namespace ExpressionTrees.Task2.ExpressionMapping.Tests.Models
{
    internal class Source
    {
        // add here some properties
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double? Value { get; set; }
        public string FooUniqueProperty { get; set; }
        public string WrongTypeValue { get; set; }
    }
}
