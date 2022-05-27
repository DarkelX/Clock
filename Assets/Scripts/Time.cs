using UnityEngine;

namespace Scripts
{
    public class Time : UnityEngine.Time
    {
        public static float GetTimeInSeconds(float hours, float minutes, float seconds)
        {
            return hours * 3600 + minutes * 60 + seconds;
        }
    }
}