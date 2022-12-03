using GroceryMaster.Data.Models;
using GroceryMaster.UI.ViewModels;
using GroceryMaster.Services;
using System.Collections.ObjectModel;

namespace GroceryMaster.UnitTests;

public class StoreEdit_Should
{
    private IGroceryDataService service;
    private Store store;
    private StoreEdit storeEdit;

    public StoreEdit_Should()
    {
        service = Substitute.For<IGroceryDataService>();
        this.store = new Store()
        {
            StoreId = 1,
            Name = "Store1"
        };
        storeEdit = new(store, service);
    }

    [Fact]
    public void Add_an_aisle()
    {
        // arrange
        string name = "Blamo";
        Aisle aisle = new()
        {
            Name = name,
        };

        service.AddAisle((Arg.Is<Aisle>(a => a.Name == name && a.StoreId == store.StoreId))).Returns(aisle);

        // act
        storeEdit.AddAisle(name);

        // assert
        service.Received().AddAisle(Arg.Is<Aisle>(a => a.Name == name && a.StoreId == store.StoreId));
        service.Received().SaveChanges();
        storeEdit.Aisles.Should().HaveCount(1);
    }

    [Fact]
    public void Delete_select_aisles()
    {
        // arrange
        Aisle aisle1 = new() { AisleId = 1, StoreId = store.StoreId };
        Aisle aisle2 = new() { AisleId = 2, StoreId = store.StoreId };
        Aisle aisle3 = new() { AisleId = 3, StoreId = store.StoreId };
        List<Aisle> aisles = new() { aisle1, aisle2, aisle3 };
        storeEdit.Aisles = new ObservableCollection<Aisle>(aisles);
        storeEdit.SelectedAisles = aisles.Take(2).ToList();

        // act
        storeEdit.DeleteSelectedAislesCommand.Execute(null);

        // assert
        service.Received().DeleteAisle(aisle1);
        service.Received().DeleteAisle(aisle2);
        storeEdit.Aisles.Should().HaveCount(1);
    }

    [Fact]
    public void Get_an_aisle_list()
    {
        // arrange
        List<Aisle> aisles = new()
        {
            new Aisle { AisleId = 1, StoreId = store.StoreId},
            new Aisle { AisleId = 2, StoreId = store.StoreId}
        };

        service.GetAisles(store.StoreId).Returns(aisles);

        // act
        var result = storeEdit.GetAisles();

        // assert
        result.Should().HaveCount(2);
    }
}