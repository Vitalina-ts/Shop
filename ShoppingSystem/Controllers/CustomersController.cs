using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingSystem.Data;
using ShoppingSystem.Models;
using ShoppingSystem.ViewModels;
namespace ShoppingSystem.Controllers;
public class CustomersController(ICustomerStore customers) : Controller
{
    public async Task<IActionResult> Index(string? search, string? sort)
    {
        if (!string.IsNullOrWhiteSpace(search)) search = search.Trim();
        return View(new CustomerListViewModel { Customers = await customers.ListAsync(search, sort), Search = search, Sort = sort });
    }
    public async Task<IActionResult> Details(int id) { var item = await customers.GetAsync(id); return item == null ? NotFound() : View(item); }
    public IActionResult Create() => View(new Customer());
    [HttpPost]
    public async Task<IActionResult> Create([Bind("FirstName,LastName,Address,Discount")] Customer input)
    {
        if (!ModelState.IsValid) return View(input);
        customers.Add(input); await customers.SaveChangesAsync(); return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Edit(int id) { var item = await customers.FindAsync(id); return item == null ? NotFound() : View(item); }
    [HttpPost]
    public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Address,Discount")] Customer input)
    {
        if (id != input.Id) return NotFound();
        var item = await customers.FindAsync(id); if (item == null) return NotFound();
        if (!ModelState.IsValid) return View(input);
        item.FirstName = input.FirstName;
        item.LastName = input.LastName;
        item.Address = input.Address;
        item.Discount = input.Discount;
        try { await customers.SaveChangesAsync(); } catch (DbUpdateConcurrencyException) { return NotFound(); }
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Delete(int id) { var item = await customers.FindAsync(id); return item == null ? NotFound() : View(item); }
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await customers.FindAsync(id); if (item == null) return NotFound();
        if (await customers.HasOrdersAsync(id))
        { ModelState.AddModelError("", "Запис використовується у замовленнях. Спочатку змініть відповідні замовлення."); return View(item); }
        customers.Remove(item);
        try { await customers.SaveChangesAsync(); }
        catch (DbUpdateException) { ModelState.AddModelError("", "Не вдалося видалити пов’язаний запис. Перевірте замовлення."); return View(item); }
        return RedirectToAction(nameof(Index));
    }
}
