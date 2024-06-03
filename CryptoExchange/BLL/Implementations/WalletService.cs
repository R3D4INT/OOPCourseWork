using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;
using DAL.Implementations;
using System.Globalization;
using System.Text.Json;
using BLL.Interfaces.IServiceInterfaces;
using Core.Models.Wallets;

namespace BLL.Implementations
{
    public class WalletService : GenericService<Wallet>, IWalletService
    {
        private const int SEED_PHRASE_LENGTH = 12;

        private readonly ISeedPhraseService _seedPhraseService;

        private readonly IHttpRequests _httpRequests;

        private readonly ICoinService _coinService;
        public WalletService(IGenericRepository<Wallet> repository, ISeedPhraseService seedPhraseService, 
            IHttpRequests httpRequests, ICoinService coinService) :
            base(repository)
        {
            _seedPhraseService = seedPhraseService;
            _httpRequests = httpRequests;
            _coinService = coinService;
        }
        public async Task<Wallet> CreateWallet()
        {
            try
            {
                var wallet = new Wallet();
                wallet.Id = Guid.NewGuid();
                wallet.AmountOfCoins = await CreateListOfCoins(wallet.Id);
                wallet.SeedPhraseSet(await CreateSeedPhrase());
                if (wallet == null)
                {
                    throw new Exception("Failed to create wallet");
                }
               
                return wallet;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create wallet {ex.Message}");
            }
        }
        private async Task<List<Coin>> CreateListOfCoins(Guid walletId)
        {
            var coinList = new List<Coin>();
            foreach (NameOfCoin name in Enum.GetValues(typeof(NameOfCoin)))
            {
                var coin = new Coin() {Amount = 0, Id = Guid.NewGuid(), Name = name, Price = await _httpRequests.GetPriceFromBinance(name), WalletId = walletId};
                coinList.Add(coin);
            }
            return coinList;
        }
        private async Task<SeedPhrase> CreateSeedPhrase()
        {
            try
            {
                var seedPhraseBase = await _seedPhraseService.GetSeedPhraseBase();
                var random = new Random();
                var seedPhrase = new SeedPhrase();
                seedPhrase.Id = Guid.NewGuid();
                seedPhrase.SeedPhraseValues = new List<string>(); 

                for (var i = 0; i < SEED_PHRASE_LENGTH; i++)
                {
                    var randomWord = seedPhraseBase.SeedPhraseValues[random.Next(seedPhraseBase.SeedPhraseValues.Count)];
                    seedPhrase.SeedPhraseValues.Add(randomWord);
                }
                return seedPhrase;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create seed phrase {ex.Message}");
            }
        }

        public async Task<Wallet> GetWallet(Guid walletId)
        {
            try
            {
                var wallet = await GetSingleByCondition(e => e.Id == walletId);
                var seedPhrase = await _seedPhraseService.GetSingleByCondition(e => e.Id == wallet.SeedPhraseId);
                wallet.SeedPhrase = seedPhrase;
                var coins = await _coinService.GetListByCondition(e => e.WalletId == walletId);
                wallet.AmountOfCoins = (List<Coin>) coins;
                return wallet;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task UpdateCoin(Coin coin)
        {
            try
            {
                await _coinService.Update(coin, e => e.Id == coin.Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}