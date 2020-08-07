using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ConciliaPraGente.Infra
{
    public static class EnumExtension
    {
        public static string ToDescription(this Enum en, bool toUpper = true)
        {
            var type = en.GetType();

            var memInfo = type.GetMember(en.ToString());

            if (memInfo.Length <= 0) return en.ToString();
            var attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            var description = attrs.Length > 0 ? ((DescriptionAttribute)attrs[0]).Description : en.ToString();
            return toUpper ? description.ToUpper() : description;
        }

        public static IList<T> ToList<T>(this T theEnum) where T : IFormattable, IConvertible
        {
            return Enumerable.ToList(Enum.GetValues(theEnum.GetType()).Cast<T>());
        }
    }
}