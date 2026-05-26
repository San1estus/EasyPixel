using CommunityToolkit.Maui;
using CrochetIt.Services;
using CrochetIt.ViewModels;
using CrochetIt.Views;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace CrochetIt
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseSkiaSharp()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<IImageProcessingService, ImageProcessingService>();
            builder.Services.AddTransient<PatternEditorPage>();
            builder.Services.AddTransient<CatalogoPage>();
            builder.Services.AddTransient<PatternEditorViewModel>();
            builder.Services.AddSingleton<IApiService>(sp =>
            {
                var httpClient = new HttpClient
                {
                    BaseAddress = new Uri("https://localhost:7089/api/")
                };

                return new ApiService(httpClient);
            });
            return builder.Build();
        }
    }
}
