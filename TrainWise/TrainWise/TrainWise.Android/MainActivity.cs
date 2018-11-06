
using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;

namespace TrainWise.Droid
{
    [Activity(Label = "TrainWise", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestContactsPermissions();
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        private void RequestContactsPermissions()
        {
            if (! ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.ReadExternalStorage)
                || ! ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.WriteExternalStorage))
            {
                string[] PERMISSIONS_CONTACT = {
                Manifest.Permission.ReadExternalStorage,
                Manifest.Permission.WriteExternalStorage
                };
                // Contact permissions have not been granted yet. Request them directly.
                ActivityCompat.RequestPermissions(this, PERMISSIONS_CONTACT, 1);
            }
        }
    }


}