using MongoDB.Driver;
using StarBank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarBank.DataAccess
{
    internal class UserRepo
    {
        public static User? GetUserByEmail(string email)
        {
            List<User> users = MongoClientProvider.GetClient().GetDatabase("starbank").GetCollection<User>("users").Find(Builders<User>.Filter.Eq("EmailId", email)).ToList();
            return users.FirstOrDefault();
        }
        public static GenericResponse UpdateUser(User user)
        {
            var collection = MongoClientProvider.GetClient().GetDatabase("starbank").GetCollection<User>("users");

            var filter = Builders<User>.Filter.Eq("EmailId", user.EmailId);

            var result = collection.ReplaceOne(filter, user);

            return new GenericResponse()
            {
                isSuccess = true,
                errorMessage = "",
                data = result
            };
        }
    }
}
