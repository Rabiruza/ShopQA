using System.Data.Common; 
using ShopQA.Core.Extensions;

namespace ShopQA.Core.Extensions;

public static class DataReaderMapper
{
public static T? Map<T>(this DbDataReader reader, Func<DbDataReader, T> factory) 
    where T : class
    => reader.Read() ? factory(reader) : null;
    
    public static List<T> MapAll<T>(this DbDataReader reader, Func<DbDataReader, T> factory) 
        where T : class
    {
        var list = new List<T>();
        while (reader.Read())
            list.Add(factory(reader));
        return list;
    }
}