using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Cassandra;
using Newtonsoft.Json.Linq;
using Dse.Auth;
using System.Threading;
using System.Linq;

namespace Frends.Cassandra
{
    public class Parameter
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class CassandraInput
    {
        /// <summary>
        /// Command timeout in seconds
        /// </summary>
        [DisplayFormat(DataFormatString = "Sql")]
        public string query { get; set; }
        /// <summary>
        /// Parameters for query.
        /// </summary>
        public Parameter[] parameters { get; set; }

    }

    public class Options
    {
        /// <summary>
        /// Command timeout in seconds
        /// </summary>
        [DefaultValue(60)]
        public int commandTimeoutSeconds { get; set; }

    }

    public class Connection
    {
        /// <summary>
        /// Database user username
        /// </summary>
        [DefaultValue("Username")]
        public string username { get; set; }

        /// <summary>
        /// Database user password
        /// </summary>
        [PasswordPropertyText]
        [DefaultValue("Password")]
        public string password { get; set; }

        /// <summary>
        /// Database port
        /// </summary>
        [DefaultValue(7000)]
        public int port { get; set; }

        /// <summary>
        /// IP address
        /// </summary>
        [DefaultValue("127.0.0.1")]
        public string nodes { get; set; }

    }

    /// <summary>
    /// Example task package for handling files
    /// </summary>
    public class CassandraTask
    {

        public static JToken ExecuteQuery([PropertyTab] CassandraInput input, [PropertyTab] Options options, [PropertyTab] Connection connection)
        {

            ISession session = GetCassandraDatabase(connection.nodes, connection.port, connection.username, connection.password);

            RowSet rowSet = ApplyQueryToSession(session, input.query);

            return new JArray(rowSet);

        }

        protected static ISession GetCassandraDatabase(string nodes, int serverPort, string username, string password)
        {
            // Establish the connection:
            // Plaintext auth
            var authProvider = new PlainTextAuthProvider(username, password);
            var cluster = Cluster.Builder().AddContactPoints(nodes).WithPort(serverPort).WithAuthProvider(authProvider).Build();
            ISession session = cluster.Connect();

            return session;
        }

        protected static RowSet ApplyQueryToSession(ISession session, string query)
        {
            var result = session.Execute(query);

            return result;
        }

        public class CassandraUtils
        {
            /// <summary>
            /// Execute a cql query.
            /// </summary>
            /// <param name="input">Input parameters</param>
            /// <param name="options">Optional parameters with default values</param>
            /// <returns>JToken</returns>
            public static async Task<object> ExecuteCqlQuery([PropertyTab] CassandraInput input, [PropertyTab] Connection connection, [PropertyTab] Parameter[] parameter, CancellationToken cancellationToken)
            {
                return await GetCassandraResult(input, connection, parameter, cancellationToken).ConfigureAwait(false);

               

            }

            private static async Task<JToken> GetCassandraResult([PropertyTab] CassandraInput input, [PropertyTab] Connection connection, [PropertyTab] Parameter[] parameters, [PropertyTab] CancellationToken cancellationToken)
            {
                var authProvider = new PlainTextAuthProvider(connection.username, connection.password);
                var cluster = Cluster.Builder().AddContactPoints(connection.nodes).WithPort(connection.port).WithAuthProvider(authProvider).Build();

                using (ISession session = cluster.Connect())
                {
                    IDictionary<string, object> paramDict = parameters.ToDictionary(param => param.name, param => (object)param.value);
                    IStatement statement = new SimpleStatement(paramDict, input.query);
                    var rs = await session.ExecuteAsync(statement);

                    return JToken.FromObject(rs);
                }

            }
            private static Dictionary<string, object> ParameterToDictionary(Parameter[] parameters)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();

                

                return dict;
            }
        }
    }
}
