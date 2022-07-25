using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palisades.Helpers
{
    [AttributeUsage(AttributeTargets.Assembly)]
    internal class ReleaseDateAttribute : Attribute
    {
        public ReleaseDateAttribute(string value)
        {
            DateTime = DateTime.ParseExact(value, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None);
        }

        public DateTime DateTime { get; }
    }
}
