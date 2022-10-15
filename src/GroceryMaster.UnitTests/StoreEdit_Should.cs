using GroceryMaster.Data.Models;
using GroceryMaster.Maui.Maui.ViewModels;
using GroceryMaster.Services;
using Xunit;

namespace GroceryMaster.UnitTests;

public class StoreEdit_Should
{
    IGroceryDataService service;
    StoreEdit storeEdit;

    public StoreEdit_Should()
    {
        service = Substitute.For<IGroceryDataService>();
        storeEdit = new(service);
    }

    [Fact]
    public void Get_a_store_list()
    {
        // arrange
        List<Store> stores = new() 
        {
            new Store { StoreId = 1},
            new Store { StoreId = 2}
        };

        service.GetStores().Returns(stores);

        // act
        var result = storeEdit.GetStores();

        // assert
        result.Should().HaveCount(2);
    }
    
}
