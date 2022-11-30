using System;
using System.Threading;
using System.Collections.Generic;
using System.Security;
using System.Security.Principal;
using System.Security.Cryptography;

namespace PractZadan_8
{
    class Program
    {
        static void Main(string[] args)
        {        
            do
            {
                Console.Clear();
                Console.WriteLine("Greetings! Use this menu to register or log into your account. You have several options, choose one of them: ");
                Console.WriteLine("1. Register;");
                Console.WriteLine("2. Login;");
                Console.WriteLine("3. Special Feature Only For Admins (Do not use it);");
                Console.WriteLine("4. Quit.");
                string yourchoice = Console.ReadLine();
                switch (yourchoice)
                {
                    case "1":
                        Console.WriteLine("Write your account name");
                        string userName = Console.ReadLine();
                        Console.WriteLine("Write your password");
                        string password = Console.ReadLine();
                        Console.WriteLine("If you are an Admin, click on the number 1. Otherwise, click 2.");
                        string choice = Console.ReadLine();
                        string[] role;
                        if (choice == "1")
                        {
                            role = new string[1] {"Admins"};
                        }
                        else
                        {
                            role = new string[1] {"Users"};
                        }
                        try
                        {
                            Protector.Register(userName, password, role);
                            Console.ReadKey();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"{ex.GetType()}: {ex.Message}");
                            Console.ReadKey();
                        }
                        break;
                    case "2":
                        Console.WriteLine("Write your account name");
                        string userNameCheck = Console.ReadLine();
                        Console.WriteLine("Write your password");
                        string passwordCheck = Console.ReadLine();
                        try
                        {
                            Protector.LogIn(userNameCheck, passwordCheck);
                            Console.ReadKey();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"{ex.GetType()}: {ex.Message}");
                            Console.ReadKey();
                        }
                        break;
                    case "3":
                        try
                        {
                            Protector.OnlyForAdminsFeature();
                            Console.ReadKey();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"{ex.GetType()}: {ex.Message}");
                            Console.ReadKey();
                        }
                        break;
                    case "4":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Wrong command. Try again.");
                        Console.ReadKey();
                        break;
                }
            }
            while (true);
        }
    }

    class User
    {
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public string[] Roles { get; set; }

        public User(string login,string passwordhash, byte[] salt, string[] roles)
        {
            Login = login;
            PasswordHash = passwordhash;
            Salt = Convert.ToBase64String(salt);
            Roles = roles;
        }
    }

    class Protector
    {
        private static Dictionary <string, User> _users = new Dictionary<string, User>();

        public static void Register(string userName, string password, string[] roles = null)
        {
            if (_users.ContainsKey(userName))
            {
                throw new ArgumentException($"An User with name \"{userName}\" already exists.");
            }
            byte[] Salt = PBKDF2.GenerateSalt();
            string HashPass = Convert.ToBase64String(PBKDF2.HashPassword(System.Text.Encoding.UTF8.GetBytes(password), Salt, 230000, "SHA256"));
            User Check = new User(userName, HashPass, Salt, roles);
            _users.Add(userName, Check);
            Console.WriteLine("Welcome!!!");

        }

        public static bool CheckPassword(string userName, string password)
        {
            if (_users.ContainsKey(userName))
            {
                User Check = _users[userName];
                byte[] Salt = Convert.FromBase64String(Check.Salt);
                string CheckHashPass = Convert.ToBase64String(PBKDF2.HashPassword(System.Text.Encoding.UTF8.GetBytes(password), Salt, 230000, "SHA256"));
                if (CheckHashPass == Check.PasswordHash)
                {
                    return true;
                }
                else
                {
                    throw new WrongPassword("Wrong login or password - try again.");
                }
            }
            else
            {
                throw new KeyNotFoundException("User with name " + userName + " was not found.");
            }
        }

        public static void LogIn(string userName, string password)
        {
            if (CheckPassword(userName, password))
            { 
                var identity = new GenericIdentity(userName, "OIBAuth");
                var principal = new GenericPrincipal(identity, _users[userName].Roles);
                Thread.CurrentPrincipal = principal;
                Console.WriteLine("Welcome back!!!");
            }
        }

        public static void OnlyForAdminsFeature()
        {
            if (Thread.CurrentPrincipal == null)
            {
                throw new SecurityException("Thread.CurrentPrincipal cannot be null.");
            }
            if (!Thread.CurrentPrincipal.IsInRole("Admins"))
            {
                throw new SecurityException("User must be a member of Admins to access this feature.");
            }
            Console.WriteLine("You have access to this secure feature.");
        }
    }

    public class PBKDF2
    {
        public static byte[] GenerateSalt()
        {
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[32];
                randomNumberGenerator.GetBytes(randomNumber);
                return randomNumber;
            }
        }
        public static byte[] HashPassword(byte[] toBeHashed, byte[] salt, int numberOfRounds, string HashAlgor)
        {
            var HashAl = new HashAlgorithmName(HashAlgor);
            using (var rfc2898 = new Rfc2898DeriveBytes(toBeHashed, salt, numberOfRounds, HashAl))
            {
                return rfc2898.GetBytes(20);
            }      
        }
    }
    public class WrongPassword : Exception
    {
        public WrongPassword(string message) : base(message) { }
    }
}
