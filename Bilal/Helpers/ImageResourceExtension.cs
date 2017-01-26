namespace Bilal.Helpers
{
    using System;

    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [ContentProperty("Source")]
    public class ImageResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (this.Source == null)
            {
                return null;
            }
            // Do your translation lookup here, using whatever method you require
            var imageSource = ImageSource.FromResource(this.Source);

            return imageSource;
        }
    }
}