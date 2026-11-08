using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingSystem.Data;
using ShoppingSystem.Models;
namespace ShoppingSystem.Controllers;
public class OrderDetailsController(ShoppingContext db) : Controller
{
    private IQueryable<OrderDetails> Query() => db.OrderDetails.Include(d => d.Product).Include(d => d.Order).ThenInclude(o => o.Customer);
    public async Task<IActionResult> Index(int? orderId) => View(await Query().AsNoTracking().Where(d => orderId == null || d.OrderId == orderId).OrderBy(d => d.OrderId).ThenBy(d => d.Id).ToListAsync());
    public async Task<IActionResult> Details(int id) { var item = await Query().AsNoTracking().FirstOrDefaultAsync(d => d.Id == id); return item == null ? NotFound() : View(item); }
    public async Task<IActionResult> Create(int? orderId) { var item = new OrderDetails { OrderId = orderId ?? 0, Quantity = 1 }; await PopulateAsync(); return View(item); }
    [HttpPost]
    public async Task<IActionResult> Create([Bind("OrderId,ProductId,Quantity")] OrderDetails input)
    {
        await ValidateAsync(input);
        if (!ModelState.IsValid) { await PopulateAsync(); return View(input); }
        db.OrderDetails.Add(input);
        try { await db.SaveChangesAsync(); }
        catch (DbUpdateException) { ModelState.AddModelError("", "Не вдалося додати позицію: перевірте зв’язки та повторення товару."); await PopulateAsync(); return View(input); }
        return RedirectToAction("Details", "Orders", new { id = input.OrderId });
    }
    public async Task<IActionResult> Edit(int id) { var item = await db.OrderDetails.FindAsync(id); if (item == null) return NotFound(); await PopulateAsync(); return View(item); }
    [HttpPost]
    public async Task<IActionResult> Edit(int id, [Bind("Id,OrderId,ProductId,Quantity")] OrderDetails input)
    {
        if (id != input.Id) return NotFound();
        var item = await db.OrderDetails.FindAsync(id); if (item == null) return NotFound();
        await ValidateAsync(input);
        if (!ModelState.IsValid) { await PopulateAsync(); return View(input); }
        item.OrderId = input.OrderId; item.ProductId = input.ProductId; item.Quantity = input.Quantity;
        try { await db.SaveChangesAsync(); }
        catch (DbUpdateException) { ModelState.AddModelError("", "Не вдалося змінити позицію. Перевірте зв’язки та повторення товару."); await PopulateAsync(); return View(input); }
        return RedirectToAction("Details", "Orders", new { id = input.OrderId });
    }
    public async Task<IActionResult> Delete(int id) { var item = await Query().AsNoTracking().FirstOrDefaultAsync(d => d.Id == id); return item == null ? NotFound() : View(item); }
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await db.OrderDetails.FindAsync(id); if (item == null) return NotFound();
        var orderId = item.OrderId; db.OrderDetails.Remove(item); await db.SaveChangesAsync();
        return RedirectToAction("Details", "Orders", new { id = orderId });
    }
    private async Task ValidateAsync(OrderDetails item)
    {
        if (!await db.Orders.AnyAsync(o => o.Id == item.OrderId)) ModelState.AddModelError("OrderId", "Замовлення не знайдено.");
        if (!await db.Products.AnyAsync(p => p.Id == item.ProductId)) ModelState.AddModelError("ProductId", "Товар не знайдено.");
        if (await db.OrderDetails.AnyAsync(d => d.OrderId == item.OrderId && d.ProductId == item.ProductId && d.Id != item.Id)) ModelState.AddModelError("ProductId", "Товар уже є в цьому замовленні.");
    }
    private async Task PopulateAsync()
    {
        ViewBag.Orders = await db.Orders.AsNoTracking().OrderByDescending(o => o.Id).Select(o => new SelectListItem("№" + o.Id + " — " + o.Customer.LastName + " " + o.Customer.FirstName, o.Id.ToString())).ToListAsync();
        ViewBag.Products = await db.Products.AsNoTracking().OrderBy(p => p.Name).Select(p => new SelectListItem(p.Name + " — " + p.Price + " грн", p.Id.ToString())).ToListAsync();
    }
}