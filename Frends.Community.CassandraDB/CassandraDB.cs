using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Cassandra;
using Newtonsoft.Json.Linq;
using Dse.Auth;

/// <summary>
/// Input for the Write Task
/// </summary>
public class CassandraInput
{
    /// <summary>
    /// Text content to be written to the file
    /// </summary>
    public string query { get; set; }

}

/// <summary>
/// Options for the Write Task
/// </summary>
public class CassandraOptions
{
    /// <summary>
    /// What should happen if the file already exist
    /// </summary>
    [DisplayName("Server Port")]
    [DefaultValue("")]
    public int serverPort { get; set; }

    [DisplayName("Address")]
    [DefaultValue("")]
    public string nodes { get; set; }

    [DisplayName("Username")]
    [DefaultValue("")]
    public string username { get; set; }

    [DisplayName("Password")]
    [DefaultValue("")]
    public string password { get; set; }
}

/// <summary>
/// Example task package for handling files
/// </summary>
public class CassandraTask
{
    /// <summary>
    /// This Task allows you to delete a file
    /// </summary>
    /// <param name="path">Full path to the file that should be delete</param>
    /// <returns>Returns true if file was able to be deleted otherwise throws an exception</returns>
    public static JObject ExecuteQuery(CassandraInput input, CassandraOptions options)
    {

        ISession session = GetCassandraDatabase(options.nodes, options.serverPort, options.username, options.password);

        RowSet rowset = executeQuery(session, input.query);

        return JObject.FromObject(rowset);
        
    }

    private static ISession GetCassandraDatabase(string nodes, int serverPort, string username, string password)
    {
        // Establish the connection:
        // Plaintext auth
        var authProvider = new PlainTextAuthProvider(username, password);
        var cluster = Cluster.Builder().AddContactPoints(nodes).WithPort(serverPort).WithAuthProvider(authProvider).Build();
        ISession session = cluster.Connect();

        return session;
    }

    private static RowSet executeQuery(ISession session, string query)
    {
        var result = session.Execute(query);

        return result;
    }


}