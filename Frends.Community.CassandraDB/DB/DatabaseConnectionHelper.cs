using System;
using Cassandra;
using Newtonsoft.Json.Linq;
using Dse.Auth;

namespace Frends.Community.CassandraDB.Helpers
{
    public class DatabaseConnectionHelper
    {

        public ISession GetCassandraDatabase(string nodes, int serverPort, string username, string password)
        {
            // Establish the connection:
            // Plaintext auth
            var authProvider = new PlainTextAuthProvider(username, password);
            var cluster = Cluster.Builder().AddContactPoints(nodes).WithPort(serverPort).WithAuthProvider(authProvider).Build();
            ISession session = cluster.Connect();

            return session;
        }

        public RowSet executeQuery(ISession session, string query)
        {
            var result = session.Execute(query);

            return result;
        }

        public JObject cassandraCommand(string nodes, string serverPort, string query, string username, string password)
        {
            // Cassandra default port for cluster communication
            int port = 9042;

            try
            {
                port = Int32.Parse(serverPort);
            } 
            catch(InvalidCastException e)
            {

            }

            var ses = GetCassandraDatabase(nodes, port, username, password);

            var rs = executeQuery(ses, query);

            var jresult = JObject.FromObject(rs);
            
            return jresult;

        }
    }
}
