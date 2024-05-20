using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;
using Core.Models.BaseModels;

namespace Core.Models.Wallets
{
    public class WalletForMarket : WalletBase
    {
        public static Guid CurrentId { get; set; }
    }
}
