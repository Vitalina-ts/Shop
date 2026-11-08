using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingSystem.Data;
using ShoppingSystem.Models;
using ShoppingSystem.ViewModels;
namespace ShoppingSystem.Controllers;
public class CustomersController(ShoppingContext db) : Controller
{
    public async Task<IActionResult> Index(string? search, string? sort)
    {
        IQueryable<Customer> query = db.Customers.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search)) { search = search.Trim(); query = query.Where(c => c.FirstName.Contains(search) || c.LastName.Contains(search)); }
        query = sort switch {
            "name_desc" => query.OrderByDescending(c => c.LastName).ThenBy(c => c.Id),
            "address" => query.OrderBy(c => c.Address).ThenBy(c => c.Id),
            "address_desc" => query.OrderByDescending(c => c.Address).ThenBy(c => c.Id),
            _ => query.OrderBy(c => c.LastName).ThenBy(c => c.Id) };
        return View(new CustomerListViewModel { Customers = await query.ToListAsync(), Search = search, Sort = sort });
    }
    public async Task<IActionResult> Details(int id) { var item = await db.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id); return item == null ? NotFound() : View(item); }
    public IActionResult Create() => View(new Customer());
    [HttpPost]
    public async Task<IActionResult> Create([Bind("FirstName,LastName,Address,Discount")] Customer input)
    {
        if (!ModelState.IsValid) return View(input);
        db.Customers.Add(input); await db.SaveChangesAsync(); return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Edit(int id) { var item = await db.Customers.FindAsync(id); return item == null ? NotFound() : View(item); }
    [HttpPost]
    public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Address,Discount")] Customer input)
    {
        if (id != input.Id) return NotFound();
        var item = await db.Customers.FindAsync(id); if (item == null) return NotFound();
        if (!ModelState.IsValid) return View(input);
        item.FirstName = input.FirstName;
        item.LastName = input.LastName;
        item.Address = input.Address;
        item.Discount = input.Discount;
        try { await db.SaveChangesAsync(); } catch (DbUpdateConcurrencyException) { return NotFound(); }
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Delete(int id) { var item = await db.Customers.FindAsync(id); return item == null ? NotFound() : View(item); }
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await db.Customers.FindAsync(id); if (item == null) return NotFound();
        if (await db.Orders.AnyAsync(x => x.CustomerId == id))
        { ModelState.AddModelError("", "Запис використовується у замовленнях. Спочатку змініть відповідні замовлення."); return View(item); }
        db.Customers.Remove(item);
        try { await db.SaveChangesAsync(); }
        catch (DbUpdateException) { ModelState.AddModelError("", "Не вдалося видалити пов’язаний запис. Перевірте замовлення."); return View(item); }
        return RedirectToAction(nameof(Index));
    }
}