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

namespace Frends.Cassandra
{
    public class Parameter
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class CassandraInput
    {
        /// <summary>
        /// Command timeout in seconds
        /// </summary>
        [DisplayFormat(DataFormatString = "Cql")]
        public string Query { get; set; }
        /// <summary>
        /// Parameters for query.
        /// </summary>
        public Parameter[] Parameters { get; set; }

    }

    public class Options
    {
        /// <summary>
        /// Command timeout in seconds
        /// </summary>
        [DefaultValue(60)]
        public int CommandTimeoutSeconds { get; set; }

    }

    public class Connection
    {
        /// <summary>
        /// Database user username
        /// </summary>
        [DefaultValue("Username")]
        public string Username { get; set; }

        /// <summary>
        /// Database user password
        /// </summary>
        [DefaultValue("Password")]
        public string Password { get; set; }

        /// <summary>
        /// Database port
        /// </summary>
        [DefaultValue(7000)]
        public int Port { get; set; }

        /// <summary>
        /// IP address
        /// </summary>
        [DefaultValue("127.0.0.1")]
        public string Nodes { get; set; }

    }

    /// <summary>
    /// Example task package for handling files
    /// </summary>
    public class CassandraTask
    {

        public static JObject ExecuteQuery(CassandraInput input, Options options, Connection connection)
        {

            ISession session = GetCassandraDatabase(connection.Nodes, connection.Port, connection.Username, connection.Password);

            RowSet rowset = executeQuery(session, input.Query);

            return JObject.FromObject(rowset);

        }

        private static ISession GetCassandraDatabase(string Nodes, int ServerPort, string Username, string Password)
        {
            // Establish the connection:
            // Plaintext auth
            var authProvider = new PlainTextAuthProvider(Username, Password);
            var cluster = Cluster.Builder().AddContactPoints(Nodes).WithPort(ServerPort).WithAuthProvider(authProvider).Build();
            ISession session = cluster.Connect();

            return session;
        }

        private static RowSet executeQuery(ISession session, string query)
        {
            var result = session.Execute(query);

            return result;
        }

        public class Cql
        {
            /// <summary>
            /// Execute a cql query.
            /// </summary>
            /// <param name="input">Input parameters</param>
            /// <param name="options">Optional parameters with default values</param>
            /// <returns>JToken</returns>
            public static async Task<object> ExecuteQuery(CassandraInput input, Connection connection, CancellationToken cancellationToken, Parameter[] parameters)
            {
                return await GetCassandraResult(input, connection, cancellationToken, parameters).ConfigureAwait(false);

                //input.Query, input.ConnectionString, input.Parameters, options, SqlCommandType.Text, cancellationToken).ConfigureAwait(false);
                //string query, string connectionString, IEnumerable<Parameter> parameters, Options options, SqlCommandType commandType, CancellationToken cancellationToken

            }

            //----------------

            private static async Task<JToken> GetCassandraResult(CassandraInput input, Connection connection, CancellationToken cancellationToken, Parameter[] parameters)
            {
                var authProvider = new PlainTextAuthProvider(connection.Username, connection.Password);
                var cluster = Cluster.Builder().AddContactPoints(connection.Nodes).WithPort(connection.Port).WithAuthProvider(authProvider).Build();

                using (ISession session = cluster.Connect())
                {
                    IDictionary<string, object> paramdict = ParameterToDictionary(parameters);
                    IStatement statement = new SimpleStatement(paramdict, input.Query);
                    var rs = await session.ExecuteAsync(statement);

                    return JToken.FromObject(rs);
                }

            }
            private static Dictionary<string, object> ParameterToDictionary(Parameter[] parameters)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();

                foreach (var parameter in parameters)
                {
                    dict.Add(parameter.Name, parameter.Value);
                }

                return dict;
            }
        }
    }
}
