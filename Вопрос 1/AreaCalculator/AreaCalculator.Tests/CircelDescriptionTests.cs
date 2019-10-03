using System;

using Xunit;

using FluentAssertions;

namespace AreaCalculator.Tests
{
    public class CircelDescriptionTests
    {
        [Theory]
        [InlineData(1.0)]
        [InlineData(10.0)]
        [InlineData(100_000.0)]
        [InlineData(0.0001)]
        [InlineData(1_000_000_000.0)]
        public void DescriptionCanBeCreatedWithAnAppropriateRadius(Double radius)
        {
            var exception = Record.Exception(() => new CircleDescription(radius));
            exception.Should().BeNull();
        }

        [Fact]
        public void DescriptionCannotBeCreatedWithNaN()
        {
            var exception = Record.Exception(() => new CircleDescription(Double.NaN));
            exception.Should().NotBeNull().And.BeOfType<ArgumentException>();
        }

        [Fact]
        public void DescriptionCannotBeCreatedWithInfiniteRadius()
        {
            var exception = Record.Exception(() => new CircleDescription(Double.PositiveInfinity));
            exception.Should().NotBeNull().And.BeOfType<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void DescriptionCannotBeCreatedWithZeroRadius()
        {
            var exception = Record.Exception(() => new CircleDescription(0.0));
            exception.Should().NotBeNull().And.BeOfType<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(-1.0)]
        [InlineData(-10.0)]
        [InlineData(-100_000.0)]
        [InlineData(-0.0001)]
        [InlineData(-1_000_000_000.0)]
        public void DescriptionCannotBeCreatedWithNegativeRadius(Double radius)
        {
            var exception = Record.Exception(() => new CircleDescription(radius));
            exception.Should().NotBeNull().And.BeOfType<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(1.0)]
        [InlineData(10.0)]
        [InlineData(100_000.0)]
        [InlineData(0.0001)]
        [InlineData(1_000_000_000.0)]
        public void RadiusIsRetained(Double radius)
        {
            var description = new CircleDescription(radius);

            var actualRadius = description.Radius;

            actualRadius.Should().Be(radius);
        }

        [Theory]
        [InlineData(1.0)]
        [InlineData(10.0)]
        [InlineData(100_000.0)]
        [InlineData(0.0001)]
        [InlineData(1_000_000_000.0)]
        public void AreaIsCorrectlyCalculatedForReasonablySmallCircles(Double radius)
        {
            var description = new CircleDescription(radius);
            var expectedArea = Math.PI * Math.Pow(radius, 2.0);

            var actualArea = description.CalculateArea();

            actualArea.Should().BeApproximately(expectedArea, 0.000_001);
        }

        [Fact]
        public void AreaCalclulationCanReturnInfinityForLargeCircles()
        {
            var description = new CircleDescription(Double.MaxValue);

            var expectedArea = Double.PositiveInfinity;

            var actualArea = description.CalculateArea();

            actualArea.Should().Be(expectedArea);
        }
    }
}
