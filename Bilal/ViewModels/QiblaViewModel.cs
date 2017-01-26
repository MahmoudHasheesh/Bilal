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

    using GalaSoft.MvvmLight;

    using Plugin.Compass;

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
        }

        public double Rotation => -1 * this.compassHeading;

        public double CompassHeading
        {
            get
            {
                return this.compassHeading;
            }
            set
            {
                if (Math.Abs(this.compassHeading - value) < Double.Epsilon) return;

                this.compassHeading = value;
                this.RaisePropertyChanged();
                this.RaisePropertyChanged("Rotation");
            }
        }
    }
}