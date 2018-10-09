using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrainWise.MarkupExtensions
{
    [ContentProperty("ResourceId")]
    class EmbeddedImage : IMarkupExtension
    {
        public string ResourceId { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrWhiteSpace(ResourceId))
                return null;
            return ImageSource.FromResource(ResourceId);
        }
    }
}
