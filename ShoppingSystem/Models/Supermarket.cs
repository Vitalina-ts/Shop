using System.ComponentModel.DataAnnotations;
namespace ShoppingSystem.Models;
public class Supermarket
{
    public int Id { get; set; }
    [Required, StringLength(50), Display(Name = "Назва")]
    public string Name { get; set; } = "";
    [StringLength(100), Display(Name = "Адреса")]
    public string? Address { get; set; }
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}