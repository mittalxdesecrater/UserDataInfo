using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDataInfo.Helper;
using Xamarin.Forms;

namespace UserDataInfo
{
    public partial class MainSqlPage : ContentPage
    {
    private DatabaseHelper _databaseHelper;
        public MainSqlPage()
        {
            InitializeComponent();
            SelectedDate = DateTime.Now;
            BindingContext = this;

            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "UserData.db");
            _databaseHelper = new DatabaseHelper(dbPath);

        }
        public DateTime SelectedDate { get; set; }

        
            private async void OnSubmitClicked(object sender, EventArgs e)
            {

                if (string.IsNullOrWhiteSpace(name.Text))
                {
                    await DisplayAlert("Error", "Name is required", "OK");
                    return;
                }

                if (string.IsNullOrWhiteSpace(phone_Number.Text))
                {
                    await DisplayAlert("Error", "Phone number is required", "OK");
                    return;
                }

                if (string.IsNullOrWhiteSpace(email.Text))
                {
                    await DisplayAlert("Error", "Email is required", "OK");
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
                    await _databaseHelper.SaveUserAsync(user);

                    await Navigation.PushAsync(new DisplayUsersPage(_databaseHelper));
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
                }
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

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DisplayUsersPage(_databaseHelper));
        }
    }
}
