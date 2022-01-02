using SplinterLandsAPI.Models;

namespace SplinterLandsDB
{
    public interface ISplinterlandsDB
    {
        string ConnectionString { get; set; }
        void AddCard(Card card);
        void RemoveCard(Card card);
        void UpdateCard(Card card);
        Card GetCardByName(string name);
        Card GetCardById(int id);
    }
}
