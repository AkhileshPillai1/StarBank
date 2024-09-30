using MongoDB.Driver;
using StarBank.Models;
using StarBank.Services;

Console.WriteLine("Welcome to StarBank!");
User loggedInUser = new User();

while(true)
{
    string? input = "";
    if(loggedInUser != null && loggedInUser.EmailId == "")
    {
        Console.WriteLine("\nPress L to login to an exisitng account");
        Console.WriteLine("Press R to create a new account");
    }
    else
    {
        Console.WriteLine("\nPress B to view Balance");
        Console.WriteLine("Press D to deposit money");
        Console.WriteLine("Press W to withdraw money");
        Console.WriteLine("Press T to transfer money");
        Console.WriteLine("Press G to Log out");
    }
    Console.WriteLine("Press X to Exit");
    input = Console.ReadLine();
    if (input == null || input.Length != 1)
    {
        Console.WriteLine("Invalid Input. Please try again\n");
        continue;
    }
    else
    {
        HandleActions(input.ToUpper()[0]);
    }
}

void HandleActions(char input)
{
    switch (input)
    {
        case 'L':
            GenericResponse userRes = AuthService.Login();//P323B
            if (userRes.isSuccess && userRes.data != null)
            {
                loggedInUser = (User)userRes.data;
                Console.WriteLine($"\nWelcome back, {loggedInUser?.FirstName}\n");
            }
            break;
        case 'R':
            GenericResponse registerRes = AuthService.Register();
            if(registerRes.isSuccess && registerRes.data != null)
            {
                loggedInUser = (User)registerRes.data;
                Console.WriteLine($"\nWelcome to Star Bank, {loggedInUser?.FirstName}\n");
            }
            break;
        case 'B':
            Console.WriteLine($"Balance is {loggedInUser?.Balance}\n");
            break;
        case 'D':
            loggedInUser = AccountService.DepositMoney(loggedInUser);
            Console.WriteLine("Deposited successfully!");
            break;
        case 'W':
            loggedInUser = AccountService.WithdrawMoney(loggedInUser);
            Console.WriteLine("Withdrawn successfully!");
            break;
        case 'T':
            loggedInUser = AccountService.TransferMoney(loggedInUser);
            Console.WriteLine("Transfered successfully!");
            break;
        case 'G':
            loggedInUser = new User();
            Console.WriteLine("\nLogged out successfully, Thank you for using StarBank!");
            break;
        case 'X':
            Console.WriteLine("Thank you for using StarBank!");
            Environment.Exit(0);
            break;
        default :
            Console.WriteLine("Invalid Input. Please try again\n");
            break;
    }
}


public class MongoClientProvider
{
    private static MongoClient? _client;

    public static MongoClient GetClient()
    {
        if (_client == null)
        {
            _client = new MongoClient("mongodb+srv://fitstore-admin:FitStore1234%40%40@fitstore.r65lcys.mongodb.net/?retryWrites=true&w=majority&appName=FitStore");
        }
        return _client;
    }
}