using Microsoft.EntityFrameworkCore;
using ShoppingSystem.Models;
namespace ShoppingSystem.Data;
public static class SampleData
{
    public static async Task InitializeAsync(ShoppingContext db)
    {
        // Заповнюємо лише цілком порожню БД: повторний запуск не відновлює видалені записи.
        await using var transaction = await db.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
        if (await db.Products.AnyAsync() || await db.Customers.AnyAsync() || await db.Supermarkets.AnyAsync() || await db.Orders.AnyAsync()) return;
        var products = new[] {
            new Product { Name = "Хліб", Price = 25 }, new Product { Name = "Молоко", Price = 40 },
            new Product { Name = "Яйця (10 шт.)", Price = 60 }, new Product { Name = "Сир", Price = 95.50m },
            new Product { Name = "Яблука", Price = 32 }, new Product { Name = "Рис", Price = 58 },
            new Product { Name = "Чай", Price = 78.90m }, new Product { Name = "Вода", Price = 22 } };
        var customers = new[] {
            new Customer { FirstName = "Anna", LastName = "Коваленко", Address = "Київ, Лісова 1", Discount = 5 },
            new Customer { FirstName = "Іван", LastName = "Бондар", Address = "Львів, Шевченка 12", Discount = 0 },
            new Customer { FirstName = "Hanna", LastName = "Мельник", Address = "Київ, Миру 5", Discount = 10 },
            new Customer { FirstName = "Олена", LastName = "Савчук", Address = "Одеса, Морська 8", Discount = 3 },
            new Customer { FirstName = "Андрій", LastName = "Ткаченко", Address = "Дніпро, Сонячна 2", Discount = 7 },
            new Customer { FirstName = "Марія", LastName = "Шевченко", Address = "Харків, Садова 9", Discount = 2 } };
        var markets = Enumerable.Range(1, 12).Select(i => new Supermarket { Name = $"Маркет {i:00}", Address = $"Київ, Торгова {i}" }).ToArray();
        db.AddRange(products); db.AddRange(customers); db.AddRange(markets);
        for (var i = 0; i < 5; i++)
            db.Orders.Add(new Order { Customer = customers[i], Supermarket = markets[i], OrderDate = new DateTime(2026, 11, 1).AddDays(i),
                OrderDetails = new List<OrderDetails> {
                    new() { Product = products[0], Quantity = 2 }, new() { Product = products[1], Quantity = 3 },
                    new() { Product = products[2 + i], Quantity = 1 } } });
        await db.SaveChangesAsync();
        await transaction.CommitAsync();
    }
}