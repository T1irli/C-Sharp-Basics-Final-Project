using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataLogicLayer;

namespace BusinessLogicLayer.Models
{
    [Serializable]
    public class User
    {
        [NonSerialized]
        private static string path = "users.xml";

        private string name;
        private string login;
        private string password;
        private DateTime birthday;

        public string Name { get => name;
            set{
                if (!Regex.IsMatch(value, @"^[a-zA-Z]+$"))
                    throw new ArgumentException();
                else name = value;
            }
        }
        public string Login { get => login;
            set{
                if (!Regex.IsMatch(value, @"^\w+$"))
                    throw new ArgumentException();
                else login = value;
            }
        }
        public string Password { get => password;
            set {
                if (!Regex.IsMatch(value, @"^.{8,}$"))
                    throw new ArgumentException();
                else password = value;
            } 
        }
        public DateTime Birthday { get => birthday;
            set{
                if(value > DateTime.Now)
                    throw new ArgumentException();
                else birthday = value;
            }
        }
        public List<Stats> Statistics { get; set; }

        public User(string name, string login, string password, DateTime birthday)
        {
            this.Name = name;
            this.Login = login;
            this.Password = password;
            this.Birthday = birthday;
            this.Statistics = new List<Stats>();
        }

        public User()
        {
            this.Statistics = new List<Stats>();
        }

        public void SaveUser()
        {
            var users = FileXmlSerrealization.Read<List<User>>(path);
            users[users.FindIndex(u => u.Login == this.Login)] = this;
            FileXmlSerrealization.Write<List<User>>(path, users);
        }

        public static bool IsUserExist(string login)
        {
            var users = FileXmlSerrealization.Read<List<User>>(path);
            return users.Find(u => u.Login == login) != null;
        }

        public static bool CheckUser(string login, string password, out User user)
        {
            var users = FileXmlSerrealization.Read<List<User>>(path);
            user = users.Find(u => u.Login == login);
            return user.Password == password;
        }

        public static void SaveNewUser(User user)
        {
            var users = FileXmlSerrealization.Read<List<User>>(path);
            users.Add(user);
            FileXmlSerrealization.Write<List<User>>(path, users);
        }
    }
}
