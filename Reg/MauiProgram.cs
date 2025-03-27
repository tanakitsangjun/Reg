using Microsoft.Extensions.Logging;
using Reg.Pages;
using Reg.ViewModel;

namespace Reg;

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
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif
		builder.Services.AddTransient<LoginPage>();
		builder.Services.AddTransient<LoginVM>();
		builder.Services.AddTransient<HomePage>();
		builder.Services.AddTransient<HomepageVM>();
		builder.Services.AddTransient<RegisterPage>();
		builder.Services.AddTransient<RegisterVM>();
		return builder.Build();
	}
}
