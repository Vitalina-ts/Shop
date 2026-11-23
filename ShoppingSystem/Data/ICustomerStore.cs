using ShoppingSystem.Models;

namespace ShoppingSystem.Data;

public interface ICustomerStore
{
    Task<List<Customer>> ListAsync(string? search, string? sort);
    Task<Customer?> GetAsync(int id);
    Task<Customer?> FindAsync(int id);
    Task<bool> HasOrdersAsync(int id);
    void Add(Customer customer);
    void Remove(Customer customer);
    Task<int> SaveChangesAsync();
}
