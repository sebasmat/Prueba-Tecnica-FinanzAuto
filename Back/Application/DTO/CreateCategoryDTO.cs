namespace Back.Application.DTO;

public class CreateCategoryDTO
{
    public string CategoryName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public byte[]? Picture { get; set; } // Opcional según el diagrama
}