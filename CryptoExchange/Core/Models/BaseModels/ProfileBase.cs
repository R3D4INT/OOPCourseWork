using Core.Enums;
using Core.Models.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.BaseModels
{
    public class ProfileBase : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Adress { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public Gender Gender { get; set; }
        public Role Role { get; set; }
        public List<Guid> FollowersIds { get; set; }
        public double Income { get; set; }
        public double Balance { get; set; }
        public double BalanceForCopyTrading { get; set; }
        public bool IsBanned { get; set; }
        public bool IsAvailableForCopyTrade { get; set; }
    }
}
