﻿// ***********************************************************************
// Assembly         : Bilal.UWP
// Author           : Mahmoud Hasheesh
// Created          : 01-16-2017
//
// Last Modified By : Mahmoud Hasheesh
// Last Modified On : 01-16-2017
// ***********************************************************************
// <copyright file="MainPage.xaml.cs" company="Mahmoud Hasheesh">
//     Licensed under the MIT License. See License.txt in the project root for license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Bilal.UWP
{
    /// <summary>
    ///     Class MainPage. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="Xamarin.Forms.Platform.UWP.WindowsPage" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class MainPage
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MainPage" /> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();

            this.LoadApplication(new Bilal.App());
        }
    }
}