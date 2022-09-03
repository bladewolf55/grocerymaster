namespace GroceryMaster.Data.Models;

public class Aisle
{
    public int AisleId { get; set; }
    public int StoreId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Sequence { get; set; }

    public ICollection<Item>? Items { get; set; }

}

