namespace Back.Core.Entities;

public class Category
{
    public int CategoryID { get; set; } // PK
    public string CategoryName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public byte[]? Picture { get; set; } // Aquí guardaremos la imagen en bytes
    
    // Relación inversa: Una categoría tiene muchos productos
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}