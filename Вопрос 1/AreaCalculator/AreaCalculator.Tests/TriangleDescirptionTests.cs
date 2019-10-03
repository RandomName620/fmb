using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;

using FluentAssertions;

namespace AreaCalculator.Tests
{
    public class TriangleDesciptionTests
    {
        [Theory]
        [MemberData(nameof(GetAppropriateLengths))]
        public void DescriptionCanBeCreatedWithAnAppropriateLengthes(Double a, Double b, Double c)
        {
            var exception = Record.Exception(() => new TriangleDescription(a, b, c));
            exception.Should().BeNull();
        }

        [Theory]
        [MemberData(nameof(GetInappropriateLengths))]
        public void DescriptionCannotBeCreatedIfLengthsCantFormATriangle(Double a, Double b, Double c)
        {
            var exception = Record.Exception(() => new TriangleDescription(a, b, c));
            exception.Should().NotBeNull().And.BeOfType<ArgumentException>();
        }

        [Theory]
        [MemberData(nameof(GetLengthsWithNaN))]
        public void DescriptionCannotBeCreatedWithNaN(Double a, Double b, Double c)
        {
            var exception = Record.Exception(() => new TriangleDescription(a, b, c));
            exception.Should().NotBeNull().And.BeOfType<ArgumentException>();
        }

        [Theory]
        [MemberData(nameof(GetLengthsWithZero))]
        public void DescriptionCannotBeCreatedWithZeroes(Double a, Double b, Double c)
        {
            var exception = Record.Exception(() => new TriangleDescription(a, b, c));
            exception.Should().NotBeNull().And.BeOfType<ArgumentOutOfRangeException>();
        }

        [Theory]
        [MemberData(nameof(GetLengthsWithNegativeValue))]
        public void DescriptionCannotBeCreatedWithNegativeLength(Double a, Double b, Double c)
        {
            var exception = Record.Exception(() => new TriangleDescription(a, b, c));
            exception.Should().NotBeNull().And.BeOfType<ArgumentOutOfRangeException>();
        }

        [Theory]
        [MemberData(nameof(GetAppropriateLengths))]
        public void ASideIsRetained(Double a, Double b, Double c)
        {
            var description = new TriangleDescription(a, b, c);

            var actualA = description.A;

            actualA.Should().Be(a);
        }

        [Theory]
        [MemberData(nameof(GetAppropriateLengths))]
        public void BSideIsRetained(Double a, Double b, Double c)
        {
            var description = new TriangleDescription(a, b, c);

            var actualB = description.B;

            actualB.Should().Be(b);
        }

        [Theory]
        [MemberData(nameof(GetAppropriateLengths))]
        public void CSideIsRetained(Double a, Double b, Double c)
        {
            var description = new TriangleDescription(a, b, c);

            var actualC = description.C;

            actualC.Should().Be(c);
        }

        [Theory]
        [MemberData(nameof(GetRightTrianglesSidesLengths))]
        public void ChecksForBeingARightTriangleWorkForReasonablySmallTriangles(Double a, Double b, Double c)
        {
            var description = new TriangleDescription(a, b, c);

            var isRight = description.IsRight;

            isRight.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(GetNotRightTrianglesSidesLengths))]
        public void ChecksForBeingARightTriangleWorkForReasonablySmallTriangles2(Double a, Double b, Double c)
        {
            var description = new TriangleDescription(a, b, c);

            var isRight = description.IsRight;

            isRight.Should().BeFalse();
        }

        [Fact]
        public void ChecksForBeingARightTriangleThrowsForLargeTriangles()
        {
            var description = new TriangleDescription(Double.MaxValue / 8.0, Double.MaxValue / 4.0, Double.MaxValue / 4.0);

            var exception = Record.Exception(() => description.IsRight);

            exception.Should().NotBeNull().And.BeOfType<InvalidOperationException>();
        }

        [Theory]
        [MemberData(nameof(GetAppropriateLengths))]
        public void AreaIsCorrectlyCalculatedForReasonablySmallTriangles(Double a, Double b, Double c)
        {
            var description = new TriangleDescription(a, b, c);
            var p = (a + b + c) / 2;
            var expectedArea = Math.Sqrt(p * (p - a) * (p - b) * (p - c));

            var actualArea = description.CalculateArea();

            actualArea.Should().BeApproximately(expectedArea, 0.000_001);
        }

