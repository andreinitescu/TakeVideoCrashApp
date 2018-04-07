using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TakeVideoCrashApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            Btn.Clicked += Btn_Clicked;
        }

        private async void Btn_Clicked(object sender, EventArgs e)
        {
            if (!await EnsurePermissions())
            {
                await DisplayAlert("Permissions Denied", "Unable to take photos.", "OK");
                return;
            }

            MakeNewVideo();
        }

        async Task<bool> EnsurePermissions()
        {
            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                cameraStatus = results[Permission.Camera];
                storageStatus = results[Permission.Storage];
            }

            return cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted;
        }

        async void MakeNewVideo()
        {
            IMedia media = CrossMedia.Current;

            await media.Initialize();
            if (media.IsTakeVideoSupported)
            {
                MediaFile result = await media.TakePhotoAsync(new StoreVideoOptions()
                //MediaFile result = await media.TakeVideoAsync(new StoreVideoOptions()
                {
                    Quality = VideoQuality.Medium,
                    DefaultCamera = CameraDevice.Rear,
                    SaveToAlbum = true
                });

                if (result != null)
                {
                    await DisplayAlert("Perfect", "OK", "Cancel");
                }
                else
                {
                    await DisplayAlert("Not good", "OK", "Cancel");
                }
            }
        }
    }
}
