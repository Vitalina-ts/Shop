using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
namespace ShoppingSystem.Models;
public class OrderDetails
{
    public int Id { get; set; }
    [Range(1, int.MaxValue), Display(Name = "Замовлення")] public int OrderId { get; set; }
    [Range(1, int.MaxValue), Display(Name = "Товар")] public int ProductId { get; set; }
    [Range(1, int.MaxValue), Display(Name = "Кількість")] public int Quantity { get; set; }
    [ValidateNever] public Order Order { get; set; } = null!;
    [ValidateNever] public Product Product { get; set; } = null!;
}