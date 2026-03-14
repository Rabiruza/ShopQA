using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ShopQA.Core.Database;
using ShopQA.Core.Models;

namespace ShopQA.App.ViewModels;

public class LoginResult
{
    public bool IsSuccess { get; init; }
    public string ErrorMessage { get; init; } = string.Empty;
    public UserModel? User { get; init; }

    public static LoginResult Success(UserModel user) => new() { IsSuccess = true, User = user };
    public static LoginResult Failure(string message) => new() { IsSuccess = false, ErrorMessage = message };
}

public class LoginViewModel : IDisposable
{
    private readonly DatabaseHelper _db;
    private readonly bool _ownsDb;

    public LoginViewModel(DatabaseHelper db, bool ownsDb = false)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db), "DatabaseHelper instance is required.");
        _ownsDb = ownsDb;
    }

    // Async version for production (keep sync version for demo if preferred)
    public async Task<LoginResult> LoginAsync(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email))
            return LoginResult.Failure("Email is required.");

        if (string.IsNullOrWhiteSpace(password))
            return LoginResult.Failure("Password is required.");

        if (!IsValidEmail(email))
            return LoginResult.Failure("Please enter a valid email address.");

        // Run DB call off UI thread
        var authRecord = await Task.Run(() => _db.GetAuthRecord(email));

        if (authRecord is null)
            return LoginResult.Failure("No account found with this email.");

        // Production: Use BCrypt or fixed-time comparison
        #if DEBUG
        // Demo mode: plain comparison (NOT for production)
        if (authRecord.PasswordHash != password)
            return LoginResult.Failure("Incorrect password. Please try again.");
        #else
        // Production mode: secure verification
        if (!VerifyPassword(password, authRecord.PasswordHash))
            return LoginResult.Failure("Incorrect password. Please try again.");
        #endif

        var user = await Task.Run(() => _db.GetUserByEmail(email));
        if (user is null)
            return LoginResult.Failure("Failed to load user profile. Please try again.");

        return LoginResult.Success(user);
    }

    // Secure password verification helper
    private static bool VerifyPassword(string password, string storedHash)
    {
        return CryptographicOperations.FixedTimeEquals(
        Encoding.UTF8.GetBytes(password),
        Encoding.UTF8.GetBytes(storedHash));
    }

    private static bool IsValidEmail(string email)
    {
        try 
        { 
            var addr = new MailAddress(email); 
            return addr.Address == email; 
        }
        catch 
        { 
            return false; 
        }
    }

    public void Dispose()
    {
        if (_ownsDb) 
        {
            _db?.Dispose();
        }
    }
}