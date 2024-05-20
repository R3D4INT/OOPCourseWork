using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Persons;
using Core.Models.Wallets;

namespace Core.Models.BaseModels
{
    public class DealBase : BaseEntity
    {
        public Guid CoinId { get; set; }
        public Guid UserId { get; set; }
    }
}
