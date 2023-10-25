using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json.Linq;

namespace Frends.CassandraDB.ExecuteQuery.Definitions;

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
    /// <example>JToken</example>
    public JToken QueryResults { get; private set; }

    /// <summary>
    /// Returns the server-side warnings for this query.
    /// This feature is only available for Cassandra 2.2 or above; with lower versions, this property always returns null.
    /// </summary>
    /// <example>List { "warning1", "warning2" }</example>
    public List<string> Warnings { get; private set; }

    internal Result(bool success, JToken queryResults, List<string> warnings)
    {
        Success = success;
        QueryResults = queryResults;
        Warnings = warnings;
    }
}