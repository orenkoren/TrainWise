using Microsoft.WindowsAzure.Storage.Core.Util;
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

        private Progress<StorageProgress> progress;

        private byte[] bytes;

        public UploadVideo()
        {
            InitializeComponent();
        }

        public UploadVideo(string userName)
        {
            InitializeComponent();
            UserName = userName;
            UsernameLabel.Text = "Welcome, " + userName;
            CurrentVideo = new Video { OfUser = userName };
            progress = new Progress<StorageProgress>();
            progress.ProgressChanged += (s, progressValue) =>
            {            
                progressBar.Progress = progressValue.BytesTransferred / (double)bytes.Length;
            };
        }

        private async void SelectVideo_Clicked(object sender, EventArgs e)
        {


            var file = await CrossFilePicker.Current.PickFile();

            if (file != null)
            {
                FileName.Text = file.FileName;
                CurrentVideo.VideoName = file.FileName;
                CurrentVideo.VideoPath = file.FilePath;
            }
        }

        private void Squat_Clicked(object sender, EventArgs e)
        {
            CurrentVideo.ExerciseType = "Squat";
            SquatButton.BackgroundColor = Color.DarkGray;
            DeadliftButton.BackgroundColor = Color.White;
            BenchpressButton.BackgroundColor = Color.White;
        }

        private void Deadlift_Clicked(object sender, EventArgs e)
        {
            CurrentVideo.ExerciseType = "Deadlift";
            DeadliftButton.BackgroundColor = Color.DarkGray;
            SquatButton.BackgroundColor = Color.White;
            BenchpressButton.BackgroundColor = Color.White;
        }

        private void BenchPress_Clicked(object sender, EventArgs e)
        {
            CurrentVideo.ExerciseType = "BenchPress";
            BenchpressButton.BackgroundColor = Color.DarkGray;
            SquatButton.BackgroundColor = Color.White;
            DeadliftButton.BackgroundColor = Color.White;
        }

        private async void Upload_Clicked(object sender, EventArgs e)
        {
            if (ValidateForm() == true)
            {
                CurrentVideo.Repetitions = int.Parse(Repetitions.Text);
                CurrentVideo.Weight = int.Parse(Weight.Text);
                bytes = File.ReadAllBytes(CurrentVideo.VideoPath);
                progressBar.IsVisible = true;
                ProgressLabel.IsVisible = true;
                MainGrid.IsVisible = false;
                await UploadVideoService.InsertItem(CurrentVideo);
                await StorageService.UploadFileAsync("videocontainer", new MemoryStream(bytes), CurrentVideo.VideoName, progress);
                progressBar.IsVisible = false;
                ProgressLabel.IsVisible = false;

                await DisplayAlert("Success", "File has been uploaded", "OK");
                await Navigation.PushModalAsync(new UploadVideo(UserName), true);
            }
            else
            {
                await DisplayAlert("Failure", "It seems one or more of the information is incorrect, please try again", "OK");
            }
        }


        private bool ValidateForm()
        {
            if (Repetitions != null && Weight != null && CurrentVideo != null && CurrentVideo.ExerciseType != null)
                return true;
            return false;
        }

        private async void SignOut_Clicked(object sender, EventArgs e)
        {
            var loginPage = new LoginPage();
            await Navigation.PushModalAsync(loginPage, true);
        }
    }
}