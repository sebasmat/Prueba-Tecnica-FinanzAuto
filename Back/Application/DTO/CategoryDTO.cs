namespace Back.Application.DTO;

public class CategoryDTO
{
    public int CategoryID { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? Description { get; set; }
}