using System;

namespace AreaCalculator
{
    ///<summary> Discribes a triangle </summary>
    ///<remarks> Default constructor does not uphold the type's invariant </remarks>
    public readonly struct TriangleDescription : ICanCalculateArea
    {
        public Double A { get; }

        public Double B { get; }

        public Double C { get; }

        ///<summary> Creates a new instance of <see cref="TriangleDescription"/> structure </summary>
        ///<param name="a"> the length of the first side </param>
        ///<param name="b"> the length of the second side </param>
        ///<param name="c"> the length of the third side </param>
        ///<exception cref="ArgumentOutOfRangeException"> for negative or infinite sides' lengths </exception>
        ///<exception cref="ArgumentException"> for <see cref="Double.NaN"/> sides' lengths
        /// or when the triangle cannot be created with provided sides </exception>
        public TriangleDescription(Double a, Double b, Double c)
        {
            void CheckParameters()
            {
                void CheckSide(ref Double side)
                {
                    if (Double.IsInfinity(side))
                    {
                        throw new ArgumentOutOfRangeException(
                            nameof(side),
                            "A triange cannot have an infinite side.");
                    }

                    if (Double.IsNaN(side))
                    {
                        throw new ArgumentException(
                            "Triangle's side's length cannot be represented by a \"Not a Number\" value.",
                            nameof(side));

                    }

                    if (side <= 0.0)
                    {
                        throw new ArgumentOutOfRangeException(
                            nameof(side),
                            "Triangle's side's length should be represented by a positive value.");
                    }
                }

                CheckSide(ref a);
                CheckSide(ref b);
                CheckSide(ref c);
            }

            void CheckTriangleExistence()
            {
                var ab = a + b;
                var bc = b + c;
                var ac = a + c;

                if (Double.IsInfinity(ab) || Double.IsInfinity(bc) || Double.IsInfinity(ac))
                {
                    throw new ArgumentException("The triangle is too large.");
                }

                if (ab <= c || bc <= a || ac <= b)
                {
                    throw new ArgumentException("Such a triangle cannot exist.");
                }
            }

            CheckParameters();
            CheckTriangleExistence();

            A = a;
            B = b;
            C = c;
        }

        ///<summary> Checks whether the triangle is right </summary>
        ///<returns> <c>true</c> for right triangles, and <c>false</c> otherwise
        ///<remarks> Uses absolute hardcoded error margin,
        /// the ULP or Epsilon overloads could be provided if needed </remarks>
        ///<exception cref="InvalidOperationException"> if triangle is too large to check </exception>
        public Boolean IsRight
        {
            get
            {
                (Double Left, Double Right) Evaluate(Double a, Double b, Double c)
                {
                    var max = Math.Max(a, Math.Max(b, c));

#pragma warning disable CS8509
                    var (min1, min2) = max switch
                    {
                        _ when max == a => (b, c),
                        _ when max == b => (a, c),
                        _ when max == c => (a, b)
                    };
#pragma warning restore CS8509
                    return (Math.Pow(max, 2.0), Math.Pow(min1, 2.0) + Math.Pow(min2, 2.0));
                }

                const Double MAX_DIFFERENCE = 0.000_001;

                var (left, right) = Evaluate(A, B, C);

                if (Double.IsInfinity(left) || Double.IsInfinity(right))
                {
                    throw new InvalidOperationException("The triangle is too large to check by Pythagorean theorem.");
                }

                return Math.Abs(left - right) <= MAX_DIFFERENCE;
            }
        }

        public Double CalculateArea()
        {
            var p = (A + B + C) / 2;
            return Math.Sqrt(p * (p - A) * (p - B) * (p - C));
        }
    }
}
