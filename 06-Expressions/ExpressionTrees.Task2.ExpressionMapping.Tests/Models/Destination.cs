using System;

namespace ExpressionTrees.Task2.ExpressionMapping.Tests.Models
{
    internal class Destination
    {
        // add here some other properties
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Value { get; set; }
        public string BarUniqueProperty { get; set; }
        public int? WrongTypeValue { get; set; }
    }
}
