using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDataInfo.Helper;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UserDataInfo
{
    public partial class DisplayUsersPage : ContentPage
    {
        private DatabaseHelper _databaseHelper;
        public DisplayUsersPage(DatabaseHelper databaseHelper)
        {
            InitializeComponent();
            _databaseHelper = databaseHelper;

            LoadUsers();
        }

       

        private async void LoadUsers()
        {
            var users = await _databaseHelper.GetUsersAsync();


            UsersCollectionView.ItemsSource = users;
        }

        private async void OnUserSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var collectionView = (ListView)sender;


            var selectedUser = collectionView.SelectedItem as User;

            if (selectedUser != null)
            {

                await Navigation.PushModalAsync(new EditUserPage(selectedUser, _databaseHelper));
            }


            collectionView.SelectedItem = null;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }
    }
}