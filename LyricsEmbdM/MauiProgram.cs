using CommunityToolkit.Maui;
namespace LyricsEmbdM;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
        // After initializing the .NET MAUI Community Toolkit, optionally add additional fonts
        // In your MauiProgram.cs
#if ANDROID
        builder.Services.AddSingleton<IMusicFilePicker, LyricsEmbdM.Platforms.Android.AndroidMusicFilePicker>();
#endif
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
        return builder.Build();
	}
}
