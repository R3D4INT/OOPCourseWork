using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;
using Core.Models.BaseModels;
using Core.Models.Wallets;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Core.Models.Persons
{
        public class User : ProfileBase
        {
            public Wallet Wallet { get; set; }
            public Guid WalletId { get; set; }
            public User()
            {
                Role = Role.User;
                Balance = 0;
            }

            public User(string name, string surname, string phoneNumber, string address, int age, string email,
                string country, Gender gender, Role role, double income, Guid id, Wallet wallet, List<Guid> followers)
            {
                Name = name;
                Surname = surname;
                PhoneNumber = phoneNumber;
                Adress = address;
                Age = age;
                Email = email;
                Country = country;
                Gender = gender;
                Role = role;
                FollowersIds = followers;
                Income = income;
                Id = id;
                Wallet = wallet;
            }
        }
}
