using System;

namespace Organizer.Model
{
    public class User : IComparable
    {
        public int ID { get; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public User(string login, string password)
        {
            this.Login = login;
            this.Password = password;
        }
        public User(int id, string login, string password, string name, string surname, string email, string phoneNumber)
        {
            this.ID = id;
            this.Login = login;
            this.Password = password;
            this.Name = name;
            this.Surname = surname;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
        }
        public override string ToString()
        {
            return this.Name + " " + this.Surname;
        }

        public int CompareTo(object obj)
        {
            return Login.CompareTo(obj);
        }
    }
}
