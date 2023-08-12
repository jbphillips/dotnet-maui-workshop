namespace MonkeyFinder.ViewModel;

// I could have used this over in the details code behind but doing here is cleaner
[QueryProperty("Monkey", "Monkey")]
public partial class MonkeyDetailsViewModel : BaseViewModel
{
    IMap map;

    public MonkeyDetailsViewModel(IMap map) 
    { 
        this.map = map;
    }

    [ObservableProperty]
    Monkey monkey;

    [RelayCommand]
    async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync(".."); // .. is go back -- ..?id=1 passes parameters
    }

    [RelayCommand]
    async Task OpenMapAsync()
    {
        try
        {
            await map.OpenAsync(Monkey.Latitude, Monkey.Longitude,
                new MapLaunchOptions
                {
                    Name = Monkey.Name,
                    NavigationMode = NavigationMode.None
                });
        }
        catch(Exception ex)
        {
            await Shell.Current.DisplayAlert("Error!", $"Unable to open map: {ex.Message}", "Ok");

        }
    }
}
