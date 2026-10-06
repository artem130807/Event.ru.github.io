using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EventRep.Infrastructure.Persistence;

/// <summary>
/// Creates the database context for EF Core design-time commands such as
/// migrations add, migrations script and database update.
/// </summary>
public sealed class EventRepDbContextFactory
    : IDesignTimeDbContextFactory<EventRepDbContext>
{
    private const string ConnectionStringName = "DefaultConnection";
    private const string ConnectionStringEnvironmentVariable =
        "ConnectionStrings__DefaultConnection";

    public EventRepDbContext CreateDbContext(string[] args)
    {
        var connectionString = GetConnectionString(args);

        var optionsBuilder = new DbContextOptionsBuilder<EventRepDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new EventRepDbContext(optionsBuilder.Options);
    }

    private static string GetConnectionString(string[] args)
    {
        var connectionString = GetCommandLineConnectionString(args)
            ?? Environment.GetEnvironmentVariable(
                ConnectionStringEnvironmentVariable)
            ?? GetConnectionStringFromAppSettings();

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                $"Connection string '{ConnectionStringName}' was not found. " +
                $"Pass '--connection <value>' to dotnet-ef, set the " +
                $"'{ConnectionStringEnvironmentVariable}' environment variable, " +
                "or configure it in EventRep.Api/appsettings.json.");
        }

        return connectionString;
    }

    private static string? GetCommandLineConnectionString(string[] args)
    {
        for (var index = 0; index < args.Length; index++)
        {
            if (args[index].StartsWith("--connection=", StringComparison.OrdinalIgnoreCase))
            {
                return args[index]["--connection=".Length..];
            }

            if (args[index].Equals("--connection", StringComparison.OrdinalIgnoreCase) &&
                index + 1 < args.Length)
            {
                return args[index + 1];
            }
        }

        return null;
    }

    private static string? GetConnectionStringFromAppSettings()
    {
        var appSettingsPath = FindAppSettingsPath();
        if (appSettingsPath is null)
        {
            return null;
        }

        using var stream = File.OpenRead(appSettingsPath);
        using var document = JsonDocument.Parse(stream);

        if (!document.RootElement.TryGetProperty("ConnectionStrings", out var section) ||
            !section.TryGetProperty(ConnectionStringName, out var connectionString))
        {
            return null;
        }

        return connectionString.GetString();
    }

    private static string? FindAppSettingsPath()
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());

        while (directory is not null)
        {
            var paths = new[]
            {
                Path.Combine(directory.FullName, "src", "EventRep.Api", "appsettings.json"),
                Path.Combine(directory.FullName, "EventRep.Api", "appsettings.json"),
                Path.Combine(directory.FullName, "appsettings.json")
            };

            var existingPath = paths.FirstOrDefault(File.Exists);
            if (existingPath is not null)
            {
                return existingPath;
            }

            directory = directory.Parent;
        }

        return null;
    }
}
