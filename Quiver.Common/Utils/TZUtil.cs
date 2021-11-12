using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.Common.Utils
{
    public class TZUtil
    {
        public static DateTime GetDataHoraDeBrasilia()
        {
            DateTime dateTime = DateTime.UtcNow;
            TimeZoneInfo hrBrasilia = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, hrBrasilia);
        }

        public static DateTime GetDataDeBrasilia()
        {
            var dataHoraDeBrasilia = GetDataHoraDeBrasilia();
            return new DateTime(dataHoraDeBrasilia.Year, dataHoraDeBrasilia.Month, dataHoraDeBrasilia.Day, 0, 0, 0);
        }
    }
}
