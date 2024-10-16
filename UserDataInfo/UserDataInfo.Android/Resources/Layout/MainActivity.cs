using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.Media.Abstractions;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserDataInfo.Helper;

namespace UserDataInfo.Droid.Resources.Layout
{
    [Activity(Label = "User Data Info", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private EditText name;
        private EditText phone_Number;
        private EditText email;
        private DatePicker datePicker;
        private ImageView profileImage;
        private string profileImagePath;
        private DatabaseHelper databaseHelper;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            name = FindViewById<EditText>(Resource.Id.name);
            phone_Number = FindViewById<EditText>(Resource.Id.phone_Number);
            email = FindViewById<EditText>(Resource.Id.email);
            datePicker = FindViewById<DatePicker>(Resource.Id.datePicker);
            profileImage = FindViewById<ImageView>(Resource.Id.profileImage);
            var uploadButton = FindViewById<Button>(Resource.Id.uploadButton);
            var submitButton = FindViewById<Button>(Resource.Id.submitButton);

            databaseHelper = new DatabaseHelper();

            uploadButton.Click += OnSelectPictureClicked;
            submitButton.Click += OnSubmitClicked;
        }

        private async void OnSelectPictureClicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                Toast.MakeText(this, "Photo picking not supported on this device", ToastLength.Short).Show();
                return;
            }

            var mediaOptions = new PickMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
            };

            var selectedImageFile = await CrossMedia.Current.PickPhotoAsync(mediaOptions);

            if (selectedImageFile == null)
            {
                Toast.MakeText(this, "Could not get the image, please try again.", ToastLength.Short).Show();
                return;
            }

            profileImagePath = selectedImageFile.Path; // Save the image path
            profileImage.SetImageURI(Android.Net.Uri.Parse(profileImagePath)); // Set the image
        }

        private async void OnSubmitClicked(object sender, EventArgs e)
        {
            var user = new User
            {
                Name = name.Text,
                PhoneNumber = phone_Number.Text,
                Email = email.Text,
                DateOfBirth = datePicker.DateTime,
                ProfilePicturePath = profileImagePath
            };

            await databaseHelper.SaveUserAsync(user);
            Toast.MakeText(this, "User details saved successfully.", ToastLength.Short).Show();

            // Navigate to UserListActivity
            var intent = new Intent(this, typeof(UserListActivity));
            StartActivity(intent);
        }
    }
}