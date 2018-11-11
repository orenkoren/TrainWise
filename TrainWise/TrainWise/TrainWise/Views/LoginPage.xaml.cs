using System;
using System.Linq;
using System.Threading.Tasks;
using TrainWise.Services;
using TrainWise.User;
using TrainWise.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrainWise.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        LoginViewModel ViewModel { get; }

        public System.Collections.Generic.IDictionary<string, object> Properties { get; } = Application.Current.Properties;
        public LoginPage()
        {
            InitializeComponent();
            ViewModel = new LoginViewModel();
            BindingContext = ViewModel;
            if (Properties.ContainsKey("username") && Properties.ContainsKey("password"))
            {
                ViewModel.Username = Properties["username"].ToString();
                ViewModel.Password = Properties["password"].ToString();
                UsernameEntry.Text = ViewModel.Username;
                PasswordEntry.Text = ViewModel.Password;
            }
        }

        private async void Login_Clicked(object sender, EventArgs e)
        {
            Indicator.IsRunning = true;
            MainGrid.IsVisible = false;
            var validCredentials = ValidateCredentials();
            if (validCredentials == false)
            {
                await DisplayAlert("Failure", "It seems one or more of the information is incorrect, please try again", "OK");
                Indicator.IsRunning = false;
                MainGrid.IsVisible = true;
                return;
            }
            var userExists = await UserExists();
            if (userExists == null)
            {
                Credentials cred = new Credentials
                {
                    Username = ViewModel.Username,
                    Password = ViewModel.Password
                };
                await MongoService.InsertItem(cred);
            }
            else if (userExists.Password != ViewModel.Password)
            {
                await DisplayAlert("Failure", "Incorrect password for given username", "OK");
                Indicator.IsRunning = false;
                MainGrid.IsVisible = true;
                return;
            }
            if (!Properties.ContainsKey("username") && !Properties.ContainsKey("password"))
            {
                Properties.Add("username", ViewModel.Username);
                Properties.Add("password", ViewModel.Password);
            }
            var videoPage = new NavigationPage(new UploadVideo(ViewModel.Username));
            await Navigation.PushModalAsync(videoPage, true);
        }

        private bool ValidateCredentials()
        {
            if (ViewModel.Username != string.Empty && ViewModel.Password != string.Empty)
                return true;
            return false;
        }

        private async Task<Credentials> UserExists()
        {
            var allItems = await MongoService.GetAllItems();
             return allItems.FirstOrDefault(c => c.Username == ViewModel.Username);
        }
    }
}