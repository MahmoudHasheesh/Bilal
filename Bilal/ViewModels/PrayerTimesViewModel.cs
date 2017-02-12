// ***********************************************************************
// Assembly         : Bilal
// Author           : Mahmoud Hasheesh
// Created          : 01-16-2017
//
// Last Modified By : Mahmoud Hasheesh
// Last Modified On : 01-16-2017
// ***********************************************************************
// <copyright file="MainPageViewModel.cs" company="Mahmoud Hasheesh">
//     Licensed under the MIT License. See License.txt in the project root for license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Bilal
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    using Plugin.Geolocator;
    using Plugin.Geolocator.Abstractions;

    using PrayerTimes;

    /// <summary>
    ///     Class MainPageViewModel.
    /// </summary>
    public class PrayerTimesViewModel : ViewModelBase
    {
        readonly IGeolocator locator;

        private string asr;

        private string date;

        private string dhuhr;

        private string fajr;

        private double heading;

        private string isha;

        private string maghrib;

        private string sunrise;

        private string sunset;

        public PrayerTimesViewModel()
        {
            this.RefreshCommand = new RelayCommand(this.OnRefreshExecuted);
            this.RefreshCommand.Execute(null);

            this.locator = CrossGeolocator.Current;
            this.locator.DesiredAccuracy = 50;
            this.locator.PositionChanged += this.LocatorOnPositionChanged;
        }

        public string Isha
        {
            get
            {
                return this.isha;
            }
            set
            {
                if (this.isha == value) return;

                this.isha = value;
                this.RaisePropertyChanged();
            }
        }

        public string Sunset
        {
            get
            {
                return this.sunset;
            }
            set
            {
                if (this.sunset == value) return;

                this.sunset = value;
                this.RaisePropertyChanged();
            }
        }

        public string Maghrib
        {
            get
            {
                return this.maghrib;
            }
            set
            {
                if (this.maghrib == value) return;

                this.maghrib = value;
                this.RaisePropertyChanged();
            }
        }

        public string Asr
        {
            get
            {
                return this.asr;
            }
            set
            {
                if (this.asr == value) return;

                this.asr = value;
                this.RaisePropertyChanged();
            }
        }

        public string Dhuhr
        {
            get
            {
                return this.dhuhr;
            }
            set
            {
                if (this.dhuhr == value) return;

                this.dhuhr = value;
                this.RaisePropertyChanged();
            }
        }

        public string Sunrise
        {
            get
            {
                return this.sunrise;
            }
            set
            {
                if (this.sunrise == value) return;

                this.sunrise = value;
                this.RaisePropertyChanged();
            }
        }

        public string Date
        {
            get
            {
                return this.date;
            }
            set
            {
                if (this.date == value) return;

                this.date = value;
                this.RaisePropertyChanged();
            }
        }

        public string Fajr
        {
            get
            {
                return this.fajr;
            }
            set
            {
                if (this.fajr == value) return;

                this.fajr = value;
                this.RaisePropertyChanged();
            }
        }

        public double Heading
        {
            get
            {
                return this.heading;
            }
            set
            {
                if (Math.Abs(this.heading - value) < double.Epsilon) return;

                this.heading = value;
                this.RaisePropertyChanged();
            }
        }

        public RelayCommand RefreshCommand { get; }

        private void OnRefreshExecuted()
        {
            this.CalculatePrayTimes();
        }

        private async void CalculatePrayTimes()
        {
            var currentLocation = await this.GetcurrentLocation();
            if (currentLocation != null)
            {
                var calc = new PrayerTimesCalculator(currentLocation.Latitude, currentLocation.Longitude)
                               {
                                   CalculationMethod = CalculationMethods.Egypt,
                                   AsrJurusticMethod = AsrJuristicMethods.Shafii
                               };
                var local = currentLocation.Timestamp.ToLocalTime();
                this.Heading = currentLocation.Heading;
                var times = calc.GetPrayerTimes(local);
                var timesDate = times.Date.Date;
                this.Date = timesDate.ToString("D");
                this.Fajr = timesDate.Add(times.Fajr).ToString("hh:mm tt");
                this.Sunrise = timesDate.Add(times.Sunrise).ToString("hh:mm tt");
                this.Dhuhr = timesDate.Add(times.Dhuhr).ToString("hh:mm tt");
                this.Asr = timesDate.Add(times.Asr).ToString("hh:mm tt");
                this.Maghrib = timesDate.Add(times.Maghrib).ToString("hh:mm tt");
                this.Sunset = timesDate.Add(times.Sunset).ToString("hh:mm tt");
                this.Isha = timesDate.Add(times.Isha).ToString("hh:mm tt");
            }
        }

        private async Task<Position> GetcurrentLocation()
        {
            try
            {
                //await locator.StartListeningAsync(5000, 10);
                var position = await this.locator.GetPositionAsync();
                return position;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to get location, may need to increase timeout: " + ex);
                return null;
            }
        }

        private void LocatorOnPositionChanged(object sender, PositionEventArgs positionEventArgs)
        {
            this.CalculatePrayTimes();
        }
    }
}