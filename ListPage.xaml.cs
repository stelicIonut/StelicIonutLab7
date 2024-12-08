using StelicIonutLab7.Models;
namespace StelicIonutLab7;


public partial class ListPage : ContentPage
{
    public ListPage()
    {
        InitializeComponent();
    }

    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        slist.Date = DateTime.UtcNow;
        await App.Database.SaveShopListAsync(slist);
        await Navigation.PopAsync();
    }
    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        await App.Database.DeleteShopListAsync(slist);
        await Navigation.PopAsync();
    }

    async void OnChooseButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProductPage((ShopList)
        this.BindingContext)
        {
            BindingContext = new Product()
        });
    }

    async void OnDeleteButtonItemClicked(object sender, EventArgs e)
    {
        var currentShopList = BindingContext as ShopList;
        var selectedProduct = listView.SelectedItem as Product;

        if (selectedProduct != null && currentShopList != null)
        {
            await App.Database.DeleteItemFromShopListAsync(selectedProduct.ID, currentShopList.ID);

            listView.ItemsSource = await App.Database.GetListProductsAsync(currentShopList.ID);

            listView.SelectedItem = null;
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var shopl = (ShopList)BindingContext;
        listView.ItemsSource = await App.Database.GetListProductsAsync(shopl.ID);
    }
}