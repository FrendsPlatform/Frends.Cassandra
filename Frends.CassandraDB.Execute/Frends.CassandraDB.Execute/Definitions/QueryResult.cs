namespace Frends.CassandraDB.Execute.Definitions;

/// <summary>
/// Single query result.
/// </summary>
public class QueryResult
{
    /// <summary>
    /// Key.
    /// </summary>
    /// <example>Foo</example>
    public string Key { get; set; }

    /// <summary>
    /// Value.
    /// </summary>
    /// <example>Bar</example>
    public object Value { get; set; }
}