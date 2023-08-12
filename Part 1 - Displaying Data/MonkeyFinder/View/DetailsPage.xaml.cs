namespace MonkeyFinder;

public partial class DetailsPage : ContentPage
{
	/// <summary>
	/// we have to know what object we are showing details for, MAUI will take parameters
	/// </summary>
	public DetailsPage(MonkeyDetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}