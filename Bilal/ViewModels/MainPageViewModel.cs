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
    using GalaSoft.MvvmLight;
    /// <summary>
    ///     Class MainPageViewModel.
    /// </summary>
    public class MainPageViewModel : ViewModelBase
    {
        private string mainText;

        public string MainText
        {
            get
            {
                return this.mainText;
            }
            set
            {
                if (this.mainText == value) return;

                this.mainText = value;
                this.RaisePropertyChanged("MainText");
            }
        }

        public MainPageViewModel()
        {
            this.MainText = "Hello Xamarin";
        }
    }
}