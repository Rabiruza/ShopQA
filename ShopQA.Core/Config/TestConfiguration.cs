namespace ShopQA.Core.Config;  

public static class TestConfiguration
{
    public static string GetConnectionString() 
        => "Data Source=:memory:;Cache=Shared";
}