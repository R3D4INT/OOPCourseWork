using Core.Models.Wallets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace BLL.Interfaces.IServiceInterfaces
{
    public interface ISeedPhraseService : IGenericService<SeedPhrase>
    {
        Task AddBaseWords();
        Task<SeedPhrase> GetSeedPhraseBase();
    }
}
