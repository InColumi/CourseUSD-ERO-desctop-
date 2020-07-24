using System;
using System.Collections.Generic;
using System.Linq;
namespace getCourseUSD.Classes
{
    class Course
    {
        public float Current { get; private set; }
        public float Previous { get; private set; }
        public float Difference { get; private set; }

        public Course()
        {
            Current = 0f;
            Previous = 0f;
            Difference = 0f;
        }

        public Course(float current, float previous)
        {
            Current = current;
            Previous = previous;
            Difference = current / previous;
        }

        public override string ToString()
        {
            string current = Rounding(Current);
            string previous = Rounding(Previous);
            string difference = Rounding(Difference, "{0:f1}");

            return $"Current: {current}\n" +
                $"Previous: {previous}\n" +
                $"Differens: {difference}";
        }

        private string Rounding(float number, string format = "{0:f2}")
        {
            return String.Format(format, number);
        }
    }
}
