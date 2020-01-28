using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Cassandra;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Moq;
using System.Net;
using System.Linq;
using Frends.Cassandra;
using System.Collections.Generic;

namespace Frends.CassandraDB.Tests
{
    public class CassandraTaskHelper : CassandraTask
    {

        public virtual ISession GetCassandraDatabaseWrapper(string Nodes, int ServerPort, string Username, string Password)
        {
            //return CassandraTask.GetCassandraDatabase(Nodes, ServerPort, Username, Password);
            return null;
        }

        public virtual RowSet ApplyQueryToSessionWrapper(ISession session, string query)
        {
            //return CassandraTask.ApplyQueryToSession(session, query);
            return null;
        }

      
        public virtual JToken ExecuteQueryWrapper(CassandraInput input, Options options, Connection connection)
        {

            ISession session = GetCassandraDatabaseWrapper(connection.Nodes, connection.Port, connection.Username, connection.Password);

            RowSet rowset = ApplyQueryToSessionWrapper(session, input.Query);

            return new JArray(rowset);

        }
    }

    [TestClass]
    public class CassandraTests
    {

        [TestMethod]
        public void TestCassandraTask()
        {
            var jobject = new JObject();
            jobject.Add("result", "1234");

            var input = new CassandraInput();
            input.Query = "test query";
            var options = new Options();
            var connection = new Connection();
            connection.Nodes = "testaddress";
            connection.Port = 70001;
            connection.Username = "test user";
            connection.Password = "test password";
            

            var mock = new Mock<CassandraTaskHelper>();
            mock.CallBase = true;
            RowSet rows = new RowSet();
             
            mock.Setup(c => c.ApplyQueryToSessionWrapper(null, It.IsAny<string>()))
                .Returns((ISession session, string query) => rows);


            mock.Setup(c => c.GetCassandraDatabaseWrapper(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string nodes, int port, string username, string password) => null);

            var result = mock.Object.ExecuteQueryWrapper(input, options, connection);
            Console.WriteLine(result.ToString());
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(result.ToString());
        }

    }
}
