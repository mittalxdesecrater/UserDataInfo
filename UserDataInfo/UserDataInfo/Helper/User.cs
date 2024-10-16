using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserDataInfo.Helper
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public string Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ProfilePicture { get; set; }
    }
}
