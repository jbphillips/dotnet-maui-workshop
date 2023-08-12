using Microsoft.Extensions.Logging;
using MonkeyFinder.Services;
using MonkeyFinder.View;

namespace MonkeyFinder;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif


        builder.Services.AddSingleton<MonkeyService>();

        builder.Services.AddSingleton<MonkeysViewModel>();
		// Transient because we are creating a new view each time a new monkey details are displayed
		builder.Services.AddTransient<MonkeyDetailsViewModel>();

        builder.Services.AddSingleton<MainPage>();
        // Transient because we are creating a new view each time a new monkey details are displayed
        builder.Services.AddTransient<DetailsPage>();

        return builder.Build();
	}
}
