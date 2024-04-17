using System;

namespace Assets.Scripts.Utility
{
    public static class Extensions
    {
        public static bool ApproximatelyEqual(this float number, float comparable, float acceptableDifference)
        {
            return Math.Abs(number - comparable) <= acceptableDifference;
        }
    }
}