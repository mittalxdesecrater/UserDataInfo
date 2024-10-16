using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDataInfo.Firebase;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UserDataInfo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnSqliteClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainSqlPage());
        }
        private async void OnFirebaseClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainFirebse());
        }
    }
}