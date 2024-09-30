using MongoDB.Bson;
using MongoDB.Driver;
using StarBank.DataAccess;
using StarBank.Models;
using StarBank.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarBank.Services
{
    internal class AccountService
    {
        public static User DepositMoney(User user)
        {
            Console.WriteLine("Enter amount to be deposited");
            double amt = Convert.ToDouble(Console.ReadLine());
            var update = Builders<User>.Update
            .Set("Balance", amt+user.Balance);
            var result = MongoClientProvider.GetClient().GetDatabase("starbank").GetCollection<User>("users").UpdateOne(Builders<User>.Filter.Eq("EmailId", user.EmailId), update);
            return UserRepo.GetUserByEmail(user.EmailId);
        }
        public static User WithdrawMoney(User user) {
            try
            {
                double withdraw;
                while (true)
                {
                    Console.WriteLine("Enter amount to be withdrawn");
                    try
                    {
                        withdraw = Convert.ToDouble(Console.ReadLine());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Please enter a valid amount");
                        continue;
                    }
                    
                    if (withdraw > user.Balance)
                    {
                        Console.WriteLine($"Available balance : {user.Balance}. Please enter a valid amount");
                        continue;
                    }
                    else if (withdraw < 0)
                    {
                        Console.WriteLine("Please enter a valid amount");
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                var update = Builders<User>.Update
                .Set("Balance", user.Balance - withdraw);
                var result = MongoClientProvider.GetClient().GetDatabase("starbank").GetCollection<User>("users").UpdateOne(Builders<User>.Filter.Eq("EmailId", user.EmailId), update);
                return UserRepo.GetUserByEmail(user.EmailId);
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        public static User TransferMoney(User sender)
        {
            string? recepientEmail = "";
            double amt = 0;
            User? recepientUser;
            while (true)
            {
                Console.WriteLine("Enter Recepient's Email ID");
                recepientEmail = Console.ReadLine();
                if(recepientEmail == null || !InputValidators.validateEmail(recepientEmail))
                {
                    Console.WriteLine("Invalid email. Enter a valid one\n");
                }
                else
                {
                    recepientUser = UserRepo.GetUserByEmail(recepientEmail);
                    if (recepientUser == null)
                    {
                        Console.WriteLine("An account with this email Id doesn't exist. Try again\n");
                    }
                    else
                    {
                        break;
                    }
                }
                
            }
            
            while (true)
            {
                try
                {
                    Console.WriteLine("Enter amount to be transferred");
                    amt = Convert.ToDouble(Console.ReadLine());
                    if (amt > sender.Balance)
                    {
                        Console.WriteLine("Insufficient balance. Please try again\n");
                    }
                    else
                    {
                        break;
                    }
                }
                catch (Exception ex) { 
                    Console.WriteLine("Invalid Input. Please try again\n");
                }
            }

            sender.Balance = sender.Balance - amt;
            recepientUser.Balance = recepientUser.Balance + amt;
            
            UserRepo.UpdateUser(recepientUser);
            UserRepo.UpdateUser(sender);
            return UserRepo.GetUserByEmail(sender.EmailId);
        }
    }
}
