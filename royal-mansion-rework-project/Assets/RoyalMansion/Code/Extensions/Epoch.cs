using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoyalMansion.Code.Extensions
{
    public static class Epoch
    {
        public static float Current() => 
            (DateTime.UtcNow.Hour * 60 * 60 + DateTime.UtcNow.Minute * 60 + DateTime.UtcNow.Second);

        public static float SecondsElapsed(float t1)
        {
            float difference = t1 - Current();

            return difference;
        }

        public static float SecondsElapsed(float t1, float t2)
        {
            float difference = t1 - t2;

            return Mathf.Abs(difference);
        }
    }
}