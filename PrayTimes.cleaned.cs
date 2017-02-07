using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace WpfApplication1.Coversion
{
    public class PrayTimes
    {
        #region Static Fields and Constants

        private MethodName calcMethod;

        public double elv;

        public string invalidTime = "-----";

        public double jDate;

        //----------------------- Local Variables ---------------------
        public double lat;

        public double lng;

        public double numIterations = 1;

        public string timeFormat = "24h";

        public string[] timeSuffixes =
            {
                "am", "pm"
            };

        // coordinates
        public double timeZone;

        #endregion

        #region Constructors

        // time variables

        //------------------------ Constants --------------------------

        public PrayTimes(MethodName method = MethodName.MWL)
        {
            // Time Names
            this.timeNames = new[]
                                 {
                                     TimeName.Imsak, TimeName.Fajr, TimeName.Sunrise, TimeName.Dhuhr, TimeName.Asr, TimeName.Sunset, TimeName.Maghrib, TimeName.Isha,
                                     TimeName.Midnight
                                 };

            // Calculation Methods
            this.methods = new Dictionary<MethodName, Method>
                               {
                                   {
                                       MethodName.MWL, new Method
                                                           {
                                                               name = "Muslim World League",
                                                               _params = new Dictionary<TimeName, object>
                                                                             {
                                                                                 {
                                                                                     TimeName.Fajr, 18
                                                                                 },
                                                                                 {
                                                                                     TimeName.Isha, 17
                                                                                 }
                                                                             }
                                                           }
                                   },
                                   {
                                       MethodName.ISNA, new Method
                                                            {
                                                                name = "Islamic Society of North America (ISNA)",
                                                                _params = new Dictionary<TimeName, object>
                                                                              {
                                                                                  {
                                                                                      TimeName.Fajr, 15
                                                                                  },
                                                                                  {
                                                                                      TimeName.Isha, 15
                                                                                  }
                                                                              }
                                                            }
                                   },
                                   {
                                       MethodName.Egypt, new Method
                                                             {
                                                                 name = "Egyptian General Authority of Survey",
                                                                 _params = new Dictionary<TimeName, object>
                                                                               {
                                                                                   {
                                                                                       TimeName.Fajr, 19.5
                                                                                   },
                                                                                   {
                                                                                       TimeName.Isha, 17.5
                                                                                   }
                                                                               }
                                                             }
                                   },
                                   {
                                       MethodName.Makkah, new Method
                                                              {
                                                                  name = "Umm Al-Qura University, Makkah",
                                                                  _params = new Dictionary<TimeName, object>
                                                                                {
                                                                                    {
                                                                                        TimeName.Fajr, 18.5
                                                                                    },
                                                                                    {
                                                                                        TimeName.Isha, "90 min"
                                                                                    }
                                                                                }
                                                              }
                                   }, // fajr was 19 degrees before 1430 hijri
                                   {
                                       MethodName.Karachi, new Method
                                                               {
                                                                   name = "University of Islamic Sciences, Karachi",
                                                                   _params = new Dictionary<TimeName, object>
                                                                                 {
                                                                                     {
                                                                                         TimeName.Fajr, 18
                                                                                     },
                                                                                     {
                                                                                         TimeName.Isha, 18
                                                                                     }
                                                                                 }
                                                               }
                                   },
                                   {
                                       MethodName.Tehran, new Method
                                                              {
                                                                  name = "Institute of Geophysics, University of Tehran",
                                                                  _params = new Dictionary<TimeName, object>
                                                                                {
                                                                                    {
                                                                                        TimeName.Fajr, 17.7
                                                                                    },
                                                                                    {
                                                                                        TimeName.Isha, 14
                                                                                    },
                                                                                    {
                                                                                        TimeName.Maghrib, 4.5
                                                                                    },
                                                                                    {
                                                                                        TimeName.Midnight, "Jafari"
                                                                                    }
                                                                                }
                                                              }
                                   },
                                   // isha is not explicitly specified in this method
                                   {
                                       MethodName.Jafari, new Method
                                                              {
                                                                  name = "Shia Ithna-Ashari, Leva Institute, Qum",
                                                                  _params = new Dictionary<TimeName, object>
                                                                                {
                                                                                    {
                                                                                        TimeName.Fajr, 16
                                                                                    },
                                                                                    {
                                                                                        TimeName.Isha, 14
                                                                                    },
                                                                                    {
                                                                                        TimeName.Maghrib, 4
                                                                                    },
                                                                                    {
                                                                                        TimeName.Midnight, "Jafari"
                                                                                    }
                                                                                }
                                                              }
                                   }
                               };

            // Default Parameters in Calculation Methods
            this.defaultParams = new Dictionary<TimeName, object>
                                     {
                                         {
                                             TimeName.Maghrib, "0 min"
                                         },
                                         {
                                             TimeName.Midnight, "Standard"
                                         }
                                     };

            //----------------------- Parameter Values ----------------------
            /*

        // Asr Juristic Methods
        asrJuristics = [ 
            "Standard",    // Shafi`i, Maliki, Ja`fari, Hanbali
            "Hanafi"       // Hanafi
        ],


        // Midnight Mode
        midnightMethods = [ 
            "Standard",    // Mid Sunset to Sunrise
            "Jafari"       // Mid Sunset to Fajr
        ],


        // Adjust Methods for Higher Latitudes
        highLatMethods = [
            "NightMiddle", // middle of night
            "AngleBased",  // angle/60th of night
            "OneSeventh",  // 1/7th of night
            "None"         // No adjustment
        ],


        // Time Formats
        timeFormats = [
            "24h",         // 24-hour format
            "12h",         // 12-hour format
            "12hNS",       // 12-hour format with no suffix
            "Float"        // floating point number 
        ],
        */

            //---------------------- Default Settings --------------------
            // do not change anything here; use adjust method instead
            this.setting = new Dictionary<TimeName, object>
                               {
                                   {
                                       TimeName.Imsak, "10 min"
                                   },
                                   {
                                       TimeName.Dhuhr, "0 min"
                                   },
                                   {
                                       TimeName.Asr, AsrMethod.Standard
                                   },
                                   {
                                       TimeName.HighLats, "NightMiddle"
                                   }
                               };

            //---------------------- Initialization -----------------------
            // set methods defaults
            foreach (var method1 in this.methods)
            {
                var _paramss = method1.Value._params;
                var defParams = this.defaultParams.Where(p => !_paramss.ContainsKey(p.Key));
                foreach (var keyValuePair in defParams)
                {
                    _paramss.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }

            // initialize settings
            var _params = this.methods[method]._params;
            foreach (var param in _params)
            {
                this.setting.Add(param.Key, param.Value);
            }

            // init time offsets

            this.offset = this.timeNames.ToDictionary(i => i, i => 0.0);
        }

        #endregion

        #region Public Properties and Indexers

        //--------------------------- Mine ----------------------------
        public TimeName[] timeNames { get; set; }

        public Dictionary<MethodName, Method> methods { get; set; }

        public Dictionary<TimeName, object> defaultParams { get; set; }

        public Dictionary<TimeName, object> setting { get; set; }

        public Dictionary<TimeName, double> offset { get; set; }

        public MethodName CalcMethod
        {
            get
            {
                return this.calcMethod;
            }
            set
            {
                if (this.calcMethod == value) return;

                this.adjust(this.methods[value]._params);
                this.calcMethod = value;
            }
        }

        #endregion

        // set calculation method 
        public void setMethod(MethodName method)
        {
            if (this.methods.ContainsKey(method))
            {
                this.calcMethod = method;
            }
        }

        // set calculating parameters
        public void adjust(Dictionary<TimeName, object> paramss)
        {
            foreach (var id in paramss)
            {
                this.setting[id.Key] = id.Value;
            }
        }

        // set time offsets
        public void tune(double[] timeOffsets)
        {
            for (int i = 0; i < timeOffsets.Length; i++)
            {
                var element = this.offset.ElementAt(i);
                this.offset[element.Key] = timeOffsets[i];
            }
        }

        // get current calculation method
        public MethodName getMethod()
        {
            return this.calcMethod;
        }

        // get current setting
        public Dictionary<TimeName, object> getSetting()
        {
            return this.setting;
        }

        // get current time offsets
        public Dictionary<TimeName, double> getOffsets()
        {
            return this.offset;
        }

        // get default calc parametrs
        public Dictionary<MethodName, Method> getDefaults()
        {
            return this.methods;
        }

        // return prayer times foreach a given date
        public Dictionary<TimeName, double> getTimes(DateTime date, GeoCoordinate coords, double? timezone = null, double? dst = null, string format = null)
        {
            this.lat = 1 * coords.Latitude;
            this.lng = 1 * coords.Longitude;
            this.elv = 1 * coords.Elevation ?? 0;
            this.timeFormat = !string.IsNullOrEmpty(format) ? format : this.timeFormat;

            if (!timezone.HasValue) timezone = this.getTimeZone(date);
            if (!dst.HasValue) dst = this.getDst(date);
            this.timeZone = 1.0 * timezone.Value + 1.0 * dst.Value;
            this.jDate = this.julian(date.Year, date.Month, date.Day) - this.lng / (15 * 24);

            return this.computeTimes();
        }

        // convert float time to the given format (see timeFormats)
        public string getFormattedTime(double time, string format, string[] suffixes = null)
        {
            if (double.IsNaN(time)) throw new Exception(this.invalidTime);
            if (format == "Float") return time.ToString(CultureInfo.InvariantCulture);
            suffixes = suffixes ?? this.timeSuffixes;

            time = DMath.fixHour(time + 0.5 / 60); // add 0.5 minutes to round
            var hours = Math.Floor(time);
            var minutes = Math.Floor((time - hours) * 60);
            var suffix = format == "12h" ? suffixes[hours < 12 ? 0 : 1] : string.Empty;
            var hour = format == "24h" ? this.twoDigitsFormat(hours) : ((hours + 12 - 1) % 12 + 1).ToString(CultureInfo.InvariantCulture);
            return hour + ":" + this.twoDigitsFormat(minutes) + (!string.IsNullOrEmpty(suffix) ? " " + suffix : string.Empty);
        }

        //---------------------- Calculation Functions -----------------------

        // compute mid-day time
        public double midDay(double time)
        {
            var eqt = this.sunPosition(this.jDate + time).equation;
            var noon = DMath.fixHour(12 - eqt);
            return noon;
        }

        // compute the time at which sun reaches a specific angle below horizon
        public double sunAngleTime(double angle, double time, string direction = null)
        {
            var decl = this.sunPosition(this.jDate + time).declination;
            var noon = this.midDay(time);
            var t = 1.0 / 15.0 * DMath.arccos((-DMath.sin(angle) - DMath.sin(decl) * DMath.sin(this.lat)) / (DMath.cos(decl) * DMath.cos(this.lat)));
            return noon + (direction == "ccw" ? -t : t);
        }

        // compute asr time 
        public double asrTime(double factor, double time)
        {
            var decl = this.sunPosition(this.jDate + time).declination;
            var angle = -DMath.arccot(factor + DMath.tan(Math.Abs(this.lat - decl)));
            return this.sunAngleTime(angle, time);
        }

        // compute declination angle of sun and equation of time
        // Ref: http://aa.usno.navy.mil/faq/docs/SunApprox.php
        public SunPosition sunPosition(double jd)
        {
            var D = jd - 2451545.0;
            var g = DMath.fixAngle(357.529 + 0.98560028 * D);
            var q = DMath.fixAngle(280.459 + 0.98564736 * D);
            var L = DMath.fixAngle(q + 1.915 * DMath.sin(g) + 0.020 * DMath.sin(2 * g));

            var R = 1.00014 - 0.01671 * DMath.cos(g) - 0.00014 * DMath.cos(2 * g);
            var e = 23.439 - 0.00000036 * D;

            var RA = DMath.arctan2(DMath.cos(e) * DMath.sin(L), DMath.cos(L)) / 15;
            var eqt = q / 15 - DMath.fixHour(RA);
            var decl = DMath.arcsin(DMath.sin(e) * DMath.sin(L));

            return new SunPosition(decl, eqt);
        }

        // convert Gregorian date to Julian day
        // Ref: Astronomical Algorithms by Jean Meeus
        public double julian(double year, double month, double day)
        {
            if (month <= 2)
            {
                year -= 1;
                month += 12;
            }
            var A = Math.Floor(year / 100.0);
            var B = 2 - A + Math.Floor(A / 4.0);

            var JD = Math.Floor(365.25 * (year + 4716)) + Math.Floor(30.6001 * (month + 1.0)) + day + B - 1524.5;
            return JD;
        }

        //---------------------- Compute Prayer Times -----------------------

        // compute prayer times at given julian date
        public Dictionary<TimeName, double> computePrayerTimes(Dictionary<TimeName, double> times)
        {
            times = this.dayPortion(times);
            var paramss = this.setting;

            var imsak = this.sunAngleTime(this.eval(paramss[TimeName.Imsak]), times[TimeName.Imsak], "ccw");
            var fajr = this.sunAngleTime(this.eval(paramss[TimeName.Fajr]), times[TimeName.Fajr], "ccw");
            var sunrise = this.sunAngleTime(this.riseSetAngle(), times[TimeName.Sunrise], "ccw");
            var dhuhr = this.midDay(times[TimeName.Dhuhr]);
            var asr = this.asrTime(this.asrFactor((AsrMethod)paramss[TimeName.Asr]), times[TimeName.Asr]);
            var sunset = this.sunAngleTime(this.riseSetAngle(), times[TimeName.Sunset]);
            var maghrib = this.sunAngleTime(this.eval(paramss[TimeName.Maghrib]), times[TimeName.Maghrib]);
            var isha = this.sunAngleTime(this.eval(paramss[TimeName.Isha]), times[TimeName.Isha]);

            return new Dictionary<TimeName, double>
                       {
                           {
                               TimeName.Imsak, imsak
                           },
                           {
                               TimeName.Fajr, fajr
                           },
                           {
                               TimeName.Sunrise, sunrise
                           },
                           {
                               TimeName.Dhuhr, dhuhr
                           },
                           {
                               TimeName.Asr, asr
                           },
                           {
                               TimeName.Sunset, sunset
                           },
                           {
                               TimeName.Maghrib, maghrib
                           },
                           {
                               TimeName.Isha, isha
                           }
                       };
        }

        // compute prayer times 
        public Dictionary<TimeName, double> computeTimes()
        {
            // default times
            var times = new Dictionary<TimeName, double>
                            {
                                {
                                    TimeName.Imsak, 5
                                },
                                {
                                    TimeName.Fajr, 5
                                },
                                {
                                    TimeName.Sunrise, 6
                                },
                                {
                                    TimeName.Dhuhr, 12
                                },
                                {
                                    TimeName.Asr, 13
                                },
                                {
                                    TimeName.Sunset, 18
                                },
                                {
                                    TimeName.Maghrib, 18
                                },
                                {
                                    TimeName.Isha, 18
                                }
                            };

            // main iterations
            for (var i = 1; i <= this.numIterations; i++)
            {
                times = this.computePrayerTimes(times);
            }

            times = this.adjustTimes(times);

            // add midnight time
            times[TimeName.Midnight] = (string)this.setting[TimeName.Midnight] == "Jafari"
                                           ? times[TimeName.Sunset] + this.timeDiff(times[TimeName.Sunset], times[TimeName.Fajr]) / 2
                                           : times[TimeName.Sunset] + this.timeDiff(times[TimeName.Sunset], times[TimeName.Sunrise]) / 2;

            times = this.tuneTimes(times);
            return this.modifyFormats(times);
        }

        // adjust times 
        public Dictionary<TimeName, double> adjustTimes(Dictionary<TimeName, double> times)
        {
            var paramss = this.setting;

            for (int i = 0; i < times.Count; i++)
            {
                times[times.ElementAt(i).Key] += this.timeZone - this.lng / 15;
            }

            if ((string)paramss[TimeName.HighLats] != "None") times = this.adjustHighLats(times);

            if (this.isMin(paramss[TimeName.Imsak])) times[TimeName.Imsak] = times[TimeName.Fajr] - this.eval(paramss[TimeName.Imsak]) / 60;
            if (this.isMin(paramss[TimeName.Maghrib])) times[TimeName.Maghrib] = times[TimeName.Sunset] + this.eval(paramss[TimeName.Maghrib]) / 60;
            if (this.isMin(paramss[TimeName.Isha])) times[TimeName.Isha] = times[TimeName.Maghrib] + this.eval(paramss[TimeName.Isha]) / 60;
            times[TimeName.Dhuhr] += this.eval(paramss[TimeName.Dhuhr]) / 60;

            return times;
        }

        // get asr shadow factor
        public double asrFactor(AsrMethod asrParam)
        {
            var factor = new Dictionary<AsrMethod, double>
                             {
                                 {
                                     AsrMethod.Standard, 1
                                 },
                                 {
                                     AsrMethod.Hanafi, 2
                                 }
                             };
            //[asrParam];
            //return factor || this.eval(asrParam);
            return factor[asrParam];
        }

        // return sun angle foreach sunset/sunrise
        public double riseSetAngle()
        {
            //var earthRad = 6371009; // in meters
            //var angle = DMath.arccos(earthRad/(earthRad+ elv));
            var angle = 0.0347 * Math.Sqrt(this.elv); // an approximation
            return 0.833 + angle;
        }

        // apply offsets to the times
        public Dictionary<TimeName, double> tuneTimes(Dictionary<TimeName, double> times)
        {
            for (int i = 0; i < times.Count; i++)
            {
                var element = times.ElementAt(i);
                times[element.Key] += this.offset[element.Key] / 60.0;
            }
            return times;
        }

        // convert times to given time format
        public Dictionary<TimeName, double> modifyFormats(Dictionary<TimeName, double> times)
        {
            for (int i = 0; i < times.Count; i++)
            {
                var element = times.ElementAt(i);
                //times[i.Key] = this.getFormattedTime(times[i.Key], timeFormat);
                var formattedTime = this.getFormattedTime(times[element.Key], this.timeFormat);
                times[element.Key] = Convert.ToDouble(this.getFormattedTime(times[element.Key], this.timeFormat));
            }
            return times;
        }

        // adjust times foreach locations in higher latitudes
        public Dictionary<TimeName, double> adjustHighLats(Dictionary<TimeName, double> times)
        {
            var paramss = this.setting;
            var nightTime = this.timeDiff(times[TimeName.Sunset], times[TimeName.Sunrise]);

            times[TimeName.Imsak] = this.adjustHLTime(times[TimeName.Imsak], times[TimeName.Sunrise], this.eval(paramss[TimeName.Imsak]), nightTime, "ccw");
            times[TimeName.Fajr] = this.adjustHLTime(times[TimeName.Fajr], times[TimeName.Sunrise], this.eval(paramss[TimeName.Fajr]), nightTime, "ccw");
            times[TimeName.Isha] = this.adjustHLTime(times[TimeName.Isha], times[TimeName.Sunset], this.eval(paramss[TimeName.Isha]), nightTime);
            times[TimeName.Maghrib] = this.adjustHLTime(times[TimeName.Maghrib], times[TimeName.Sunset], this.eval(paramss[TimeName.Maghrib]), nightTime);

            return times;
        }

        // adjust a time foreach higher latitudes
        public double adjustHLTime(double time, double _base, double angle, double night, string direction = null)
        {
            var portion = this.nightPortion(angle, night);
            var timeDiff = direction == "ccw" ? this.timeDiff(time, _base) : this.timeDiff(_base, time);
            if (double.IsNaN(time) || timeDiff > portion) time = _base + (direction == "ccw" ? -portion : portion);
            return time;
        }

        // the night portion used foreach adjusting times in higher latitudes
        public double nightPortion(double angle, double night)
        {
            var method = (string)this.setting[TimeName.HighLats];
            var portion = 1.0 / 2.0; // MidNight

            if (method == "AngleBased") portion = 1.0 / 60.0 * angle;
            if (method == "OneSeventh") portion = 1.0 / 7.0;
            return portion * night;
        }

        // convert hours to day portions 
        public Dictionary<TimeName, double> dayPortion(Dictionary<TimeName, double> times)
        {
            for (int i = 0; i < times.Count; i++)
            {
                times[times.ElementAt(i).Key] /= 24;
            }
            return times;
        }

        //---------------------- Time Zone Functions -----------------------

        // get local time zone
        public double getTimeZone(DateTime date)
        {
            var year = date.Year;
            double t1 = this.gmtOffset(new DateTime(year, 0, 1));
            double t2 = this.gmtOffset(new DateTime(year, 6, 1));
            return Math.Min(t1, t2);
        }

        // get daylight saving foreach a given date
        public double getDst(DateTime date)
        {
            //return 1 * (this.gmtOffset(date) != this.getTimeZone(date));
            return 1 * this.getTimeZone(date);
        }

        // GMT offset foreach a given date
        public double gmtOffset(DateTime date)
        {
            var localDate = new DateTime(date.Year, date.Month - 1, date.Day, 12, 0, 0, 0);
            var GMTString = localDate.ToUniversalTime().ToString(CultureInfo.InvariantCulture);
            var GMTDate = DateTime.Parse(GMTString.Substring(0, GMTString.LastIndexOf(" ", StringComparison.Ordinal) - 1));
            var hoursDiff = (localDate - GMTDate).TotalHours / (1000.0 * 60.0 * 60.0);
            return hoursDiff;
        }

        //---------------------- Misc Functions -----------------------

        // convert given string into a number
        public double eval(object str)
        {
            return 1 * double.Parse(Regex.Split(str + string.Empty, "[^0-9.+-]")[0]);
        }

        // detect if input contains "min"
        public bool isMin(object arg)
        {
            return (arg + string.Empty).IndexOf("min", StringComparison.Ordinal) != -1;
        }

        // compute the difference between two times 
        public double timeDiff(double time1, double time2)
        {
            return DMath.fixHour(time2 - time1);
        }

        // add a leading 0 if necessary
        public string twoDigitsFormat(double num)
        {
            return num < 10 ? "0" + num : num.ToString(CultureInfo.InvariantCulture);
        }
    }
}
