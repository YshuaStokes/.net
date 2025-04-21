using System;
using System.ComponentModel;

namespace MTDataAccess
{
    public enum TimeSignatureType
    {
        [Description("4/4")]
        CommonTime = 3,
        
        [Description("6/8")]
        SixEight = 13
    }
    
    public static class TimeSignatureExtensions
    {
        public static string ToDisplayString(this TimeSignatureType timeSignature)
        {
            switch (timeSignature)
            {
                case TimeSignatureType.CommonTime:
                    return "4/4";
                case TimeSignatureType.SixEight:
                    return "6/8";
                default:
                    return timeSignature.ToString();
            }
        }
        
        public static TimeSignatureType FromInt(int value)
        {
            if (Enum.IsDefined(typeof(TimeSignatureType), value))
            {
                return (TimeSignatureType)value;
            }
            
            // Default to CommonTime if the value is not recognized
            return TimeSignatureType.CommonTime;
        }
        
        public static string GetDisplayString(int value)
        {
            return FromInt(value).ToDisplayString();
        }
    }
}