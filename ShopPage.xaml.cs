using Microsoft.Maui.Devices.Sensors;
using Plugin.LocalNotification;
using StelicIonutLab7.Models;
namespace StelicIonutLab7;

public partial class ShopPage : ContentPage
{
	public ShopPage()
	{
		InitializeComponent();
	}

    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;

        if (await DisplayAlert("Delete Shop", $"Are you sure you want to delete {shop.ShopName}?", "Yes", "No"))
        {
            await App.Database.DeleteShopAsync(shop); 
            await Navigation.PopAsync();
        }
    }

    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;
        await App.Database.SaveShopAsync(shop);
        await Navigation.PopAsync();
    }

    async void OnShowMapButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;
        var address = shop.Adress;
        //var locations = await Geocoding.GetLocationsAsync(address);

        var options = new MapLaunchOptions
        {
            Name = "Magazinul meu preferat" };
        //var shoplocation = locations?.FirstOrDefault();
            var shoplocation= new Location(46.7492379, 23.5745597);//pentru Windows Machine
            var distance = shoplocation.CalculateDistance(shoplocation, DistanceUnits.Kilometers);

        if (distance < 5)
        {
            var request = new NotificationRequest
            {
                Title = "Ai de facut cumparaturi in apropiere!",
                Description = address,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now.AddSeconds(1)
                }
            };
            LocalNotificationCenter.Current.Show(request);
        }

        await Map.OpenAsync(shoplocation, options);
        }
}