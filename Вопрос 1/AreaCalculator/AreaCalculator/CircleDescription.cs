using System;

namespace AreaCalculator
{
    ///<summary> Discribes a circle </summary>
    ///<remarks> Default constructor does not uphold the type's invariant </remarks>
    public readonly struct CircleDescription : ICanCalculateArea
    {
        public Double Radius { get; }

        ///<summary> Creates a new instance of <see cref="CircleDescription"/> structure </summary>
        ///<param name="radius"> the radius of a circle </param>
        ///<exception cref="ArgumentOutOfRangeException"> for negative or infinite radius </exception>
        ///<exception cref="ArgumentException"> for <see cref="Double.NaN"/> radius </exception>
        public CircleDescription(Double radius)
        {
            void CheckParameters()
            {
                if (Double.IsInfinity(radius))
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(radius),
                        "A circle should have a finite radius.");
                }

                if (Double.IsNaN(radius))
                {
                    throw new ArgumentException(
                        "A circle's radius cannot be represented by a \"Not a Number\" value.",
                        nameof(radius));

                }

                if (radius <= 0.0)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(radius),
                        "Radius should be represented by a positive value.");
                }
            }

            CheckParameters();

            Radius = radius;
        }

        public Double CalculateArea() => Math.PI * Math.Pow(Radius, 2.0);
    }
}