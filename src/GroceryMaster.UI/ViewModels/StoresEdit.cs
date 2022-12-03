using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace GroceryMaster.UI.ViewModels;

public partial class StoresEdit : ObservableObject
{
    private readonly IGroceryDataService service;

    [ObservableProperty]
    private ObservableCollection<Store> stores;

    public StoresEdit(IGroceryDataService service)
    {
        this.service = service;
        Refresh();
    }

    public List<Store> SelectedStores { get; set; } = new List<Store>();

    public void AddStore(string name)
    {
        Store store = new() { Name = name };
        store = service.AddStore(store);
        service.SaveChanges();
        Stores.Add(store);
    }

    public void ChangeStoreSelection(Store store, CheckedChangedEventArgs e)
    {
        if (e.Value == true)
        {
            SelectedStores.Add(store);
        }
        else
        {
            SelectedStores.Remove(store);
        }
    }

    public IEnumerable<Store> GetStores() => service.GetStores();

    [RelayCommand]
    private async void AddStoreWithPrompt()
    {
        string result = await Application.Current.MainPage.DisplayPromptAsync("Store entry", "Enter the store name");
        if (result != null)
        {
            try
            {
                AddStore(result);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.GetBaseException().Message, "OK");
            }
        }
    }

    [RelayCommand]
    private void DeleteSelectedStores()
    {
        List<Store> deleted = new();
        try
        {
            foreach (var store in SelectedStores)
            {
                service.DeleteStore(store);
                service.SaveChanges();
                Stores.Remove(store);
                deleted.Add(store);
            }
        }
        catch (Exception ex)
        {
        }
        finally { SelectedStores = SelectedStores.Except(deleted).ToList(); }
    }

    [RelayCommand]
    private void Refresh()
    {
        Stores = new ObservableCollection<Store>(GetStores().OrderBy(a => a.Name));
    }
}