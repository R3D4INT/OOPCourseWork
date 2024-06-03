using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.BaseModels;

namespace Core.Models.Wallets
{
    public class PriceHistory : BaseEntity
    {
        public List<double> CoinPrices { get; set; }
        public Coin Coin { get; set; }
    }
}
