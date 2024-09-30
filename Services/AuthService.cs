using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using StarBank.DataAccess;
using StarBank.Models;
using StarBank.Utilities;

namespace StarBank.Services
{
    internal class AuthService
    {
        public static GenericResponse Login()
        {
            string? email;
            string? password;
            while(true)
            {
                Console.WriteLine("Enter EmailId");
                email = Console.ReadLine();
                if (!InputValidators.validateEmail(email))
                {
                    Console.WriteLine("Invalid Email! Please enter again.");
                }
                else {
                    break;
                }
            }

            while(true)
            {
                Console.WriteLine("Enter Password");
                password = Console.ReadLine();
                if (!InputValidators.validatePassword(password))
                {
                    Console.WriteLine("Invalid Password! Min length: 8 characters");
                }
                else
                {
                    break;
                }
            }

            List<User> users = MongoClientProvider.GetClient().GetDatabase("starbank").GetCollection<User>("users").Find(Builders<User>.Filter.Eq("EmailId", email)).ToList();
            if (users.Count > 0) {
                if (users[0].Password == password)
                {
                    return new GenericResponse()
                    {
                        isSuccess = true,
                        errorMessage = "",
                        data = users[0]
                    };
                }
            }
            return new GenericResponse()
            {
                isSuccess = false,
                errorMessage = "Something went wrong",
                data = null
            };
        }

        public static GenericResponse Register()
        {
            try
            {
                User newUser = new User();
                string? temp = "";
                while (true)
                {
                    Console.WriteLine("Enter First Name");
                    temp = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(temp))
                    {
                        Console.WriteLine("First Name is mandatory! Please enter again.");
                    }
                    else
                    {
                        newUser.FirstName = temp;
                        break;
                    }
                }

                Console.WriteLine("Enter Last Name");
                newUser.LastName = Console.ReadLine();

                Console.WriteLine("Enter Phone Number");
                newUser.PhoneNumber = Console.ReadLine();

                while (true)
                {
                    Console.WriteLine("Enter EmailId");
                    newUser.EmailId = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newUser.EmailId) || !InputValidators.validateEmail(newUser.EmailId))
                    {
                        Console.WriteLine("Invalid Email! Please enter again.");
                    }

                    else if (UserRepo.GetUserByEmail(newUser.EmailId) != null)
                    {
                        Console.WriteLine("An account with this Email already exists. Please enter another one.");
                    }
                    else
                    {
                        break;
                    }
                }

                while (true)
                {
                    Console.WriteLine("Enter Password");
                    newUser.Password = Console.ReadLine();
                    if (!InputValidators.validatePassword(newUser.Password))
                    {
                        Console.WriteLine("Invalid Password! Min length: 8 characters");
                    }
                    else
                    {
                        break;
                    }
                }

                var userCollection = MongoClientProvider.GetClient().GetDatabase("starbank").GetCollection<User>("users");
                userCollection.InsertOne(newUser);

                return new GenericResponse()
                {
                    isSuccess = true,
                    errorMessage = "",
                    data = newUser
                };
            }
            catch (Exception ex) {
                return new GenericResponse()
                {
                    isSuccess = false,
                    errorMessage = ex.Message,
                    data = null
                };
            }
            
        }
    }
}
