namespace GroceryMaster.UI.Models;

public class AisleItemGroup : List<Item>
{
    public string Name { get; private set; }
    public int Sequence { get; private set;}
    public int AisleId { get; private set; }

    public AisleItemGroup(string name, int sequence, int id, List<Item> items) : base(items)
    { 
        Name = name;
        Sequence = sequence;
        AisleId = id;
    }
}
