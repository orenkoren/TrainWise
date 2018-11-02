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

        private Video CurrentVideo { get; set; }

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
                
                FileName.Text = file.FileName;
                CurrentVideo = new Video
                {
                    VideoName = file.FileName,
                    VideoPath = file.FilePath,
                    OfUser = UserName,
                };
                
            }
        }

        private void Squat_Clicked(object sender, EventArgs e)
            => CurrentVideo.ExerciseType = "Squat";

        private void Deadlift_Clicked(object sender, EventArgs e)
            => CurrentVideo.ExerciseType = "Deadlift";

        private void BenchPress_Clicked(object sender, EventArgs e)
            => CurrentVideo.ExerciseType = "BenchPress";

        private async void Upload_Clicked(object sender, EventArgs e)
        {
            CurrentVideo.Repetitions = int.Parse(Repetitions.Text);
            CurrentVideo.Weight = int.Parse(Weight.Text);

            byte[] bytes = File.ReadAllBytes(CurrentVideo.VideoPath);
            await UploadVideoService.InsertItem(CurrentVideo);
            await StorageService.UploadFileAsync("videocontainer", new MemoryStream(bytes), CurrentVideo.VideoName);
        }
    }
}