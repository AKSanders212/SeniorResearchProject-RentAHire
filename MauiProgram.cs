/*
 * GUID: G00225605
 * Aaron Keith Sanders
 * UA Grantham
 * IS498-Senior Research Project
 * Robert Chubbuck
 * Capstone Project
 * File: MauiProgram.cs
 * Date: 17 March 2026
 */
using Microsoft.Extensions.Logging;
using SQLite; // Just in case I need to include SQLite here as well

namespace SandersCapstoneProject
{
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
            var mydb = new MyDatabase(); // Creating an object to use MyDatabase() class
            return builder.Build();
        }
    }
}
