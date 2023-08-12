using Android.Views.Accessibility;
using MonkeyFinder.Services;

namespace MonkeyFinder.ViewModel;

public partial class MonkeysViewModel : BaseViewModel
{
    MonkeyService monkeyService;
    public ObservableCollection<Monkey> Monkeys { get; } = new ObservableCollection<Monkey>();

    // This allows you to get monkeys from MAUI xaml.
    // GetMonkeysAsync is a private call.
    // Dont need to do it this way anymore. Use the toolkit
    //public Command GetMonkeysCommand { get; }

    public MonkeysViewModel(MonkeyService monkeyService)
    {
        Title = "Monkey Finder";
        this.monkeyService = monkeyService;

        // dont need to do it this way anymore. Use the toolkit and add attribute to the getmonkeys call [RelayCommand]
        //GetMonkeysCommand = new Command(async () => await GetMonkeysAsync());

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
