using Microsoft.VisualStudio.TestTools.UnitTesting;
using SplinterLandsAPI;
using System.Linq;

namespace SplinterLandsDB.Test
{
    [TestClass]
    public class DBTest
    {
        [TestMethod]
        public void TestAddCard()
        {
            var client = new SplinterLandsClient(null);

            Assert.IsNotNull(client);

            var set = client.GetCards();

            Assert.IsNotNull(set);
            Assert.IsNotNull(set.Cards);

            var testCard = set.Cards.First();

            Assert.IsNotNull(testCard);
            

            var db = new SplinterLandsDB.SplinterlandsDB(null)
            {
                ConnectionString = new System.Data.SqlClient.SqlConnectionStringBuilder()
                {
                    InitialCatalog = "Splinterlands",
                    UserID = "SplinterlandsReader",
                    Password = "420splinterlands#HIVE#Play2Earn$#bot69",
                    DataSource = "LAPTOP-UIBR26H4\\SQLEXPRESS"
                }.ConnectionString
            };

            set.Cards.ForEach( card => db.AddCard(card));
        }
    }
}