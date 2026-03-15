namespace ShopQA.Core.Models;

using System.Net;

/// <summary>
/// Safe user representation used everywhere in the app and tests.
/// Never contains credentials — safe to log, assert, and pass around.
/// </summary>
public class UserModel
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Role { get; set; } = "user";
    public string FullName => $"{FirstName} {LastName}".Trim();
    public string? CreatedAt { get; set; } 
}

/// <summary>
/// Used ONLY inside LoginViewModel for credential verification.
/// Never returned from public methods, never passed to tests directly.
/// Intentionally a separate type so the compiler prevents accidental exposure.
/// </summary>
internal sealed record UserAuthRecord(
    int Id,
    string Email = "",
    string PasswordHash = ""
)
{
    public UserModel ToSafeModel(string firstName, string lastName, string role) => new()
    {
        Id = this.Id,
        Email = this.Email,
        FirstName = firstName,
        LastName = lastName,
        Role = role
    };
}

public record ProductModel(
    int Id,
    string Name,
    string Category = "",
    decimal Price = default,
    int Stock = 0,
    string Description = ""
);

public class OrderModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public List<OrderItemModel> Items { get; set; } = new();
    public decimal TotalPrice => Items.Sum(i => i.Subtotal);
    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public record OrderItemModel(
    int Id,
    int ProductId,
    string ProductName = "",
    decimal UnitPrice = default,
    int Quantity = 0
)
{
    public decimal Subtotal => UnitPrice * Quantity;
}

public class ApiResponseModel<T>
{
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    
    public bool IsSuccess => StatusCode is >= HttpStatusCode.OK and < HttpStatusCode.MultipleChoices;
    
    // Implicit conversion for backward compatibility
    public int ResponseCode 
    { 
        get => (int)StatusCode; 
        set => StatusCode = (HttpStatusCode)value; 
    }
}
