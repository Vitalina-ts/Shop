using ShoppingSystem.Models;
namespace ShoppingSystem.ViewModels;
public class CustomerListViewModel
{
    public List<Customer> Customers { get; set; } = new();
    public string? Search { get; set; }
    public string? Sort { get; set; }
}
public class SupermarketListViewModel
{
    public List<Supermarket> Supermarkets { get; set; } = new();
    public int Page { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
}