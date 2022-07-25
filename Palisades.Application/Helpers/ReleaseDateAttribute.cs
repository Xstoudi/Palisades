using System;
using System.Globalization;

namespace Palisades.Helpers
{
    [AttributeUsage(AttributeTargets.Assembly)]
    internal class ReleaseDateAttribute : Attribute
    {
        internal ReleaseDateAttribute(string value)
        {
            DateTime = DateTime.ParseExact(value, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None);
        }

        internal DateTime DateTime { get; }
    }
}
