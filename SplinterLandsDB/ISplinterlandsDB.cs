using SplinterLandsDB.DTOs;

namespace SplinterLandsDB
{
    public interface ISplinterlandsDB
    {
        string ConnectionString { get; set; }
        void AddCard(SplinterLands.DTOs.Models.Card card);
        void RemoveCard(Card card);
        void UpdateCard(Card card);
        Card GetCardByName(string name);
        Card GetCardById(int id);
    }
}
