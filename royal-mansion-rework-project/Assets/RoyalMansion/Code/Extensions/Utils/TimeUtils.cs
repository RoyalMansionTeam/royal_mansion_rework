using RoyalMasion.Code.Infrastructure.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoyalMansion.Code.Extensions.Utils
{

    [System.Serializable]
    public struct NormalizedDateTime
    {
        public int Day;
        public int Month;
        public int Year;
        public NormalizedTime Time;
        //Converts a struct to parsable DateTime string 
        public string ToDateTimeString()
        {
            return $"{Year}-{Month}-{Day}-{Time.ToDateTimeString()}";
        }
        public DateTime ToDateTime()
        {
            return new DateTime(Year, Month, Day,
                Time.Houres, Time.Minutes, Time.Seconds, DateTimeKind.Utc);
        }
    }
    

    [System.Serializable]
    public struct NormalizedTime
    {
        public int Houres;
        public int Minutes;
        public int Seconds;
        public float ToFloat()
        {
            return Seconds + Minutes * 60 + Houres * 60 * 60;
        }
        //Converts a struct to parsable DateTime string 
        public string ToDateTimeString()
        {
            return $"{Houres}-{Minutes}-{Seconds}";
        }

    }
}
