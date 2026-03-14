using Microsoft.Data.Sqlite;
using ShopQA.Core.Config;
using ShopQA.Core.Models;
using ShopQA.Core.Extensions;

namespace ShopQA.Core.Database;

/// <summary>
/// Provides direct database access for test assertions and data seeding.
/// Validates that UI/API actions correctly persist data to SQLite.
/// </summary>
public class DatabaseHelper : IDisposable
{
    private readonly SqliteConnection _connection;

    public DatabaseHelper(string? connectionString = null)
    {
        var cs = connectionString ?? TestConfiguration.GetConnectionString();
        _connection = new SqliteConnection(cs);
        try
        {
            _connection.Open();
            using var pragmaCmd = _connection.CreateCommand();
            pragmaCmd.CommandText = "PRAGMA foreign_keys = ON;";
            pragmaCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to connect to database with connection string: ", ex);
        }
    }

    // ── Schema ──────────────────────────────────────────────────────────────

    public void InitializeSchema()
    {
        ExecuteNonQuery(@"
            CREATE TABLE IF NOT EXISTS Users (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Email TEXT NOT NULL UNIQUE,
                Password TEXT NOT NULL,
                FirstName TEXT,
                LastName TEXT,
                Role TEXT DEFAULT 'user',
                CreatedAt TEXT DEFAULT (datetime('now'))
            );

            CREATE TABLE IF NOT EXISTS Products (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Category TEXT,
                Price REAL NOT NULL,
                Stock INTEGER DEFAULT 0,
                Description TEXT
            );

            CREATE TABLE IF NOT EXISTS Orders (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                UserId INTEGER NOT NULL,
                Status TEXT DEFAULT 'Pending',
                CreatedAt TEXT DEFAULT (datetime('now')),
                FOREIGN KEY (UserId) REFERENCES Users(Id)
            );

            CREATE TABLE IF NOT EXISTS OrderItems (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                OrderId INTEGER NOT NULL,
                ProductId INTEGER NOT NULL,
                ProductName TEXT NOT NULL,
                UnitPrice REAL NOT NULL,
                Quantity INTEGER NOT NULL,
                FOREIGN KEY (OrderId) REFERENCES Orders(Id)
            );");
    }

    /// <summary>
    /// Returns a safe, credential-free UserModel.
    /// Use this everywhere: tests, assertions, UI state, logs.
    /// </summary>
    public UserModel? GetUserByEmail(string email)
    {
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = "SELECT * FROM Users WHERE Email = @Email";
        cmd.Parameters.AddWithValue("@Email", email);

        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;

        return new UserModel
        {
            Id = reader.GetRequiredInt32("Id"),
            Email = reader.GetRequiredString("Email"),
            FirstName = reader.GetStringOrNull("FirstName"),
            LastName = reader.GetStringOrNull("LastName"),
            Role = reader.GetStringOrNull("Role", "user"),
            CreatedAt = reader.GetNullableString("CreatedAt")
        };
    }

    /// <summary>
    /// Used ONLY by LoginViewModel for credential verification.
    /// Internal — not accessible from test projects directly.
    /// Returns null if email not found.
    /// </summary>
    internal UserAuthRecord? GetAuthRecord(string email)
    {
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = "SELECT Id, Email, Password FROM Users WHERE Email = @Email";
        cmd.Parameters.AddWithValue("@Email", email);

        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;

        return new UserAuthRecord(
            reader.GetRequiredInt32("Id"),
            reader.GetRequiredString("Email"),
            reader.GetRequiredString("Password")
        );
    }

    public bool UserExists(string email) => GetUserByEmail(email) is not null;

    // ── Products ─────────────────────────────────────────────────────────────

    public int GetProductCount() =>
        (int)(long)ExecuteScalar("SELECT COUNT(*) FROM Products")!;

    public ProductModel? GetProductByName(string name)
    {
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = "SELECT * FROM Products WHERE Name = @Name ORDER BY Id LIMIT 1";
        cmd.Parameters.AddWithValue("@Name", name);

        using var reader = cmd.ExecuteReader();
        return reader.Map(r => new ProductModel(
            r.GetRequiredInt32("Id"),
            r.GetRequiredString("Name"),
            r.GetStringOrNull("Category"),
            r.GetDecimalOrNull("Price"),
            r.GetInt32OrNull("Stock"),
            r.GetStringOrNull("Description")
        ));
    }

    // ── Orders 
    
    public int GetOrderCountForUser(int userId)
    {
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = "SELECT COUNT(*) FROM Orders WHERE UserId = @UserId";
        cmd.Parameters.AddWithValue("@UserId", userId);
        return (int)(long)cmd.ExecuteScalar()!;
    }

    public string? GetLatestOrderStatus(int userId)
    {
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = "SELECT Status FROM Orders WHERE UserId = @UserId ORDER BY CreatedAt DESC LIMIT 1";
        cmd.Parameters.AddWithValue("@UserId", userId);
        return cmd.ExecuteScalar()?.ToString();
    }

    public List<ProductModel> GetAllProducts()
    {
        var products = new List<ProductModel>();
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = "SELECT * FROM Products ORDER BY Name";
        
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            products.Add(new ProductModel(
            reader.GetRequiredInt32("Id"),
            reader.GetRequiredString("Name"),
            reader.GetStringOrNull("Category"),
            reader.GetDecimalOrNull("Price"),
            reader.GetInt32OrNull("Stock"),
            reader.GetStringOrNull("Description")
        ));
        }
        return products;
    }

    public int InsertOrder(OrderModel order)
    {
        using var transaction = _connection.BeginTransaction();
        try
        {
            using var cmd = _connection.CreateCommand();
            cmd.Transaction = transaction;  
            cmd.CommandText = @"INSERT INTO Orders (UserId, Status, CreatedAt)
                            VALUES (@UserId, @Status, @CreatedAt);
                            SELECT last_insert_rowid();";
            cmd.Parameters.AddWithValue("@UserId", order.UserId);
            cmd.Parameters.AddWithValue("@Status", order.Status);
            cmd.Parameters.AddWithValue("@CreatedAt", order.CreatedAt.ToString("o"));
            var orderId = (int)(long)cmd.ExecuteScalar()!;

            foreach (var item in order.Items)
            {
                using var itemCmd = _connection.CreateCommand();
                itemCmd.Transaction = transaction;  
                itemCmd.CommandText = @"INSERT INTO OrderItems (OrderId, ProductId, ProductName, UnitPrice, Quantity)
                                    VALUES (@OrderId, @ProductId, @ProductName, @UnitPrice, @Quantity)";
                itemCmd.Parameters.AddWithValue("@OrderId", orderId);
                itemCmd.Parameters.AddWithValue("@ProductId", item.ProductId);
                itemCmd.Parameters.AddWithValue("@ProductName", item.ProductName);
                itemCmd.Parameters.AddWithValue("@UnitPrice", (double)item.UnitPrice);
                itemCmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                itemCmd.ExecuteNonQuery();
            }
            transaction.Commit();
            return orderId;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private void ExecuteNonQuery(string sql)
    {
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = sql;
        cmd.ExecuteNonQuery();
    }

    private object? ExecuteScalar(string sql)
    {
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = sql;
        return cmd.ExecuteScalar();
    }

    public void Reset()
    {
        ExecuteNonQuery("DELETE FROM OrderItems; DELETE FROM Orders; DELETE FROM Products; DELETE FROM Users;");
    }

    private bool _disposed;

    public void Dispose()
    {
        if (_disposed) return;
        _connection?.Dispose();
        _disposed = true;
        GC.SuppressFinalize(this);
    }
}
