using System.ComponentModel.DataAnnotations;
namespace ShoppingSystem.Models;
public class Product
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Вкажіть назву."), StringLength(50), Display(Name = "Назва")]
    public string Name { get; set; } = "";
    [Range(typeof(decimal), "0", "9999999999999999.99"), Display(Name = "Ціна, грн")]
    public decimal Price { get; set; }
    public ICollection<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();
}