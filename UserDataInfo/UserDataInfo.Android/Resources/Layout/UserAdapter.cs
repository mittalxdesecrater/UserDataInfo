
    // UserAdapter.cs
    using Android.Content;
    using Android.Views;
    using Android.Widget;
    using AndroidX.RecyclerView.Widget;
    using global::UserDataInfo.Helper;
    using System.Collections.Generic;

    namespace UserDataInfo.Droid.Resources.Layout
    {
        public class UserAdapter : RecyclerView.Adapter
        {
            private readonly List<User> _users;
            private readonly Context _context;

            public UserAdapter(List<User> users, Context context)
            {
                _users = users;
                _context = context;
            }

            public override int ItemCount => _users.Count;

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                var userViewHolder = holder as UserViewHolder;
                var user = _users[position];
                userViewHolder.BindData(user);
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.user_item, parent, false);
                return new UserViewHolder(view);
            }
        }

        public class UserViewHolder : RecyclerView.ViewHolder
        {
            private readonly TextView _nameTextView;
            private readonly TextView _phoneTextView;
            private readonly ImageView _profileImageView;

            public UserViewHolder(View itemView) : base(itemView)
            {
                _nameTextView = itemView.FindViewById<TextView>(Resource.Id.userName);
                _phoneTextView = itemView.FindViewById<TextView>(Resource.Id.userPhone);
                _profileImageView = itemView.FindViewById<ImageView>(Resource.Id.profileImage);
            }

            public void BindData(User user)
            {
                _nameTextView.Text = user.Name;
                _phoneTextView.Text = user.PhoneNumber;
                _profileImageView.SetImageURI(Android.Net.Uri.Parse(user.ProfilePicturePath));
            }
        }
    }

