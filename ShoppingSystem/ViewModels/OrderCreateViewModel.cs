using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace ShoppingSystem.ViewModels;
public class OrderCreateViewModel
{
    public int Id { get; set; }
    [Range(1, int.MaxValue), Display(Name = "Покупець")] public int CustomerId { get; set; }
    [Range(1, int.MaxValue), Display(Name = "Супермаркет")] public int SupermarketId { get; set; }
    [Required, DataType(DataType.Date), Display(Name = "Дата замовлення")]
    public DateTime? OrderDate { get; set; } = DateTime.Today;
    public List<OrderItemViewModel> Items { get; set; } = new();
    [ValidateNever] public List<SelectListItem> Customers { get; set; } = new();
    [ValidateNever] public List<SelectListItem> Supermarkets { get; set; } = new();
}
public class OrderItemViewModel
{
    public int ProductId { get; set; }
    public bool Selected { get; set; }
    public int Quantity { get; set; } = 1;
    [ValidateNever] public string Name { get; set; } = "";
    [ValidateNever] public decimal Price { get; set; }
}