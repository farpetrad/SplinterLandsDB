using Dapper;
using Microsoft.Extensions.Logging;
using SplinterLandsAPI.Models;
using System.Data.SqlClient;

namespace SplinterLandsDB
{
    public class SplinterlandsDB : ISplinterlandsDB
    {
        private readonly ILogger _logger;
        public SplinterlandsDB(ILogger logger)
        {
            _logger = logger;
        }
        public string ConnectionString { get; set; } = string.Empty;

        public void AddCard(Card card)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        var inserted = InsertCardethod(card, connection, transaction);
                        if(inserted != Guid.Empty)
                        {
                            switch (card.Type)
                            {
                                case "Summoner":
                                    InsertSummonerStats(card.Stats, inserted, connection, transaction);
                                    break;
                                case "Monster":
                                    InsertMonsterStats(card.Stats, inserted, connection, transaction);
                                    break;
                                default:
                                    _logger.LogWarning($"Card {inserted} does will not have an attributes record");
                                    break;
                            }
                        }
                        transaction.Commit();
                    }
                    connection.Close();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occured inserting card");
            }
        }

        private static Guid InsertCardethod(Card card, SqlConnection connection, SqlTransaction transaction)
        {
            const string sql = @$"INSERT INTO Cards(Id, Name, Color, Type, Total_Printed, Rarity, Is_Promo, Is_Starter)
                                        OUTPUT INSERTED.CardKey
                                        VALUES(@Id, @Name, @Color, @Type, @Total_Printed, @Rarity, @Is_Promo, @Is_Starter)";

            var insertedId = connection.QuerySingle<Guid>(
                sql,
                new
                {
                    Id = card.Id,
                    Name = card.Name,
                    Color = card.Color,
                    Type = card.Type,
                    Total_Printed = card.Total_printed,
                    Rarity = card.Rarity,
                    Is_Promo = card.Is_Promo,
                    Is_Starter = card.Is_Starter
                },
                transaction);

            return insertedId;
        }
        private static void InsertSummonerStats(CardStats stats, Guid cardKey, SqlConnection connection, SqlTransaction transaction)
        {
            const string sql = $@"INSERT INTO SummonerStats (CardKey, Armor, Attack, Health, Magic, Mana, Ranged, Speed, Abilities)
                                          VALUES(@CardKey, @Armor, @Attack, @Health, @Magic, @Mana, @Ranged, @Speed, @Abilities)";
            var success = connection.Execute(sql, new { 
                CardKey = cardKey,
                Armor = stats.Armor,
                Attack = stats.Attack,
                Health = stats.Health,
                Magic = stats.Magic,
                Mana = stats.Mana,
                Ranged = stats.Ranged,
                Speed = stats.Speed,
                Abilities = String.Join(",",stats.Abilities as List<string> ?? new List<string>())
            }, transaction);
        }

        private static void InsertMonsterStats(CardStats stats, Guid cardKey, SqlConnection connection, SqlTransaction transaction)
        {
            const string sql = $@"INSERT INTO MonsterStats (CardKey, Armor, Attack, Health, Magic, Mana, Ranged, Speed, Abilities)
                                          VALUES(@CardKey, @Armor, @Attack, @Health, @Magic, @Mana, @Ranged, @Speed, @Abilities)";

            var success = connection.Execute(sql, new
            {
                CardKey = cardKey,
                Armor = String.Join(",", stats.Armor as List<string> ?? new List<string>()),
                Attack = String.Join(",", stats.Attack as List<string> ?? new List<string>()),
                Health = String.Join(",", stats.Health as List<string> ?? new List<string>()),
                Magic = String.Join(",", stats.Magic as List<string> ?? new List<string>()),
                Mana = String.Join(",", stats.Mana as List<string> ?? new List<string>()),
                Ranged = String.Join(",", stats.Ranged as List<string> ?? new List<string>()),
                Speed = String.Join(",", stats.Speed as List<string> ?? new List<string>()),
                Abilities = String.Join(",",stats.Abilities as List<string> ?? new List<string>())
            }, transaction);
        }

        public Card GetCardById(int id)
        {
            throw new NotImplementedException();
        }

        public Card GetCardByName(string name)
        {
            throw new NotImplementedException();
        }

        public void RemoveCard(Card card)
        {
            throw new NotImplementedException();
        }

        public void UpdateCard(Card card)
        {
            throw new NotImplementedException();
        }
    }
}
