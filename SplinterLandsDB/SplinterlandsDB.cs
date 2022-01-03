using Dapper;
using Microsoft.Extensions.Logging;
using SplinterLandsDB.DTOs;
using System.Data;
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

        public void AddCard(SplinterLands.DTOs.Models.Card card)
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

        private static Guid InsertCardethod(SplinterLands.DTOs.Models.Card card, SqlConnection connection, SqlTransaction transaction)
        {
            if(DoesCcardExist(card.Id, connection)) return Guid.Empty;

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
        private static void InsertSummonerStats(SplinterLands.DTOs.Models.CardStats stats, Guid cardKey, SqlConnection connection, SqlTransaction transaction)
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

        private static void InsertMonsterStats(SplinterLands.DTOs.Models.CardStats stats, Guid cardKey, SqlConnection connection, SqlTransaction transaction)
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
            if (id <= 0) throw new ArgumentException("Provided Id must be > 0", nameof(id));

            using(var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                const string selectCardTypeSql = "SELECT Type FROM Cards WITH(NOLOCK) WHERE Id=@Id";
                var cardType = connection.QuerySingle<string>(selectCardTypeSql, new { Id = id });



                if (string.IsNullOrEmpty(cardType)) throw new Exception("Error in GetCardById, no card found with specified id");

                switch (cardType.ToLower())
                {
                    case "summoner":
                        return GetSummonerById(id, connection);
                    case "monster":
                        return GetMonsterById(id, connection);
                    default:
                        throw new Exception($"No card type of {cardType} was found");
                }
            }
            
        }

        private static bool DoesCcardExist(int id, IDbConnection connection)
        {
            connection.Open();
            const string selectCardTypeSql = "SELECT COUNT(*) FROM Cards WITH(NOLOCK) WHERE Id=@Id";
            var count = connection.QuerySingle<int>(selectCardTypeSql, new { Id = id });
            return count > 0;
        }

        private Card GetSummonerById(int cardId, IDbConnection connection)
        {
            const string sql = $@"SELECT * FROM Cards WITH(NOLOCK)
                                  INNER JOIN SummonerStats ST WITH(NOLOCK)
                                   ON ST.CardKey = Cards.CardKey
                                WHERE Id=@Id";
            var card = connection.Query<
                Card, SummonerStats, Card>(sql, (card, stats) =>
                {
                    card.Stats = stats;
                    return card;
                }, new { Id = cardId }).FirstOrDefault() ?? new Card();
            return card;
        }

        private Card GetMonsterById(int cardId, IDbConnection connection)
        {
            const string sql = $@"SELECT * FROM Cards WITH(NOLOCK)
                                  INNER JOIN MonsterStats MT WITH(NOLOCK)
                                   ON MT.CardKey = Cards.CardKey
                                WHERE Id=@Id";

            var card = connection.Query<
                Card, MonsterStats, Card>(sql, (card, stats) =>
                {
                    card.Stats = stats;
                    return card;
                }, new { Id = cardId }).FirstOrDefault() ?? new Card();
            

            return card;
        }

        public Card GetCardByName(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Provided name must be valid and have length > 0", nameof(name));
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
