using System;
using UserDataInfo.Firebase;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using System.Linq;
using Firebase.Database.Query;

namespace UserDataInfo.Helper
{
    public class FirebaseHelper
    {
        private readonly FirebaseClient _firebaseClient;

        public FirebaseHelper() 
        {
            _firebaseClient = new FirebaseClient("https://userdatainfo-8c48c-default-rtdb.firebaseio.com/");
        }

        public async Task SaveUser(User user)
        {
            await _firebaseClient
              .Child("Users")
              .PostAsync(user);
        }

        public async Task<List<User>> GetAllUsers()
        {
            var users = await _firebaseClient
                 .Child("Users")
                 .OnceAsync<User>();

            return users.Select(u => new User
            {
                Id = u.Key, 
                Name = u.Object.Name,
                PhoneNumber = u.Object.PhoneNumber,
                Email = u.Object.Email,
                DateOfBirth = u.Object.DateOfBirth,
                ProfilePicture = u.Object.ProfilePicture
            }).ToList();
        }

        public async Task<User> GetUser(string name)
        {
            var allUsers = await GetAllUsers();
            return allUsers.FirstOrDefault(a => a.Name == name);
        }

        public async Task UpdateUserAsync(string userId, User updatedUser)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                await _firebaseClient
                    .Child("Users")
                    .Child(userId) // Use Firebase ID to update
                    .PutAsync(updatedUser);
            }
            else
            {
                throw new Exception("User ID is null or empty.");
            }
        }
        
        public async Task DeleteUserAsync(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                await _firebaseClient
                    .Child("Users")
                    .Child(userId)
                    .DeleteAsync();
            }
            else
            {
                throw new Exception("User ID is null or empty.");
            }
        }

       
    }
}
