using Core.Models.BaseModels;

namespace Core.Models.Wallets;

public class SeedPhrase : BaseEntity
{
    public List<string> SeedPhraseValues { get; set; }
}