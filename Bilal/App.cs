// ***********************************************************************
// Assembly         : Bilal
// Author           : Mahmoud Hasheesh
// Created          : 01-16-2017
//
// Last Modified By : Mahmoud Hasheesh
// Last Modified On : 01-16-2017
// ***********************************************************************
// <copyright file="App.cs" company="Mahmoud Hasheesh">
//     Licensed under the MIT License. See License.txt in the project root for license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Bilal
{
    using Xamarin.Forms;

    /// <summary>
    ///     Class App.
    /// </summary>
    /// <seealso cref="Xamarin.Forms.Application" />
    public class App : Application
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="App" /> class.
        /// </summary>
        public App()
        {
            // The root page of your application
            var content = new MainPage();
            this.MainPage = new NavigationPage(content);
        }

        /// <summary>
        ///     Application developers override this method to perform actions when the application starts.
        /// </summary>
        /// <remarks>To be added.</remarks>
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        /// <summary>
        ///     Application developers override this method to perform actions when the application enters the sleeping state.
        /// </summary>
        /// <remarks>To be added.</remarks>
        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        /// <summary>
        ///     Application developers override this method to perform actions when the application resumes from a sleeping state.
        /// </summary>
        /// <remarks>To be added.</remarks>
        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}