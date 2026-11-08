using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingSystem.Data;
using ShoppingSystem.Models;
using ShoppingSystem.ViewModels;
namespace ShoppingSystem.Controllers;
public class SupermarketsController(ShoppingContext db) : Controller
{
    public async Task<IActionResult> Index(int page = 1)
    {
        const int size = 5;
        var count = await db.Supermarkets.CountAsync();
        var pages = Math.Max(1, (int)Math.Ceiling(count / (double)size));
        page = Math.Clamp(page, 1, pages);
        return View(new SupermarketListViewModel { Page = page, TotalPages = pages, TotalCount = count,
            Supermarkets = await db.Supermarkets.AsNoTracking().OrderBy(s => s.Name).ThenBy(s => s.Id).Skip((page - 1) * size).Take(size).ToListAsync() });
    }
    public async Task<IActionResult> Details(int id) { var item = await db.Supermarkets.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id); return item == null ? NotFound() : View(item); }
    public IActionResult Create() => View(new Supermarket());
    [HttpPost]
    public async Task<IActionResult> Create([Bind("Name,Address")] Supermarket input)
    {
        if (!ModelState.IsValid) return View(input);
        db.Supermarkets.Add(input); await db.SaveChangesAsync(); return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Edit(int id) { var item = await db.Supermarkets.FindAsync(id); return item == null ? NotFound() : View(item); }
    [HttpPost]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address")] Supermarket input)
    {
        if (id != input.Id) return NotFound();
        var item = await db.Supermarkets.FindAsync(id); if (item == null) return NotFound();
        if (!ModelState.IsValid) return View(input);
        item.Name = input.Name;
        item.Address = input.Address;
        try { await db.SaveChangesAsync(); } catch (DbUpdateConcurrencyException) { return NotFound(); }
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Delete(int id) { var item = await db.Supermarkets.FindAsync(id); return item == null ? NotFound() : View(item); }
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await db.Supermarkets.FindAsync(id); if (item == null) return NotFound();
        if (await db.Orders.AnyAsync(x => x.SupermarketId == id))
        { ModelState.AddModelError("", "Запис використовується у замовленнях. Спочатку змініть відповідні замовлення."); return View(item); }
        db.Supermarkets.Remove(item);
        try { await db.SaveChangesAsync(); }
        catch (DbUpdateException) { ModelState.AddModelError("", "Не вдалося видалити пов’язаний запис. Перевірте замовлення."); return View(item); }
        return RedirectToAction(nameof(Index));
    }
}