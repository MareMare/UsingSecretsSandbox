// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using var host = Host.CreateDefaultBuilder(args)
    .UseEnvironment("Development") // secrets.json の構成プロバイダーは "開発環境" が対象
    .ConfigureServices((hostingContext, services) =>
    {
        // POCO へのマッピング
        var appSetting = hostingContext.Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
        services
            .AddSingleton(appSetting)
            .AddTransient<Hoge>();
    })
    .Build();

using var scope = host.Services.CreateScope();
var services = scope.ServiceProvider;

// インデクサによる取得
var config = services.GetRequiredService<IConfiguration>();
Console.WriteLine($"indexer: {config["AppSettings:User"]} {config["AppSettings:ApiKey"]}");

// POCO へのマッピングによる取得
var appSetting = services.GetRequiredService<AppSettings>();
Console.WriteLine($"POCO: {appSetting.User} {appSetting.ApiKey}");

// DI による取得
var hoge = scope.ServiceProvider.GetRequiredService<Hoge>();
hoge.Dump();

public class AppSettings
{
    public string User { get; set; } = null!;
    public string ApiKey { get; set; } = null!;
}

public class Hoge
{
    private readonly AppSettings _appSettings;

    public Hoge(AppSettings appSettings) => _appSettings = appSettings;

    public void Dump() => Console.WriteLine($"DI: {_appSettings.User} {_appSettings.ApiKey}");
}