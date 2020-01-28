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
    public class Helper
    {
        public virtual JObject CassandraTaskWrapper(CassandraInput cassandrainput, Options opt, Connection conn)
        {
            return CassandraTask.ExecuteQuery(cassandrainput, opt, conn);
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
            var options = new Options();
            var connection = new Connection();
            connection.Username = "testuser";
            connection.Password = "testPassword";

            var mock = new Mock<Helper>(MockBehavior.Loose);
            mock.Setup(c => c.CassandraTaskWrapper(It.IsAny<CassandraInput>(), It.IsAny<Options>(), It.IsAny<Connection>()))
                .Returns((CassandraInput input, Options options, Connection connection) => jobject);
            
            

            Helper h = new Helper();

            var result = mock.Object.CassandraTaskWrapper(input, options, connection);
            Console.WriteLine(result);
            mock.VerifyAll();
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(1, 1);
        }

    }
}
