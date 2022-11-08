using GroceryMaster.Data.Models;
namespace GroceryMaster.UI.Pages;

public partial class StorePage : ContentPage
{
	public StorePage(Store store)
	{
		InitializeComponent();
		BindingContext = store;
	}
}