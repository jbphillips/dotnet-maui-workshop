namespace MonkeyFinder.ViewModel;

// I could have used this over in the details code behind but doing here is cleaner
[QueryProperty("Monkey", "Monkey")]
public partial class MonkeyDetailsViewModel : BaseViewModel
{
    public MonkeyDetailsViewModel() 
    { 
        
    }

    [ObservableProperty]
    Monkey monkey;
}
