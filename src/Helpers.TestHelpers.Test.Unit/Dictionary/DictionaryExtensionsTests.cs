using FluentAssertions;
using Helpers.TestHelpers.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Helpers.TestHelpers.Test.Unit
{
    [TestClass]
    public class DictionaryExtensionsTests
    {
        [TestMethod]
        public void IsEqualTo_TwoEqualDictionaries_ReturnsTrue()
        {
            // Arrange
            var first = new Dictionary<string, string>
            {
                { "one", "two" },
                { "three", "four" },
                { "five", "six" },
            };

            var second = new Dictionary<string, string>
            {
                { "one", "two" },
                { "three", "four" },
                { "five", "six" },
            };

            // Act
            var result = first.IsEqualTo(second);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void IsEqualTo_TwoEqualDictionariesWIthDifferentOrder_ReturnsTrue()
        {
            // Arrange
            var first = new Dictionary<string, string>
            {
                { "one", "two" },
                { "three", "four" },
                { "five", "six" },
            };

            var second = new Dictionary<string, string>
            {
                { "three", "four" },
                { "one", "two" },
                { "five", "six" },
            };

            // Act
            var result = first.IsEqualTo(second);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void IsEqualTo_SecondDictionaryHasMoreEntries_ReturnsFalse()
        {
            // Arrange
            var first = new Dictionary<string, string>
            {
                { "one", "two" },
                { "three", "four" },
                { "five", "six" },
            };

            var second = new Dictionary<string, string>
            {
                { "one", "two" },
                { "three", "four" },
                { "five", "six" },
                { "seven", "eight" }
            };

            // Act
            var result = first.IsEqualTo(second);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void IsEqualTo_SecondsDictionaryHasDifferentContent_ReturnsFalse()
        {
            // Arrange
            var first = new Dictionary<string, string>
            {
                { "one", "two" },
                { "three", "four" },
                { "five", "six" },
            };

            var second = new Dictionary<string, string>
            {
                { "thousand", "two" },
                { "three", "four" },
                { "five", "six" },
            };

            // Act
            var result = first.IsEqualTo(second);

            // Assert
            result.Should().BeFalse();
        }
    }
}