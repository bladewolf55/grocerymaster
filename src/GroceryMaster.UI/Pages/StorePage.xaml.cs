using GroceryMaster.UI.ViewModels;
using System.Collections.ObjectModel;

namespace GroceryMaster.UI.Pages;

public partial class StorePage : ContentPage
{
    private IGroceryDataService service;
    private StoreEdit vm;

    public StorePage(StoreEdit vm, IGroceryDataService service)
    {
        InitializeComponent();
        BindingContext = vm;
        this.vm = vm;
        this.service = service;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        var aisle = ((Button)sender).BindingContext as Aisle;
        var vm = new AisleEdit(aisle, service);
        Application.Current.MainPage.Navigation.PushAsync(new AislePage(vm, service), true);
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var aisle = ((CheckBox)sender).BindingContext as Aisle;
        vm.ChangeAisleSelection(aisle, e);
    }

    private void DragGestureRecognizer_DragStarting(object sender, DragStartingEventArgs e)
    {
        var context = ((DragGestureRecognizer)sender).BindingContext as Aisle;
        e.Data.Properties.Add("Aisle", context);
    }

    private void DropGestureRecognizer_Drop(object sender, DropEventArgs e)
    {
        var dragAisle = e.Data.Properties["Aisle"] as Aisle;
        var dropAisle = (sender as DropGestureRecognizer).BindingContext as Aisle;
        // for now, if drag sequence < drop, insert after drop
        // if drag seq > drop, insert before drop

        vm.Aisles.Remove(dragAisle);
        var dropIndex = vm.Aisles.IndexOf(dropAisle);
        if (dragAisle.Sequence < dropAisle.Sequence)
            dropIndex++;

        vm.Aisles.Insert(dropIndex, dragAisle);
        vm.SetAisleSequences();
    }
}