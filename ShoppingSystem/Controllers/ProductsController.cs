using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingSystem.Data;
using ShoppingSystem.Models;
using ShoppingSystem.ViewModels;
namespace ShoppingSystem.Controllers;
public class ProductsController(ShoppingContext db) : Controller
{
    public async Task<IActionResult> Index() => View(await db.Products.AsNoTracking().OrderBy(x => x.Id).ToListAsync());
    public async Task<IActionResult> Details(int id) { var item = await db.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id); return item == null ? NotFound() : View(item); }
    public IActionResult Create() => View(new Product());
    [HttpPost]
    public async Task<IActionResult> Create([Bind("Name,Price")] Product input)
    {
        if (!ModelState.IsValid) return View(input);
        db.Products.Add(input); await db.SaveChangesAsync(); return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Edit(int id) { var item = await db.Products.FindAsync(id); return item == null ? NotFound() : View(item); }
    [HttpPost]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price")] Product input)
    {
        if (id != input.Id) return NotFound();
        var item = await db.Products.FindAsync(id); if (item == null) return NotFound();
        if (!ModelState.IsValid) return View(input);
        item.Name = input.Name;
        item.Price = input.Price;
        try { await db.SaveChangesAsync(); } catch (DbUpdateConcurrencyException) { return NotFound(); }
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Delete(int id) { var item = await db.Products.FindAsync(id); return item == null ? NotFound() : View(item); }
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await db.Products.FindAsync(id); if (item == null) return NotFound();
        if (await db.OrderDetails.AnyAsync(x => x.ProductId == id))
        { ModelState.AddModelError("", "Запис використовується у замовленнях. Спочатку змініть відповідні замовлення."); return View(item); }
        db.Products.Remove(item);
        try { await db.SaveChangesAsync(); }
        catch (DbUpdateException) { ModelState.AddModelError("", "Не вдалося видалити пов’язаний запис. Перевірте замовлення."); return View(item); }
        return RedirectToAction(nameof(Index));
    }
}