namespace Back.Application.DTO;

public class CreateProductDTO
{
    public string ProductName { get; set; } = string.Empty;
    public int? CategoryID { get; set; }
    public int? SupplierID { get; set; }
    public decimal UnitPrice { get; set; }
    public short UnitsInStock { get; set; }
    public short UnitsOnOrder { get; set; }
    public short ReorderLevel { get; set; }
    public string? QuantityPerUnit { get; set; }
}