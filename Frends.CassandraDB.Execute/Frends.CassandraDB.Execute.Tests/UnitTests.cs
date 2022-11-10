using Microsoft.VisualStudio.TestTools.UnitTesting;
using Frends.CassandraDB.Execute.Definitions;

namespace Frends.CassandraDB.Execute.Tests;

[TestClass]
public class UnitTests
{
    /* 
     * docker run --rm -d -p 9042:9042 cassandra:4
     * 
     * (Optional) Run following command in \Frends.CassandraDB.Execute.Tests\Files\ to build up test DB. 
     * docker run --rm --network cassandra -v "$(pwd)/data.cql:/scripts/data.cql" -e CQLSH_HOST=cassandra -e CQLSH_PORT=9042 -e CQLVERSION=3.4.5 nuvo/docker-cqlsh
     * Note: It might take some time to get Cassandra running, so this command might fail. Wait for ~20sec and try again.
     * 
     * (Optional) Test connection:
     * docker run --rm -it --network cassandra nuvo/docker-cqlsh cqlsh cassandra 9042 --cqlversion='3.4.5'
     * Terminal: 
     *  cqlsh>SELECT * FROM store.shopping_cart;
     *  cqlsh>INSERT INTO store.shopping_cart (userid, item_count) VALUES ('4567', 20);
     * 
     * 
     * Cleanup:
     * docker kill cassandra
     * docker network rm cassandra
    */

    /// <summary>
    /// Creating testing DB. Sleep(40000) because it might take a while to start a fresh Cassandra DB (docker / Workflow)
    /// </summary>
    [TestInitialize]
    public void Startup()
    {
        Thread.Sleep(40000); //For first run. Can be commented out.

        var queries = new List<string>()
        {
            "CREATE KEYSPACE IF NOT EXISTS store WITH REPLICATION = { 'class' : 'SimpleStrategy', 'replication_factor' : '1' };",
            "CREATE TABLE IF NOT EXISTS store.shopping_cart (userid text PRIMARY KEY,item_count int,last_update_timestamp timestamp);",
            "INSERT INTO store.shopping_cart(userid, item_count, last_update_timestamp)VALUES ('9876', 2, toTimeStamp(now()));"
        };

        foreach (var query in queries)
        {
            var _input = new Input()
            {
                ContactPoints = new[] { new ContactPoint() { Value = "localhost" } },
                Keyspace = null,
                Port = 9042,
                Query = query,
            };

            CassandraDB.Execute(_input, default);
        }
    }

    [TestMethod]
    public void Test_Execute_Insert()
    {
        var queries = new List<string>()
        {
            "INSERT INTO store.shopping_cart(userid, item_count, last_update_timestamp)VALUES ('1234', 5, toTimeStamp(now()));",
        };

        foreach (var query in queries)
        {
            var _input = new Input()
            {
                ContactPoints = new[] { new ContactPoint() { Value = "localhost" } },
                Keyspace = null,
                Port = 9042,
                Query = query,
            };

            var result = CassandraDB.Execute(_input, default);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.QueryResults.Equals("{}"));
        }
    }

    [TestMethod]
    public void Test_Execute_Select()
    {
        var _input = new Input()
        {
            ContactPoints = new[] { new ContactPoint() { Value = "localhost" } },
            Keyspace = null,
            Port = 9042,
            Query = "SELECT * FROM store.shopping_cart;",
        };

        var result = CassandraDB.Execute(_input, default);
        Assert.IsTrue(result.Success);
        Assert.IsTrue(!string.IsNullOrWhiteSpace(result.QueryResults));
    }
}