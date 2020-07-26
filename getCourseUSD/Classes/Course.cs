using System;
using System.Collections.Generic;
using System.Linq;
namespace getCourseUSD.Classes
{
    class Course
    {
        public float Current { get; private set; }
        public float Previous { get; private set; }
        public float DifferenceValue { get; private set; }
        public float DifferenceInPersent { get; private set; }
        
        public Course()
        {
            Current = 0f;
            Previous = 0f;
            DifferenceValue = 0f;
            DifferenceInPersent = 0f;
        }

        public Course(float current, float previous)
        {
            Current = current;
            Previous = previous;
            DifferenceValue = Math.Abs(Current - Previous);
            DifferenceInPersent = DifferenceValue / Current * 100;
        }

        public override string ToString()
        {
            string current = Rounding(Current);
            string previous = Rounding(Previous);
            string differenceValue = Rounding(DifferenceValue, "{0:f1}");
            string differenceInPercent = Rounding(DifferenceInPersent, "{0:f2}");
            return $"Current: {current}\n" +
                $"Previous: {previous}\n" +
                $"Differens Value: {differenceValue}\n" +
                $"Differens in persent: {differenceInPercent}";
        }

        private string Rounding(float number, string format = "{0:f2}")
        {
            return String.Format(format, number);
        }
    }
}
