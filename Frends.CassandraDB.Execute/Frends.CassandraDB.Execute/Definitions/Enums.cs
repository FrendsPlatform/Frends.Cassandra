namespace Frends.CassandraDB.Execute.Definitions;

/// <summary>
/// Authentication methods.
/// </summary>
public enum AuthenticationMethods
{
    /// <summary>
    /// None.
    /// </summary>
    None,

    /// <summary>
    /// Plain-text authentication using username and password.
    /// NOTE: The PlainTextAuthProvider should only be used when authenticating against Apache Cassandra® clusters, not DSE
    /// </summary>
    PlainTextAuthProvider,

    /// <summary>
    /// SASL authentication using the PLAIN mechanism.
    /// </summary>
    DsePlainTextAuthProvider,
}