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
    public partial class DisplayUsersPage : ContentPage
    {
        private FirebaseHelper _firebaseHelper;
        public DisplayUsersPage(FirebaseHelper firebaseHelper)
        {
            InitializeComponent();
            _firebaseHelper = firebaseHelper;
            LoadUsers();
        }
        private async void LoadUsers()
        {
            var users = await _firebaseHelper.GetAllUsers();
            UsersListView.ItemsSource = users;
        }


        private async void OnUserSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var collectionView = (ListView)sender;

            // Get the selected item using SelectedItem
            var selectedUser = collectionView.SelectedItem as User;

            if (selectedUser != null)
            {
                // Navigate to the EditUserPage or perform an action
                await Navigation.PushModalAsync(new EditUserPage(selectedUser, _firebaseHelper));
            }

            // Clear the selection (optional)
            collectionView.SelectedItem = null;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainFirebse());
        }
    }
}