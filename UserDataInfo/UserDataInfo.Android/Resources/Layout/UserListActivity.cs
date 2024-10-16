using Android.App;
using System;
using Android.OS;
using AndroidX.RecyclerView.Widget;
using System.Collections.Generic;
using UserDataInfo.Helper;

namespace UserDataInfo.Droid.Resources.Layout
{
    [Activity(Label = "User List")]
    public class UserListActivity : Activity
    {
        private RecyclerView recyclerView;
        private UserAdapter adapter;
        private List<User> userList;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_user_list);

            recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            recyclerView.SetLayoutManager(new LinearLayoutManager(this));

            var databaseHelper = new DatabaseHelper();
            userList = await databaseHelper.GetUsersAsync();

            adapter = new UserAdapter(userList, this);
            recyclerView.SetAdapter(adapter);
        }
    }
}