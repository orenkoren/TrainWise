using Plugin.FilePicker;
using System;
using System.IO;
using TrainWise.Services;
using TrainWise.User;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrainWise.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UploadVideo : ContentPage
    {
        private string UserName { get; }

        public UploadVideo()
        {
            InitializeComponent();
        }

        public UploadVideo(string userName)
        {
            InitializeComponent();
            UserName = userName;
        }

        private async void SelectVideo_Clicked(object sender, EventArgs e)
        {
            var file = await CrossFilePicker.Current.PickFile();

            if (file != null)
            {
                byte[] bytes = File.ReadAllBytes(file.FilePath);
                FileName.Text = file.FileName;
                Video vid = new Video
                {
                    VideoName = file.FileName,
                    ByteArray = bytes,
                    OfUser = UserName,
                    ExerciseType = "Deadlift",
                    Repetitions = 3,
                    Weight = 50
                };
                //await UploadVideoService.InsertItem(vid);
                UploadVideoService.InsertGrid(file.FilePath);
            }
        }
    }
}