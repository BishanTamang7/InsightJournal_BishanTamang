using InsightJournal.Data;
using InsightJournal.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InsightJournal
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
                });

            builder.Services.AddMauiBlazorWebView();

            // SQLite database setup
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "insightjournal.db");

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite($"Data Source={dbPath}"));

            // Register ThemeService as Singleton
            builder.Services.AddSingleton<ThemeService>();

            // Register Services with proper lifetimes
            builder.Services.AddScoped<JournalService>();
            builder.Services.AddScoped<PdfExportService>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();

            // Configure logging for debug mode
            builder.Logging.AddDebug();
            builder.Logging.SetMinimumLevel(LogLevel.Information);
#else
            // Configure logging for release mode
            builder.Logging.SetMinimumLevel(LogLevel.Warning);
#endif

            var app = builder.Build();

            // Initialize database with error handling
            try
            {
                var scope = app.Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger("DatabaseInitialization");

                logger.LogInformation("Initializing database at: {DbPath}", dbPath);
                db.Database.EnsureCreated();
                logger.LogInformation("Database initialized successfully");
            }
            catch (Exception ex)
            {
                // Log the error but don't crash the app
                var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("DatabaseInitialization");
                logger.LogCritical(ex, "Failed to initialize database. Application may not function correctly.");

#if DEBUG
                throw; // Re-throw in debug mode for development
#endif
            }

            return app;
        }
    }
}