using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace GroceryMaster.Maui.Maui.ViewModels;

public partial class StoreEdit : ObservableObject
{
    private readonly IGroceryDataService service;
    private Store selectedStore;

    public StoreEdit(IGroceryDataService service)
    {
        this.service = service;
        Stores = new ObservableCollection<Store>(GetStores());
    }

    public Store SelectedStore
    {
        get
        {
            Application.Current.MainPage.DisplayAlert("Selected", $"Get {selectedStore.Name}", "OK");
            return selectedStore;
        }
        set
        {
            var name = value != null ? value.Name : "na";
            Application.Current.MainPage.DisplayAlert("Selected", $@"Set {name}", "OK");
            SetProperty(ref selectedStore, value);
        }
    }

    public ObservableCollection<Store> Stores { get; private set; }

    public IEnumerable<Store> GetStores() => service.GetStores();
}