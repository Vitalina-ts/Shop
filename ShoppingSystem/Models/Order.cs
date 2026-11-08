using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
namespace ShoppingSystem.Models;
public class Order
{
    public int Id { get; set; }
    [Range(1, int.MaxValue), Display(Name = "Покупець")] public int CustomerId { get; set; }
    [Range(1, int.MaxValue), Display(Name = "Супермаркет")] public int SupermarketId { get; set; }
    [DataType(DataType.Date), Display(Name = "Дата замовлення")] public DateTime OrderDate { get; set; }
    [ValidateNever] public Customer Customer { get; set; } = null!;
    [ValidateNever] public Supermarket Supermarket { get; set; } = null!;
    public ICollection<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();
}