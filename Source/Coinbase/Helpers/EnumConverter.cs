using System;
using System.Globalization;

namespace Coinbase.Helpers
{
    public static class EnumConverter
    {
        public static T ToEnum<T>(this string enumString) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            foreach (T item in Enum.GetValues(typeof(T)))
                if (item.ToString(CultureInfo.InvariantCulture).ToLower().Equals(enumString.Trim().ToLower()))
                    return item;

            throw new ArgumentException($"{nameof(enumString)} is not a valid value");
        }
    }

}
