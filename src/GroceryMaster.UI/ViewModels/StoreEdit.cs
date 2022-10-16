using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GroceryMaster.Maui.Maui.Pages;
using System.Collections.ObjectModel;

namespace GroceryMaster.Maui.Maui.ViewModels;

public partial class StoreEdit : ObservableObject
{
    private readonly IGroceryDataService service;

    [ObservableProperty]
    ObservableCollection<Store> stores;

    [ObservableProperty]
    Store selectedStore;

    [ObservableProperty]
    string selectedStoreName;

    [ObservableProperty]
    string newStoreName;

    public StoreEdit(IGroceryDataService service)
    {
        this.service = service;
        Refresh();
    }

    partial void OnSelectedStoreChanged(Store value)
    {
        SelectedStoreName = value.Name;
    }

    [RelayCommand]
    private void Refresh()
    {
        Stores = new ObservableCollection<Store>(GetStores());        
    }

    [RelayCommand]
    private void AddStore()
    {
        if (String.IsNullOrWhiteSpace(NewStoreName))
        {
            Application.Current.MainPage.DisplayAlert("Error","Name is required", "OK");
            return;
        }
        try
        {
            Store store = new() { Name = NewStoreName };
            store = service.AddStore(store);
            service.SaveChanges();
            Stores.Add(store);
            NewStoreName = "";
        }
        catch (Exception ex)
        {
            Application.Current.MainPage.DisplayAlert("Error", ex.GetBaseException().Message, "OK");
        }
    }
    [RelayCommand]
    private void DeleteStore(object store)
    {
        var x = (Store)store;
        service.DeleteStore(x);
        service.SaveChanges();
        Stores.Remove(x);
    }

    public IEnumerable<Store> GetStores() => service.GetStores();
}