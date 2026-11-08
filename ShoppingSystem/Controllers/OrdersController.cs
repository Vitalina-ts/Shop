using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingSystem.Data;
using ShoppingSystem.Models;
using ShoppingSystem.ViewModels;
namespace ShoppingSystem.Controllers;
public class OrdersController(ShoppingContext db) : Controller
{
    public async Task<IActionResult> Index() => View(await db.Orders.AsNoTracking().Include(o => o.Customer).Include(o => o.Supermarket).OrderByDescending(o => o.OrderDate).ThenByDescending(o => o.Id).ToListAsync());
    private IQueryable<Order> FullOrder() => db.Orders.Include(o => o.Customer).Include(o => o.Supermarket).Include(o => o.OrderDetails).ThenInclude(d => d.Product);
    public async Task<IActionResult> Details(int id) { var order = await FullOrder().AsNoTracking().FirstOrDefaultAsync(o => o.Id == id); return order == null ? NotFound() : View(order); }
    public async Task<IActionResult> Create() { var vm = new OrderCreateViewModel(); await PopulateAsync(vm, true); return View(vm); }
    public async Task<IActionResult> Edit(int id)
    {
        var order = await db.Orders.AsNoTracking().Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.Id == id);
        if (order == null) return NotFound();
        var vm = new OrderCreateViewModel { Id = id, CustomerId = order.CustomerId, SupermarketId = order.SupermarketId, OrderDate = order.OrderDate,
            Items = order.OrderDetails.Select(d => new OrderItemViewModel { ProductId = d.ProductId, Quantity = d.Quantity, Selected = true }).ToList() };
        await PopulateAsync(vm, true); return View(vm);
    }
    [HttpPost]
    public async Task<IActionResult> Create(OrderCreateViewModel vm)
    {
        await ValidateAsync(vm);
        if (!ModelState.IsValid) { await PopulateAsync(vm, false); return View(vm); }
        var order = new Order { CustomerId = vm.CustomerId, SupermarketId = vm.SupermarketId, OrderDate = vm.OrderDate!.Value,
            OrderDetails = vm.Items.Where(i => i.Selected).Select(i => new OrderDetails { ProductId = i.ProductId, Quantity = i.Quantity }).ToList() };
        db.Orders.Add(order);
        try { await db.SaveChangesAsync(); }
        catch (DbUpdateException) { ModelState.AddModelError("", "Дані змінилися. Перевірте покупця, магазин і товари."); await PopulateAsync(vm, false); return View(vm); }
        return RedirectToAction(nameof(Details), new { id = order.Id });
    }
    [HttpPost]
    public async Task<IActionResult> Edit(int id, OrderCreateViewModel vm)
    {
        if (id != vm.Id) return NotFound();
        var order = await db.Orders.Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.Id == id);
        if (order == null) return NotFound();
        await ValidateAsync(vm);
        if (!ModelState.IsValid) { await PopulateAsync(vm, false); return View(vm); }
        order.CustomerId = vm.CustomerId; order.SupermarketId = vm.SupermarketId; order.OrderDate = vm.OrderDate!.Value;
        var selected = vm.Items.Where(i => i.Selected).ToDictionary(i => i.ProductId);
        foreach (var detail in order.OrderDetails.ToList())
        {
            if (selected.Remove(detail.ProductId, out var item)) detail.Quantity = item.Quantity;
            else db.OrderDetails.Remove(detail);
        }
        foreach (var item in selected.Values) order.OrderDetails.Add(new OrderDetails { ProductId = item.ProductId, Quantity = item.Quantity });
        // Один SaveChanges: всі зміни зберігаються транзакційно.
        try { await db.SaveChangesAsync(); }
        catch (DbUpdateException) { ModelState.AddModelError("", "Не вдалося зберегти зміни. Дані могли змінитися; оновіть сторінку."); await PopulateAsync(vm, false); return View(vm); }
        return RedirectToAction(nameof(Details), new { id });
    }
    public async Task<IActionResult> Delete(int id) { var order = await FullOrder().AsNoTracking().FirstOrDefaultAsync(o => o.Id == id); return order == null ? NotFound() : View(order); }
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var order = await db.Orders.FindAsync(id); if (order == null) return NotFound();
        db.Orders.Remove(order); await db.SaveChangesAsync(); return RedirectToAction(nameof(Index));
    }
    private async Task ValidateAsync(OrderCreateViewModel vm)
    {
        if (!await db.Customers.AnyAsync(c => c.Id == vm.CustomerId)) ModelState.AddModelError(nameof(vm.CustomerId), "Покупця не знайдено.");
        if (!await db.Supermarkets.AnyAsync(s => s.Id == vm.SupermarketId)) ModelState.AddModelError(nameof(vm.SupermarketId), "Супермаркет не знайдено.");
        if (vm.OrderDate is null || vm.OrderDate.Value == DateTime.MinValue) ModelState.AddModelError(nameof(vm.OrderDate), "Вкажіть коректну дату.");
        vm.Items ??= new();
        if (!vm.Items.Any(i => i.Selected)) ModelState.AddModelError("", "Оберіть принаймні один товар.");
        if (vm.Items.GroupBy(i => i.ProductId).Any(g => g.Count() > 1)) ModelState.AddModelError("", "Товари не повинні повторюватися.");
        var ids = vm.Items.Select(i => i.ProductId).Distinct().ToList();
        var products = await db.Products.AsNoTracking().Where(p => ids.Contains(p.Id)).ToDictionaryAsync(p => p.Id);
        for (var i = 0; i < vm.Items.Count; i++)
        {
            var item = vm.Items[i];
            if (!products.ContainsKey(item.ProductId)) ModelState.AddModelError($"Items[{i}].ProductId", "Товар не знайдено.");
            if (item.Selected && item.Quantity <= 0) ModelState.AddModelError($"Items[{i}].Quantity", "Кількість має бути більшою за нуль.");
        }
    }
    private async Task PopulateAsync(OrderCreateViewModel vm, bool complete)
    {
        vm.Customers = await db.Customers.AsNoTracking().OrderBy(c => c.LastName).Select(c => new SelectListItem(c.LastName + " " + c.FirstName, c.Id.ToString())).ToListAsync();
        vm.Supermarkets = await db.Supermarkets.AsNoTracking().OrderBy(s => s.Name).Select(s => new SelectListItem(s.Name, s.Id.ToString())).ToListAsync();
        var products = await db.Products.AsNoTracking().OrderBy(p => p.Name).ToListAsync();
        if (complete) foreach (var p in products) if (!vm.Items.Any(i => i.ProductId == p.Id)) vm.Items.Add(new OrderItemViewModel { ProductId = p.Id });
        foreach (var item in vm.Items)
        {
            var p = products.FirstOrDefault(p => p.Id == item.ProductId);
            item.Name = p?.Name ?? "Недоступний товар"; item.Price = p?.Price ?? 0;
        }
    }
}