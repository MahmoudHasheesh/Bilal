// ***********************************************************************
// Assembly         : Bilal
// Author           : Mahmoud Hasheesh
// Created          : 01-16-2017
//
// Last Modified By : Mahmoud Hasheesh
// Last Modified On : 01-16-2017
// ***********************************************************************
// <copyright file="QiblaViewModel.cs" company="Mahmoud Hasheesh">
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

    using Plugin.Compass;
    using Plugin.Geolocator;
    using Plugin.Geolocator.Abstractions;

    /// <summary>
    ///     Class MainPageViewModel.
    /// </summary>
    public class QiblaViewModel : ViewModelBase
    {
        private double compassHeading;

        public QiblaViewModel()
        {
            CrossCompass.Current.CompassChanged += (s, e) => { this.CompassHeading = Math.Round(e.Heading, 0); };
            CrossCompass.Current.Start();

            this.locator = CrossGeolocator.Current;
            this.locator.DesiredAccuracy = 50;
            this.locator.PositionChanged += this.LocatorOnPositionChanged;
            this.BearingToKaaba();
        }

        private IGeolocator locator;
        public double Rotation => -1 * this.compassHeading;

        public double CompassHeading
        {
            get
            {
                return this.compassHeading;
            }
            set
            {
                if (Math.Abs(this.compassHeading - value) < double.Epsilon) return;

                this.compassHeading = value;
                this.RaisePropertyChanged();
                this.RaisePropertyChanged("Rotation");
            }
        }

        private double bearing;

        public double Bearing
        {
            get
            {
                return this.bearing;
            }
            set
            {
                if (Math.Abs(this.bearing - value) < double.Epsilon) return;

                this.bearing = value;
                this.RaisePropertyChanged();
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
            this.BearingToKaaba();
        }

        private async void BearingToKaaba()
        {
            var currentLocation = await this.GetcurrentLocation();
            if (currentLocation != null)
            {
                this.Bearing = GeoHelper.BearingTo(currentLocation.Latitude, currentLocation.Longitude, 21.422487, 39.826206);
            }
        }
    }
}