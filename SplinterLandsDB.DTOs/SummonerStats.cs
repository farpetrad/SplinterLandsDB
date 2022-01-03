namespace SplinterLandsDB.DTOs
{
    public class SummonerStats : CardStats
    {
        public int Armor { get; set; } = -1;
        public int Attack { get; set; } = -1;
        public int Health { get; set; } = -1;
        public int Magic { get; set; } = -1;
        public int Mana { get; set; } = -1;
        public int Ranged { get; set; } = -1;
        public int Speed { get; set; } = -1;
        public string Abilities { get; set; }= string.Empty;
    }
}
