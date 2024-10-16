using Plugin.Media.Abstractions;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDataInfo.Helper;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace UserDataInfo
{
    public partial class EditUserPage : ContentPage
    {
        private DatabaseHelper _databaseHelper;
        private User _user;
        public EditUserPage(User user, DatabaseHelper databaseHelper)
        {
            InitializeComponent();
            _user = user;
            _databaseHelper = databaseHelper;

            // Populate fields with user data
            nameEntry.Text = _user.Name;
            phoneEntry.Text = _user.PhoneNumber;
            emailEntry.Text = _user.Email;
            datePicker.Date = _user.DateOfBirth;
            ProfileImage.Source = _user.ProfilePicture;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            _user.Name = nameEntry.Text;
            _user.PhoneNumber = phoneEntry.Text;
            _user.Email = emailEntry.Text;
            _user.DateOfBirth = datePicker.Date;
            _user.ProfilePicture = _selectedImageFilePath;

            await _databaseHelper.UpdateUserAsync(_user);
            await Navigation.PopModalAsync();
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            await _databaseHelper.DeleteUserAsync(_user);
            await Navigation.PopModalAsync();
        }
        private string _selectedImageFilePath;

        private async void OnSelectPictureClicked(object sender, EventArgs e)
        {
            // Initialize the plugin (necessary for Android)
            await CrossMedia.Current.Initialize();

            // Check if picking a photo is supported
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Error", "Photo picking not supported on this device", "OK");
                return;
            }

            // Pick the photo from the gallery
            var mediaOptions = new PickMediaOptions
            {
                PhotoSize = PhotoSize.Medium, // You can customize the size
            };

            var selectedImageFile = await CrossMedia.Current.PickPhotoAsync(mediaOptions);

            if (selectedImageFile == null)
            {
                await DisplayAlert("Error", "Could not get the image, please try again.", "OK");
                return;
            }

            // Set the selected image to the Image control
            ProfileImage.Source = ImageSource.FromStream(() => selectedImageFile.GetStream());
            _selectedImageFilePath = selectedImageFile.Path;
        }
    }
}