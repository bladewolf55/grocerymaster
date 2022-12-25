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

    private void Editor_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void collectionView_ReorderCompleted(object sender, EventArgs e)
    {

    }
}