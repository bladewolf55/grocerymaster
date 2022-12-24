using GroceryMaster.UI.ViewModels;

namespace GroceryMaster.UI.Pages;

public partial class AisleItemsPage : ContentPage
{
    private IGroceryDataService service;
    private AisleItemsEdit vm;

    public AisleItemsPage(AisleItemsEdit vm, IGroceryDataService service)
	{
		InitializeComponent();
        BindingContext = vm;
        this.vm = vm;
        this.service = service;
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {

    }


    private void DragGestureRecognizer_DragStarting(object sender, DragStartingEventArgs e)
    {
        var itemContext = ((DragGestureRecognizer)sender).BindingContext as Item;
        e.Data.Properties.Add("Item", itemContext);
    }

    private void DropGestureRecognizer_Drop(object sender, DropEventArgs e)
    {
        var dragItem = e.Data.Properties["Item"] as Item;
        var dropItem = (sender as DropGestureRecognizer).BindingContext as Item;
 
        vm.DragDropItem(dragItem, dropItem);
    }

    private void Editor_TextChanged(object sender, TextChangedEventArgs e)
    {

    }
}