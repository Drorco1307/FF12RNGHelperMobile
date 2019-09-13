using FF12RngHelper.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace FF12RngHelper
{
    /// <summary>
    /// A converter that takes in an enumeration of <see cref="SpellTypes"/>
    /// and converts them to a <see cref="List{T}"/> where T is <see cref="string"/>, based on each value's <see cref="DescriptionAttribute"/>
    /// </summary>
    public class EnumToDisplayStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is List<SpellTypes> spellTypes)
            {
                List<string> spells = (from SpellTypes spell in spellTypes
                                       select spell.GetDiscription()).ToList();
                return spells;
            }
            else
                return ((SpellTypes)value).GetDiscription();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return GetValue<SpellTypes>(value.ToString());
        }

        /// <summary>
        /// Gets an enumeration's value from it's <see cref="DescriptionAttribute"/> value
        /// </summary>
        /// <typeparam name="T">The type of enumeration</typeparam>
        /// <param name="description">the string value of the <see cref="DescriptionAttribute"/></param>
        /// <returns></returns>
        public T GetValue<T>(string description)
        {
            foreach (var field in typeof(T).GetFields())
            {
                DescriptionAttribute[] descriptions = field.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
                if (descriptions.Any(x => x.Description == description))
                {
                    return (T)field.GetValue(null);
                }
            }
            return (T)Enum.Parse(typeof(T), description);
        }
    }
}
