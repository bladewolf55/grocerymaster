namespace GroceryMaster.Data.Models;

public class Item
{
    public int ItemId { get; set; }
    public int AisleId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Sequence { get; set; }
    public bool IsFavorite { get; set; }
    public bool IsToBuy { get; set; }
    public bool IsComplete { get; set; }
}

