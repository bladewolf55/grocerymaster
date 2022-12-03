using GroceryMaster.Data.Models;
using GroceryMaster.UI.ViewModels;
using GroceryMaster.Services;
using System.Collections.ObjectModel;

namespace GroceryMaster.UnitTests;

public class AisleEdit_Should
{
    private Aisle aisle;
    private AisleEdit aisleEdit;
    private IGroceryDataService service;

    public AisleEdit_Should()
    {
        service = Substitute.For<IGroceryDataService>();
        this.aisle = new Aisle()
        {
            AisleId = 1,
            Name = "Aisle1"
        };
        aisleEdit = new(aisle, service);
    }

    [Fact]
    public void Add_an_item()
    {
        // arrange
        string name = "Blamo";
        Item item = new()
        {
            Name = name,
        };
        service.AddItem(Arg.Is<Item>(a => a.Name == name && a.AisleId == aisle.AisleId)).Returns(item);

        // act
        aisleEdit.AddItem(name);

        // assert
        service.Received().AddItem(Arg.Is<Item>(a => a.Name == name && a.AisleId == aisle.AisleId));
        service.Received().SaveChanges();
        aisleEdit.Items.Should().HaveCount(1);
    }

    [Fact]
    public void Delete_select_items()
    {
        // arrange
        Item item1 = new() { ItemId = 1, AisleId = aisle.AisleId };
        Item item2 = new() { ItemId = 2, AisleId = aisle.AisleId };
        Item item3 = new() { ItemId = 3, AisleId = aisle.AisleId };
        List<Item> items = new() { item1, item2, item3 };
        aisleEdit.Items = new ObservableCollection<Item>(items);
        aisleEdit.SelectedItems = items.Take(2).ToList();

        // act
        aisleEdit.DeleteSelectedItemsCommand.Execute(null);

        // assert
        service.Received().DeleteItem(item1);
        service.Received().DeleteItem(item2);
        aisleEdit.Items.Should().HaveCount(1);
    }

    [Fact]
    public void Get_an_item_list()
    {
        // arrange
        List<Item> items = new()
        {
            new Item { ItemId = 1, AisleId = aisle.AisleId},
            new Item { ItemId = 2, AisleId = aisle.AisleId}
        };

        service.GetItems(aisle.AisleId).Returns(items);

        // act
        var result = aisleEdit.GetItems();

        // assert
        result.Should().HaveCount(2);
    }
}