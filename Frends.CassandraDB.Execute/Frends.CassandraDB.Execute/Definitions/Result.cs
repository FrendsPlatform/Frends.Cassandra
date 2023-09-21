using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Frends.CassandraDB.Execute.Definitions;

/// <summary>
/// Task's result.
/// </summary>
public class Result
{
    /// <summary>
    /// True if query ran successfully.
    /// </summary>
    /// <example>true</example>
    public bool Success { get; private set; }

    /// <summary>
    /// Query results. Only SELECT query contain values.
    /// </summary>
    /// <example>{\"0\":[{\"Key\":\"foo\",\"Value\":\"bar\"}],\"1\":[{\"Key\":\"foo2\",\"Value\":\"bar2\"}]}</example>
    public JToken QueryResults { get; private set; }

    /// <summary>
    /// Returns the server-side warnings for this query.
    /// This feature is only available for Cassandra 2.2 or above; with lower versions, this property always returns null.
    /// </summary>
    /// <example>Object{foo}</example>
    public List<string> Warnings { get; private set; }

    internal Result(bool success, JToken queryResults, List<string> warnings)
    {
        Success = success;
        QueryResults = queryResults;
        Warnings = warnings;
    }
}