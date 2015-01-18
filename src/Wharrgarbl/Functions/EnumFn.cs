﻿namespace Wharrgarbl.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class EnumFn
    {
        public static TEnum ParseEnum<TEnum>(string value) where TEnum : struct
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value);
        }

        public static TEnum ParseEnum<TEnum>(string value, bool ignoreCase) where TEnum : struct
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);
        }

        public static TEnum? ParseEnumOrNull<TEnum>(string value) where TEnum : struct
        {
            TEnum result;
            if (Enum.TryParse(value, out result))
                return result;
            return default(TEnum?);
        }

        public static TEnum? ParseEnumOrNull<TEnum>(string value, bool ignoreCase) where TEnum : struct
        {
            TEnum result;
            if (Enum.TryParse(value, ignoreCase, out result))
                return result;
            return default(TEnum?);
        }
    }
}