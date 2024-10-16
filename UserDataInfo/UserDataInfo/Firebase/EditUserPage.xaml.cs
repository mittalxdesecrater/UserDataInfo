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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditUserPage : ContentPage
    {
        private FirebaseHelper _firebaseHelper;
        private User _user;
        public EditUserPage(User user, FirebaseHelper firebaseHelper)
        {
            InitializeComponent();
            _user = user;
            _firebaseHelper = firebaseHelper;


            nameEntry.Text = _user.Name;
            phoneEntry.Text = _user.PhoneNumber;
            emailEntry.Text = _user.Email;
            datePicker.Date = _user.DateOfBirth;
            ProfileImage.Source = _user.ProfilePicture;
        }

        
        private string _selectedImageFilePath;

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

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                _user.Name = nameEntry.Text;
                _user.PhoneNumber = phoneEntry.Text;
                _user.Email = emailEntry.Text;
                _user.DateOfBirth = datePicker.Date;
                _user.ProfilePicture = _selectedImageFilePath;

                // Ensure user ID is valid
                if (!string.IsNullOrEmpty(_user.Id))
                {
                    await _firebaseHelper.UpdateUserAsync(_user.Id, _user);
                    await DisplayAlert("Success", "User updated successfully!", "OK");
                }
                else
                {
                    await DisplayAlert("Error", "User ID is invalid.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error updating user: {ex.Message}", "OK");
            }

            await Navigation.PopModalAsync();
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            bool confirmDelete = await DisplayAlert("Confirm Delete", "Are you sure you want to delete this user?", "Yes", "No");

            if (confirmDelete)
            {
                try
                {

                    if (!string.IsNullOrEmpty(_user.Id))
                    {
                        await _firebaseHelper.DeleteUserAsync(_user.Id);
                        await DisplayAlert("Success", "User deleted successfully!", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Error", "User ID is invalid.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Error deleting user: {ex.Message}", "OK");
                }

                await Navigation.PopModalAsync();
            }
        }
    }
}