using Plugin.Media.Abstractions;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDataInfo.Helper;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UserDataInfo.Firebase
{
	public partial class MainFirebse : ContentPage
	{
        private readonly FirebaseHelper _firebaseHelper;
        private string _selectedImageFilePath;
        public MainFirebse ()
		{
			InitializeComponent ();
            _firebaseHelper = new FirebaseHelper();
        }
        private async void OnSubmitClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(name.Text) || string.IsNullOrWhiteSpace(phone_Number.Text) || string.IsNullOrWhiteSpace(email.Text))
            {
                await DisplayAlert("Error", "All fields are required", "OK");
                return;
            }

            var user = new User
            {
                Name = name.Text,
                PhoneNumber = phone_Number.Text,
                Email = email.Text,
                DateOfBirth = datePicker.Date,
                ProfilePicture = _selectedImageFilePath
            };

            try
            {
                await _firebaseHelper.SaveUser(user);
                await DisplayAlert("Success", "User data saved successfully", "OK");
                await Navigation.PushAsync(new DisplayUsersPage(_firebaseHelper));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }


        private async void OnSelectPictureClicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Error", "Photo picking not supported on this device", "OK");
                return;
            }

            var mediaOptions = new PickMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
            };

            var selectedImageFile = await CrossMedia.Current.PickPhotoAsync(mediaOptions);

            if (selectedImageFile == null)
            {
                await DisplayAlert("Error", "Could not get the image, please try again.", "OK");
                return;
            }

            ProfileImage.Source = ImageSource.FromStream(() => selectedImageFile.GetStream());
            _selectedImageFilePath = selectedImageFile.Path;
        }


        private async void OnShowDataClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DisplayUsersPage(_firebaseHelper));
        }

        
    }
}