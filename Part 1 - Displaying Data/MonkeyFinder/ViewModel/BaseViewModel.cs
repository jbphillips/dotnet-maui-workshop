using CommunityToolkit.Mvvm.ComponentModel;

namespace MonkeyFinder.ViewModel;

// [INotifyPropertyChanged] // I could use this attribute. But better to implement ObservableObject
public partial class BaseViewModel : ObservableObject
{
    // instead of hand writing my own implementation
    // I am using the Community Toolkit.Mvvm.
    // can see it generated in the analyzers section in the denpendencies

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool isBusy;

    [ObservableProperty]
    private string title;

    public BaseViewModel()
    {

    }

    public bool IsNotBusy => !IsBusy;

}
