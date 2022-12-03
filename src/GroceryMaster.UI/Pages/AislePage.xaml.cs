using GroceryMaster.UI.ViewModels;
using System.Collections.ObjectModel;

namespace GroceryMaster.UI.Pages;

public partial class AislePage : ContentPage
{
    private IGroceryDataService service;
    private AisleEdit vm;

    public AislePage(AisleEdit vm, IGroceryDataService service)
    {
        InitializeComponent();
        BindingContext = vm;
        this.vm = vm;
        this.service = service;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        var item = ((Button)sender).BindingContext as Item;
        //var vm = new ItemEdit(aisle, service);
        //Application.Current.MainPage.Navigation.PushAsync(new ItemPage(vm), true);
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var item = ((CheckBox)sender).BindingContext as Item;
        vm.ChangeItemSelection(item, e);
    }

    private void collectionView_ReorderCompleted(object sender, EventArgs e)
    {
        vm.SetItemSequences();
    }
}