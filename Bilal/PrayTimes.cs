using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bilal
{
    public class PrayTimes
    {
    }

    public enum TimeNames
    {
        Imsak,

        Fajr,

        Sunrise,

        Dhuhr,

        Asr,

        Sunset,

        Maghrib,

        Isha,

        Midnight
    }

    public class CalculationMethod
    {
        public string Name { get; set; }

        public Params Params { get; set; }
    }

    public class Params
    {
        public double Fajr { get; set; }
        public double Isha { get; set; }
    }
}
