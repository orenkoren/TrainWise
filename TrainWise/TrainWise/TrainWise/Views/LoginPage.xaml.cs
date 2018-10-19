using System;
using System.Threading;
using System.Threading.Tasks;
using TrainWise.Services;
using TrainWise.User;
using TrainWise.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
using MongoDB.Bson;

namespace TrainWise.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        LoginViewModel ViewModel { get; }

        public LoginPage()
        {
            InitializeComponent();
            ViewModel = new LoginViewModel();
            BindingContext = ViewModel;

        }

        private async void Login_Clicked(object sender, EventArgs e)
        {
            //new Thread(() =>
            //   {
            //       Credentials cred = new Credentials
            //       {
            //           Username = ViewModel.Username,
            //           Password = ViewModel.Password
            //       };
            //       Database.CreateTask(cred);
            //   })
            //{ IsBackground = true }.Start();

            var allItems = await MongoService.GetAllItems();

            if (allItems.Any(c => c.Username == ViewModel.Username) == false)
            {
                Credentials cred = new Credentials
                {
                    //Id = ObjectId.GenerateNewId().ToString(),
                    Username = ViewModel.Username,
                    Password = ViewModel.Password
                };
                await MongoService.InsertItem(cred);
            }
            var videoPage = new NavigationPage(new UploadVideo());
            await Navigation.PushModalAsync(videoPage, true);
        }
    }
}