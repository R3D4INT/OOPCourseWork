using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;
using Core.Models.Wallets;

namespace Core.Models.BaseModels
{
    public abstract class WalletBase : BaseEntity
    {
        [MaxLength(12)]
        public SeedPhrase SeedPhrase { get; set; }
        public Guid SeedPhraseId { get; set; }
        public List<Coin> AmountOfCoins { get; set; }

        public WalletBase()
        {
           
        }

        public void SeedPhraseSet(SeedPhrase seedPhrase)
        {
            SeedPhrase = seedPhrase;
        }
    }
}
