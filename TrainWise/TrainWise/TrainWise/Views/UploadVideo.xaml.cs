using Plugin.FilePicker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrainWise.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UploadVideo : ContentPage
	{
		public UploadVideo ()
		{
			InitializeComponent ();
		}

        private async void SelectVideo_Clicked(object sender, EventArgs e)
        {
            var file = await CrossFilePicker.Current.PickFile();

            if (file != null)
                FileName.Text = file.FileName;
        }
    }
}