using Microsoft.EntityFrameworkCore;
using ShoppingSystem.Models;

namespace ShoppingSystem.Data;

public class CustomerStore(ShoppingContext db) : ICustomerStore
{
    public async Task<List<Customer>> ListAsync(string? search, string? sort)
    {
        IQueryable<Customer> query = db.Customers.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(c => c.FirstName.Contains(search) || c.LastName.Contains(search));
        query = sort switch {
            "name_desc" => query.OrderByDescending(c => c.LastName).ThenBy(c => c.Id),
            "address" => query.OrderBy(c => c.Address).ThenBy(c => c.Id),
            "address_desc" => query.OrderByDescending(c => c.Address).ThenBy(c => c.Id),
            _ => query.OrderBy(c => c.LastName).ThenBy(c => c.Id) };
        return await query.ToListAsync();
    }

    public Task<Customer?> GetAsync(int id) =>
        db.Customers.AsNoTracking().FirstOrDefaultAsync(customer => customer.Id == id);

    public Task<Customer?> FindAsync(int id) => db.Customers.FindAsync(id).AsTask();

    public Task<bool> HasOrdersAsync(int id) => db.Orders.AnyAsync(order => order.CustomerId == id);

    public void Add(Customer customer) => db.Customers.Add(customer);

    public void Remove(Customer customer) => db.Customers.Remove(customer);

    public Task<int> SaveChangesAsync() => db.SaveChangesAsync();
}
