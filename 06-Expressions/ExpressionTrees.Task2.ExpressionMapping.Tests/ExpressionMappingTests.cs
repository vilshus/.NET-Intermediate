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
        public void Should_MapMatchingProperties_When_PropertiesCanBeParsed()
        {
            var mapGenerator = new MappingGenerator();
            var mapper = mapGenerator.Generate<Foo, Bar>();

            var source = new Foo()
            {
                Id = Guid.NewGuid(),
                Name = "name",
                Description = "Some description",
                Value = 5,
                FooUniqueProperty = "foo"
            };

            var result = mapper.Map(source);

            Assert.IsNotNull(result, "#1");
            Assert.AreEqual(source.Id, result.Id, "#2");
            Assert.AreEqual(source.Name, result.Name, "#3");
            Assert.AreEqual(source.Description, result.Description, "#4");
            Assert.AreEqual((int) source.Value, result.Value, "#5");
            Assert.IsNull(result.BarUniqueProperty, "#6");
        }

        [TestMethod]
        public void Should_NotThrowException_When_PropertyCannotBeParsed()
        {
            var mapGenerator = new MappingGenerator();
            var mapper = mapGenerator.Generate<Foo, Bar>();

            var source = new Foo()
            {
                Id = Guid.NewGuid(),
                Name = "name",
                Description = "Some description",
                Value = 5.5,
                FooUniqueProperty = "foo"
            };

            var result = mapper.Map(source);

            Assert.IsNotNull(result, "#1");
            Assert.AreEqual(source.Id, result.Id, "#2");
            Assert.AreEqual(source.Name, result.Name, "#3");
            Assert.AreEqual(source.Description, result.Description, "#4");
            Assert.IsNull(result.Value, "#5");
            Assert.IsNull(result.BarUniqueProperty, "#6");
        }

        [TestMethod]
        public void Should_NotThrowException_When_SourcePropertyIsNull()
        {
            var mapGenerator = new MappingGenerator();
            var mapper = mapGenerator.Generate<Foo, Bar>();

            var source = new Foo()
            {
                Id = Guid.NewGuid(),
                Name = null,
                Description = "Some description",
                Value = 5.5,
                FooUniqueProperty = "foo"
            };

            var result = mapper.Map(source);

            Assert.IsNotNull(result, "#1");
            Assert.AreEqual(source.Id, result.Id, "#2");
            Assert.IsNull(result.Name, "#3");
            Assert.AreEqual(source.Description, result.Description, "#4");
            Assert.IsNull(result.Value, "#5");
            Assert.IsNull(result.BarUniqueProperty, "#6");
        }
    }
}
