using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SplinterLandsAPI;
using System.Linq;

namespace SplinterLandsDB.Test
{
    [TestClass]
    public class DBTest
    {
        private ILogger Log => new Mock<ILogger>().Object;

        [TestMethod]
        public void TestAddCard()
        {
            var client = new SplinterLandsClient(Log);

            Assert.IsNotNull(client);

            var set = client.GetCards();

            Assert.IsNotNull(set);
            Assert.IsNotNull(set.Cards);

            var testCard = set.Cards.First();

            Assert.IsNotNull(testCard);
            

            var db = new SplinterLandsDB.SplinterlandsDB(Log)
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

        [TestMethod]
        public void TestGetMonsterById()
        {
            var db = new SplinterLandsDB.SplinterlandsDB(Log)
            {
                ConnectionString = new System.Data.SqlClient.SqlConnectionStringBuilder()
                {
                    InitialCatalog = "Splinterlands",
                    UserID = "SplinterlandsReader",
                    Password = "420splinterlands#HIVE#Play2Earn$#bot69",
                    DataSource = "LAPTOP-UIBR26H4\\SQLEXPRESS"
                }.ConnectionString
            };

            var monster = db.GetCardById(1);
            Assert.IsNotNull(monster);
        }

        [TestMethod]
        public void TestGeSummonerById()
        {
            var db = new SplinterLandsDB.SplinterlandsDB(Log)
            {
                ConnectionString = new System.Data.SqlClient.SqlConnectionStringBuilder()
                {
                    InitialCatalog = "Splinterlands",
                    UserID = "SplinterlandsReader",
                    Password = "420splinterlands#HIVE#Play2Earn$#bot69",
                    DataSource = "LAPTOP-UIBR26H4\\SQLEXPRESS"
                }.ConnectionString
            };

            var monster = db.GetCardById(236);
            Assert.IsNotNull(monster);
        }
    }
}