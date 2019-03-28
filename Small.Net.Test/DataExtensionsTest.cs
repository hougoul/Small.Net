using NUnit.Framework;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using Small.Net.Extensions;
using System.Linq;

namespace Small.Net.Test
{
    [TestFixture]
    public class DataExtensionsTest
    {
        private DbConnection _connection;
        private const string _dataSource = "TestDb.sqlite";

        [OneTimeSetUp]
        public void Setup()
        {
            SQLiteConnection.CreateFile(_dataSource);
            _connection = SQLiteFactory.Instance.CreateConnection();
            _connection.ConnectionString = "Data Source=TestDb.sqlite;";
            _connection.Open();
            using (var dc = _connection.CreateCommand())
            {
                dc.CommandText = "create table highscores (name varchar(20), score int)";
                dc.CommandType = System.Data.CommandType.Text;
                dc.ExecuteNonQuery();

                dc.CommandText = "insert into highscores (name, score) values ('Me', 3000)";
                dc.ExecuteNonQuery();

                dc.CommandText = "insert into highscores (name, score) values ('Myself', 6000)";
                dc.ExecuteNonQuery();
            }
        }

        [Test]
        public void ConvertToTest()
        {
            using (var dc = _connection.CreateCommand())
            {
                dc.CommandText = "select name, score from highscores order by score desc";
                dc.CommandType = System.Data.CommandType.Text;

                using (var dr = dc.ExecuteReader())
                {
                    var scores = dr.ConvertTo<HighScore>().Result.ToList();
                    Assert.AreEqual(2, scores.Count);
                    Assert.AreEqual("Myself", scores[0].Name);
                    Assert.AreEqual(6000, scores[0].Score);
                }
            }
        }

        [OneTimeTearDown]
        public void Clean()
        {
            _connection?.Dispose();
            SQLiteConnection.Shutdown(true, true);
            File.Delete(_dataSource);
        }
    }

    public class HighScore
    {
        public string Name { get; set; }

        public int Score { get; set; }
    }
}
