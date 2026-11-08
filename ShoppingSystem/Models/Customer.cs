using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ShoppingSystem.Models;
public class Customer
{
    public int Id { get; set; }
    [Required, StringLength(50), Display(Name = "Ім’я")]
    public string FirstName { get; set; } = "";
    [Required, StringLength(50), Display(Name = "Прізвище")]
    public string LastName { get; set; } = "";
    [StringLength(50), Display(Name = "Адреса")]
    public string? Address { get; set; }
    [Range(0, 100), Display(Name = "Знижка, %")]
    public int Discount { get; set; }
    [NotMapped] public string FullName => $"{LastName} {FirstName}";
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}