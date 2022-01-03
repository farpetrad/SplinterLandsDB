namespace SplinterLandsDB.DTOs
{
    public class Card
    {
        public Guid CardKey { get; set; } = Guid.Empty;
        public int Id { get; set; } = -1;
        public string Name { get; set; } = String.Empty;
        public string Color { get; set; } = String.Empty;
        public string Type { get; set; } = String.Empty;
        public int Total_Printed { get; set; } = -1;
        public CardRarity Rarity { get; set; } = CardRarity.Unknown;
        public bool Is_Promo { get; set; } = false;
        public bool Is_Starter { get; set; } = false;
        public CardStats Stats { get; set; } =  new CardStats();
    }
}