        [Fact]
        public void AreaCalclulationCanReturnInfinityForLargeCircles()
        {
            var description = new TriangleDescription(Double.MaxValue / 4.0, Double.MaxValue / 4.0, Double.MaxValue / 4.0);

            var expectedArea = Double.PositiveInfinity;

            var actualArea = description.CalculateArea();

            actualArea.Should().Be(expectedArea);
        }

        private const Int32 RIGHT_TRIANGLES_QUANTITY = 10;

        private const Int32 RIGHT_TRIANGLE_MIN_GENERATED_LENGTH = 1;

        private const Int32 RIGHT_TRIANGLE_MAX_GENERATED_SIDE_LENGTH = 10_000;

        public static IEnumerable<Object[]> GetAppropriateLengths()
        {
            yield return new Object[] { 2.0, 4.0, 5.0 };
            yield return new Object[] { 3.0, 4.0, 5.0 };
            yield return new Object[] { 4.0, 4.0, 5.0 };
            yield return new Object[] { 18.0, 12.0, 8.0 };
            yield return new Object[] { 2.0, 2.0, 2.0 };
        }

        public static IEnumerable<Object[]> GetRightTrianglesSidesLengths()
            => GenerateRightTrianglesOrderedSidesLengths()
                .Select(values => Shuffle(values));

        public static IEnumerable<Object[]> GetNotRightTrianglesSidesLengths()
            => GenerateRightTrianglesOrderedSidesLengths().Select(values =>
                Shuffle(new Object[]
                {
                    values[0],
                    values[1],
                    (Double) values[2] + 0.25
                }));

        private static IEnumerable<Object[]> GenerateRightTrianglesOrderedSidesLengths()
            => Enumerable.Range(0, RIGHT_TRIANGLES_QUANTITY).Select(_ =>
            {
                Int32 GenerateLength() => generator.Next(
                    RIGHT_TRIANGLE_MIN_GENERATED_LENGTH,
                    RIGHT_TRIANGLE_MAX_GENERATED_SIDE_LENGTH);

                var firstSide = GenerateLength();
                var secondSide = generator.Next() > 0 ? GenerateLength() : firstSide; // generate some isosceles triangles
                var thirdSide = Math.Sqrt(Math.Pow(firstSide, 2.0) + Math.Pow(secondSide, 2.0));
                return new Object[]
                {
                    (Double) firstSide,
                    (Double) secondSide,
                    thirdSide
                };
            });

        private static T[] Shuffle<T>(T[] array) => array.OrderBy(_ => generator.Next()).ToArray();

        public static IEnumerable<Object[]> GetInappropriateLengths()
        {
            yield return new Object[] { 1.0, 3.0, 5.0 };
            yield return new Object[] { 1.0, 30.0, 2.0 };
            yield return new Object[] { 1.0, Double.MaxValue, Double.MaxValue };
            yield return new Object[] { Double.MaxValue, 30.0, 2.0 };
        }

        public static IEnumerable<Object[]> GetLengthsWithNaN()
        {
            yield return new Object[] { Double.NaN, 3.0, 5.0 };
            yield return new Object[] { 1.0, Double.NaN, 2.0 };
            yield return new Object[] { 1.0, Double.MaxValue, Double.NaN };
            yield return new Object[] { Double.NaN, Double.NaN, 5.0 };
            yield return new Object[] { 1.0, Double.NaN, Double.NaN };
            yield return new Object[] { Double.NaN, 1.0, Double.NaN };
            yield return new Object[] { Double.NaN, Double.NaN, Double.NaN };
        }

        public static IEnumerable<Object[]> GetLengthsWithZero()
        {
            yield return new Object[] { 0.0, 3.0, 5.0 };
            yield return new Object[] { 1.0, 0.0, 2.0 };
            yield return new Object[] { 1.0, Double.MaxValue, 0.0 };
            yield return new Object[] { 0.0, 0.0, 5.0 };
            yield return new Object[] { 1.0, 0.0, 0.0 };
            yield return new Object[] { 0.0, 1.0, 0.0 };
            yield return new Object[] { 0.0, 0.0, 0.0 };
        }

        public static IEnumerable<Object[]> GetLengthsWithNegativeValue()
        {
            yield return new Object[] { -1.0, 3.0, 5.0 };
            yield return new Object[] { 1.0, -1.0, 2.0 };
            yield return new Object[] { 1.0, Double.MaxValue, -1.0 };
            yield return new Object[] { -1.0, -1.0, 5.0 };
            yield return new Object[] { 1.0, -1.0, -1.0 };
            yield return new Object[] { -1.0, 1.0, -1.0 };
            yield return new Object[] { -1.0, -1.0, -1.0 };
        }

        private static readonly Random generator = new Random();
    }
}
