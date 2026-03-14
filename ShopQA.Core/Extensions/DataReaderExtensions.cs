using System.Data.Common;

namespace ShopQA.Core.Extensions;

/// <summary>
/// Extension methods for DbDataReader to simplify null-safe, name-based column access.
/// No caching - simple and reliable for test helpers.
/// </summary>
public static class DataReaderExtensions
{
    /// <summary>
    /// Gets a string value by column name, returning defaultValue if NULL.
    /// </summary>
    public static string GetStringOrNull(this DbDataReader reader, string columnName, string defaultValue = "")
    {
        var ordinal = reader.GetOrdinal(columnName);
        return reader.IsDBNull(ordinal) ? defaultValue : reader.GetString(ordinal);
    }

    /// <summary>
    /// Gets an int value by column name, returning defaultValue if NULL.
    /// </summary>
    public static int GetInt32OrNull(this DbDataReader reader, string columnName, int defaultValue = 0)
    {
        var ordinal = reader.GetOrdinal(columnName);
        return reader.IsDBNull(ordinal) ? defaultValue : reader.GetInt32(ordinal);
    }

    /// <summary>
    /// Gets a long value by column name, returning defaultValue if NULL.
    /// </summary>
    public static long GetInt64OrNull(this DbDataReader reader, string columnName, long defaultValue = 0)
    {
        var ordinal = reader.GetOrdinal(columnName);
        return reader.IsDBNull(ordinal) ? defaultValue : reader.GetInt64(ordinal);
    }

    /// <summary>
    /// Gets a double value by column name, returning defaultValue if NULL.
    /// </summary>
    public static double GetDoubleOrNull(this DbDataReader reader, string columnName, double defaultValue = 0.0)
    {
        var ordinal = reader.GetOrdinal(columnName);
        return reader.IsDBNull(ordinal) ? defaultValue : reader.GetDouble(ordinal);
    }

    /// <summary>
    /// Gets a decimal value by column name. 
    /// Note: SQLite stores decimals as REAL (double), so we convert safely.
    /// Returns defaultValue if NULL.
    /// </summary>
    public static decimal GetDecimalOrNull(this DbDataReader reader, string columnName, decimal defaultValue = default)
    {
        var ordinal = reader.GetOrdinal(columnName);
        return reader.IsDBNull(ordinal) ? defaultValue : (decimal)reader.GetDouble(ordinal);
    }

    /// <summary>
    /// Gets a nullable int value by column name. Returns null if database value is NULL.
    /// </summary>
    public static int? GetNullableInt32(this DbDataReader reader, string columnName)
    {
        var ordinal = reader.GetOrdinal(columnName);
        return reader.IsDBNull(ordinal) ? (int?)null : reader.GetInt32(ordinal);
    }

    /// <summary>
    /// Gets a nullable string value by column name. Returns null if database value is NULL.
    /// </summary>
    public static string? GetNullableString(this DbDataReader reader, string columnName)
    {
        var ordinal = reader.GetOrdinal(columnName);
        return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
    }

    /// <summary>
    /// Gets a required (non-null) int value by column name. Throws if NULL.
    /// Use only for columns with NOT NULL constraint.
    /// </summary>
    public static int GetRequiredInt32(this DbDataReader reader, string columnName)
    {
        var ordinal = reader.GetOrdinal(columnName);
        if (reader.IsDBNull(ordinal))
            throw new InvalidOperationException($"Column '{columnName}' contains NULL but is required.");
        return reader.GetInt32(ordinal);
    }

    /// <summary>
    /// Gets a required (non-null) string value by column name. Throws if NULL.
    /// Use only for columns with NOT NULL constraint.
    /// </summary>
    public static string GetRequiredString(this DbDataReader reader, string columnName)
    {
        var ordinal = reader.GetOrdinal(columnName);
        if (reader.IsDBNull(ordinal))
            throw new InvalidOperationException($"Column '{columnName}' contains NULL but is required.");
        return reader.GetString(ordinal);
    }
}