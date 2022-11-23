using Microsoft.Extensions.Logging;

namespace AppTarefa.Mobile;

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
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("Montserrat-Bold.ttf", "MontSerratBold");
				fonts.AddFont("Montserrat-Light.ttf", "MontSerratLight");
				fonts.AddFont("Montserrat-Regular.ttf", "MontSerratRegular");
				fonts.AddFont("Montserrat-Medium.ttf", "MontSerratMedium");
				fonts.AddFont("Montserrat-Thin.ttf", "MontSerratThin");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
