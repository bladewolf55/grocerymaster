using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace GroceryMaster.UI.ViewModels;

public partial class StoreEdit : ObservableObject
{
    private readonly IGroceryDataService service;

    [ObservableProperty]
    private ObservableCollection<Aisle> aisles;

    [ObservableProperty]
    private Store store;

    public StoreEdit(Store store, IGroceryDataService service)
    {
        this.store = store;
        this.service = service;
        Refresh();
    }

    public List<Aisle> SelectedAisles { get; set; } = new List<Aisle>();

    public void AddAisle(string name)
    {
        Aisle aisle = new() { Name = name, StoreId = Store.StoreId };
        aisle = service.AddAisle(aisle);
        service.SaveChanges();
        Aisles.Add(aisle);
        SetAisleSequences();
    }

    public void ChangeAisleSelection(Aisle Aisle, CheckedChangedEventArgs e)
    {
        if (e.Value == true)
        {
            SelectedAisles.Add(Aisle);
        }
        else
        {
            SelectedAisles.Remove(Aisle);
        }
    }

    public IEnumerable<Aisle> GetAisles() => service.GetAisles(store.StoreId);

    public void SetAisleSequences()
    {
        for (int i = 1; i <= Aisles.Count; i++)
        {
            var aisle = Aisles[i - 1];
            aisle.Sequence = i;
            service.UpdateAisle(aisle);
            service.SaveChanges();
        }
    }

    [RelayCommand]
    private async void AddAisleWithPrompt()
    {
        string result = await Application.Current.MainPage.DisplayPromptAsync("Aisle entry", "Enter the Aisle name");
        if (result != null)
        {
            try
            {
                AddAisle(result);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.GetBaseException().Message, "OK");
            }
        }
    }

    [RelayCommand]
    private void DeleteSelectedAisles()
    {
        List<Aisle> deleted = new();
        try
        {
            foreach (var Aisle in SelectedAisles)
            {
                service.DeleteAisle(Aisle);
                service.SaveChanges();
                Aisles.Remove(Aisle);
                deleted.Add(Aisle);
            }
            SetAisleSequences();
        }
        catch (Exception ex)
        {
        }
        finally { SelectedAisles = SelectedAisles.Except(deleted).ToList(); }
    }

    [RelayCommand]
    private void Refresh()
    {
        Aisles = new ObservableCollection<Aisle>(GetAisles().OrderBy(a => a.Sequence));
    }
}