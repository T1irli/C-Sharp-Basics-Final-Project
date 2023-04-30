using BusinessLogicLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleUI;
using Microsoft.SqlServer.Server;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Globalization;
using BusinessLogicLayer;

namespace QuizGameProject
{
    internal class Application
    {
        private static User currectUsrer;

        public static void Run()
        {
            SetUser();

            while (true)
            {
                int option = ConsoleUI.View.Menu(new string[]
                {
                    "Start a new quiz",
                    "Statistics",
                    "Settings",
                    "Create quiz",
                    "Change quiz",
                    "Exit"
                }, "QUIZ GAME");

                Console.Clear();
                switch (option)
                {
                    case 0:
                        var stats = GameView.Start();
                        if (stats != null)
                        {
                            currectUsrer.Statistics.Add(stats);
                            currectUsrer.SaveUser();
                        }
                        break;
                    case 1:
                        ShowStatistics();
                        Console.ReadKey();
                        break;
                    case 2:
                        ShowSettings();
                        break;
                    case 3:
                        GameView.CreateQuiz();
                        break;
                    case 4:
                        GameView.ChangeQuiz();
                        break;
                    case 5:
                        return;
                }
                Console.Clear();
            }
        }

        private static void ShowSettings()
        {
            Console.WriteLine("Name: " + currectUsrer.Name);
            Console.WriteLine("Login: " + currectUsrer.Login);
            Console.WriteLine("Password: " + currectUsrer.Password);
            Console.WriteLine("Date of birth: " + currectUsrer.Birthday);
            int option = ConsoleUI.View.Menu(new string[]
            {
                "Change password",
                "Change date of birth",
                "Exit"
            }, "Choose the option to change");
            switch (option)
            {
                case 0:
                    string password = GetValidValue("Enter a new password (8 or more symbols): ",
                @"^.{8,}$");
                    currectUsrer.Password = password;
                    currectUsrer.SaveUser();
                    Console.WriteLine("Password is changed");
                    break;
                case 1:
                    currectUsrer.Birthday = GetValidDate("Enter a new date: ");
                    currectUsrer.SaveUser();
                    Console.WriteLine("Date of birth is changed");
                    break;
                case 2:
                    return;
            }
        }

        private static void ShowStatistics()
        {
            
            if(currectUsrer.Statistics.Count == 0 || Stats.Categories.Count == 0)
            {
                Console.WriteLine("No game is played yet");
                return;
            }
            int num = 1;
            foreach(var stats in currectUsrer.Statistics)
            {
                Console.WriteLine("Game " + (num++));
                if(stats.CategoryIndex == Stats.Categories.Count)
                    Console.WriteLine("Category: mixed categories");
                else 
                    Console.WriteLine("Category: " + Stats.Categories[stats.CategoryIndex]);
                for(int i = 0; i < stats.Answers.Count; i++)
                {
                    Console.ForegroundColor = stats.Answers[i] ? 
                        ConsoleColor.Green : ConsoleColor.Red;
                    Console.Write('#');
                    Console.ResetColor();
                }
                int count = stats.Answers.Count(a => a);
                Console.WriteLine($"\n{count}/{stats.Answers.Count()}");
                Console.WriteLine("------------------");
            }
        }

        private static void SetUser()
        {
            while (currectUsrer == null)
            {
                currectUsrer = EnterWithUser();
                Console.Clear();
            }
        }

        private static User EnterWithUser()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\t\tWelcome to quiz game!\n");
            Console.ResetColor();
            int option = ConsoleUI.View.Menu(new string[]
            {
                "Login", "Register", "Exit"
            });
            if(option == 0)
            {
                return LoginUser();
            }
            else if(option == 1)
            {
                return RegisterUser();
            }
            else
            {
                Environment.Exit(0);
                return null;
            }
        }

        private static User RegisterUser()
        {
            string name = GetValidValue("Enter your nickname: ", @"^[a-zA-Z]+$");
            string login = GetValidValue("Enter your login (minimum 4 symbols): ",
                @"^\w{4,}$");
            if (User.IsUserExist(login))
            {
                MessageBox.Show("The login is already taken", "Ops...", 
                    MessageBoxButtons.OK);
                return null;
            }
            string password = GetValidValue("Enter your password (8 or more symbols): ",
                @"^.{8,}$");
            DateTime date = GetValidDate("Enter your date of birth: ");
            var user = new User(name, login, password, date);
            User.SaveNewUser(user);
            Console.WriteLine("Congrats! Now you can play!");
            return user;
        }

        public static DateTime GetValidDate(string text)
        {
            while (true)
            {
                Console.Write(text);
                if (DateTime.TryParse(Console.ReadLine(), out DateTime dateTime))
                {
                    return dateTime;
                }
                else
                    MessageBox.Show("Date is incorrect", "Ops...",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static User LoginUser()
        {
            string login = GetValidValue("Enter your login: ", @"^\w{4,}$");
            if (!User.IsUserExist(login))
            {
                MessageBox.Show("There is no user with login " + login, "Ops...", 
                    MessageBoxButtons.OK);
                return null;
            }
            string password = GetValidValue("Enter your password: ", @"^.{8,}$");
            if (User.CheckUser(login, password, out User user))
            {
                Console.WriteLine("Hello, " + user.Name);
                return user;
            }
            else
            {
                MessageBox.Show("Password is incorrect", "Ops...", MessageBoxButtons.OK);
                return null;
            }
        }

        public static string GetValidValue(string text, string pattern)
        {
            while (true)
            {
                Console.Write(text);
                string value = Console.ReadLine();
                if (Regex.IsMatch(value, pattern))
                    return value;
                else MessageBox.Show("Incorrect value", "Ops...", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
