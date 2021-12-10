using System;
using ExpressionTrees.Task2.ExpressionMapping.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionTrees.Task2.ExpressionMapping.Tests
{
    [TestClass]
    public class ExpressionMappingTests
    {
        // todo: add as many test methods as you wish, but they should be enough to cover basic scenarios of the mapping generator

        [TestMethod]
        public void Should_MapMatchingProperties_When_AllPropertiesSameType()
        {
            var mapGenerator = new MappingGenerator();
            var mapper = mapGenerator.Generate<Source, Destination>();

            var source = new Source()
            {
                Id = Guid.NewGuid(),
                Name = "name",
                Description = "Some description",
                FooUniqueProperty = "foo"
            };

            var result = mapper.Map(source);

            Assert.IsNotNull(result, "#1");
            Assert.AreEqual(source.Id, result.Id, "#2");
            Assert.AreEqual(source.Name, result.Name, "#3");
            Assert.AreEqual(source.Description, result.Description, "#4");
            Assert.IsNull(result.BarUniqueProperty, "#5");
        }

        [TestMethod]
        public void Should_MapProperty_When_PropertyCanBeParsed()
        {
            var mapGenerator = new MappingGenerator();
            var mapper = mapGenerator.Generate<Source, Destination>();

            var source = new Source()
            {
                Value = 5
            };

            var result = mapper.Map(source);

            Assert.AreEqual(5, result.Value);
        }

        [TestMethod]
        public void Should_NotThrowException_When_PropertyCannotBeParsed()
        {
            var mapGenerator = new MappingGenerator();
            var mapper = mapGenerator.Generate<Source, Destination>();

            var source = new Source()
            {
                WrongTypeValue = "test"
            };

            var result = mapper.Map(source);

            Assert.IsNull(result.WrongTypeValue);
        }

        [TestMethod]
        public void Should_NotThrowException_When_SourceObjectIsNull()
        {
            var mapGenerator = new MappingGenerator();
            var mapper = mapGenerator.Generate<Source, Destination>();

            var result = mapper.Map(null);

            Assert.IsNull(result);
        }
    }
}
