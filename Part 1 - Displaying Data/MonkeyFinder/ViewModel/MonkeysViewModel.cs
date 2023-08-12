using MonkeyFinder.Services;
using System.Linq.Expressions;

namespace MonkeyFinder.ViewModel;

public partial class MonkeysViewModel : BaseViewModel
{
    MonkeyService monkeyService;
    public ObservableCollection<Monkey> Monkeys { get; } = new ObservableCollection<Monkey>();

    IConnectivity connectivity;
    IGeolocation geolocation;

    // This allows you to get monkeys from MAUI xaml.
    // GetMonkeysAsync is a private call.
    // Dont need to do it this way anymore. Use the toolkit
    //public Command GetMonkeysCommand { get; }

    public MonkeysViewModel(MonkeyService monkeyService, IConnectivity connectivity, IGeolocation geolocation)
    {
        Title = "Monkey Finder";
        this.monkeyService = monkeyService;
        this.connectivity = connectivity;
        this.geolocation = geolocation;

        // dont need to do it this way anymore. Use the toolkit and add attribute to the getmonkeys call [RelayCommand]
        //GetMonkeysCommand = new Command(async () => await GetMonkeysAsync());

    }

    [RelayCommand]
    async Task GetClosestMonkeyAsync(Monkey monkey)
    {
        if(IsBusy || Monkeys.Count == 0) return;

        try
        {
            var location = await geolocation.GetLastKnownLocationAsync();

            if(location is null)
            {
                location = await geolocation.GetLocationAsync(
                    new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(30)
                    });
            }

            if (location is null) return;

            var first = Monkeys.OrderBy(x => location.CalculateDistance(x.Latitude, x.Longitude, DistanceUnits.Miles)).FirstOrDefault();

            if(first is null) return;

            await Shell.Current.DisplayAlert("Closest Monkey", $"{first.Name} in {first.Location}", "Ok");
        }
        catch(Exception ex)
        {
            // this UI stuff should be in an interface or somethingc. Just doing here for this exercise
            await Shell.Current.DisplayAlert("Error!", $"Unable to closest monkey: {ex.Message}", "Ok");
        } 
    }

    [RelayCommand]
    async Task GoToDetailsAsync(Monkey monkey)
    {
        if(monkey is null)
            return;

        // pass parameters as an object, also animate the view
        await Shell.Current.GoToAsync($"{nameof(DetailsPage)}", true, 
            new Dictionary<string, object>
            {
                {"Monkey", monkey} // query identifier. Pass the monkey across
            });

        // just as a reference, you could pass parameters like this... just a variable
        // await Shell.Current.GoToAsync($"{nameof(DetailsPage)}?id={monkey.Name}");

    }

    // This is a commamnd that gets called in the view i.e. Command="{Binding GetMonkeysCommand}"
    // This is from the community toolkit. It drops off the async. So the command becomes: GetMonkeysCommand
    // see the generated code for more detail: dependencies/analyzers/...RelayCommandGenerator
    [RelayCommand]
    async Task GetMonkeysAsync()
    {
        if(IsBusy) return;

        try
        {
            if(connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("Internet Issue", $"Check your internet and try again", "Ok");
            }

            IsBusy = true;
            var monkeys = await monkeyService.GetMonkeys();

            if(monkeys.Count != 0)
            {
                Monkeys.Clear();
            }

            foreach(var monkey in monkeys)
            {
                Monkeys.Add(monkey);
            }
        }
        catch(Exception ex)
        {
            Debug.WriteLine(ex);

            // this UI stuff should be in an interface or something. Just doing here for this exercise
            await Shell.Current.DisplayAlert("Error!", $"Unable to get monkeys: {ex.Message}", "Ok");
        }
        finally 
        { 
            IsBusy = false;
        }
    }
}
