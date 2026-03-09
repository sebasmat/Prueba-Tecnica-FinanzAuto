namespace Back.Core.Entities;

public class Product
{
    public int ProductID { get; set; } // PK
    public string ProductName { get; set; } = string.Empty;
    public int? SupplierID { get; set; }
    public int? CategoryID { get; set; } // FK
    public string? QuantityPerUnit { get; set; }
    public decimal UnitPrice { get; set; } // En C#, los precios siempre son decimal
    public short UnitsInStock { get; set; }
    public short UnitsOnOrder { get; set; }
    public short ReorderLevel { get; set; }
    public bool Discontinued { get; set; }
    public Supplier? Supplier { get; set; }

    // Propiedad de navegación (Relación con Category)
    public virtual Category? Category { get; set; }
}