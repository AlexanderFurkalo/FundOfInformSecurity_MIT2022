using System;
using System.Threading;
using System.Collections.Generic;
using System.Security;
using System.Security.Principal;
using System.Security.Cryptography;

using NLog;

namespace PractZadan_9
{
    class Program
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            Logger.Trace("Start of the programm");
            do
            {
                Logger.Trace("Menu: loop.");
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
                        Logger.Trace("User chose option 1, Register.");
                        Console.WriteLine("Write your account name");
                        string userName = Console.ReadLine();
                        Logger.Debug("The user chose a name during registration,");
                        Console.WriteLine("Write your password");
                        string password = Console.ReadLine();
                        Logger.Debug("The user chose a password during registration");
                        Console.WriteLine("If you are an Admin, click on the number 1. Otherwise, click 2.");
                        string choice = Console.ReadLine();
                        string[] role;
                        if (choice == "1")
                        {
                            Logger.Debug("The user chose his role during registration: Admin.");
                            role = new string[1] { "Admins" };
                        }
                        else
                        {
                            Logger.Debug("The user chose his role during registration: User");
                            role = new string[1] { "Users" };   
                        }
                        try
                        {
                            Logger.Trace("Moment of registration of a new user");
                            Protector.Register(userName, password, role);
                            Logger.Trace("New user was registred, success");
                            Logger.Info("User " + userName + " was registered");
                            Console.ReadKey();
                        }
                        catch (ArgumentException ex)
                        {
                            Logger.Trace("An arror was found: user registration is not completed because the selected name is already taken");
                            Logger.Info("The registration process failed, the new user chose an already existing name.");
                            Logger.Error("The registration process failed, error name: " + $"{ex.GetType()}" );
                            Console.ReadKey();
                        }
                        catch (Exception ex)
                        {
                            Logger.Trace("An error was found");
                            Logger.Info("The registration proccess failed");
                            Console.WriteLine($"{ex.GetType()}: {ex.Message}");
                            Logger.Error("The registration process failed, error name: " + $"{ex.GetType()}");
                            Console.ReadKey();
                        }
                        Logger.Trace("End of option 1, Register, back to menu");
                        break;
                    case "2":
                        Logger.Trace("User chose option 2, Login.");
                        Console.WriteLine("Write your account name");
                        string userNameCheck = Console.ReadLine();
                        Logger.Debug("The user entered his name for verification");
                        Console.WriteLine("Write your password");
                        string passwordCheck = Console.ReadLine();
                        Logger.Debug("The user entered his password for verification");
                        try
                        {
                            Logger.Trace("User entered the data, the authentication process is in progress");
                            Protector.LogIn(userNameCheck, passwordCheck);
                            Logger.Info("User " + userNameCheck + " logged in");
                            Logger.Trace("End of authentication process.");
                            Console.ReadKey();

                        }
                        catch (WrongPassword ex)
                        {
                            Logger.Trace("An error was found: wrong password");
                            Logger.Info("Wrong password was entered during verification");
                            Logger.Warn("Wrong password was entered, authentication process");
                            Logger.Error("The authentication process failed, error name: " + $"{ex.GetType()}");
                            Console.ReadKey();
                        }
                        catch (Exception ex)
                        {
                            Logger.Trace("An error was found");
                            Logger.Info("Error duning verification");
                            Console.WriteLine($"{ex.GetType()}: {ex.Message}");
                            Logger.Error("The authentication process failed, error name: " + $"{ex.GetType()}");
                            Console.ReadKey();
                        }
                        Logger.Trace("End of option 2, Login, back to menu");
                        break;
                    case "3":
                        Logger.Trace("User chose option 3, The Special Feature.");
                        try
                        {
                            Logger.Trace("Process of checking if the current user is an administrator.");
                            Protector.OnlyForAdminsFeature();
                            Logger.Trace("End of proccess of checking");
                            Logger.Info("The current user has successfully used the option \"The Special Feature\" ");
                            Console.ReadKey();
                        }
                        catch (Exception ex)
                        {
                            Logger.Trace("Current user is not an administrator.");
                            Logger.Info("The current user has unsuccessfully used the option \"The Special Feature\"");
                            Logger.Warn("A user without proper rights attempted to access option \"The Special Feature\"");
                            Logger.Error("The process gaining access to an option \"The Special Feature\" failed, error name: " + $"{ex.GetType()}");
                            Console.WriteLine($"{ex.GetType()}: {ex.Message}");
                            Console.ReadKey();
                        }
                        Logger.Trace("End of option 3, The Special Feature");
                        break;
                    case "4":
                        Logger.Trace("User chose option 4, Quit.");
                        Logger.Info("User has decided to exit the application");
                        LogManager.Shutdown();
                        Environment.Exit(0);
                        break;
                    default:
                        Logger.Trace("User entered the wrong command");
                        Logger.Info("The user made an error using the menu.");
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

        public User(string login, string passwordhash, byte[] salt, string[] roles)
        {
            Login = login;
            PasswordHash = passwordhash;
            Salt = Convert.ToBase64String(salt);
            Roles = roles;
        }
    }

    class Protector
    {
        private static Dictionary<string, User> _users = new Dictionary<string, User>();

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