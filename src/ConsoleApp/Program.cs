// See https://aka.ms/new-console-template for more information

using System.Reflection;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddUserSecrets(Assembly.GetExecutingAssembly())
    .Build();

// インデクサによる取得
Console.WriteLine($"indexer: {config["AppSettings:User"]} {config["AppSettings:ApiKey"]}");

// POCO へのマッピングによる取得
var appSetting = config
    .GetSection(nameof(AppSettings))
    .Get<AppSettings>();
Console.WriteLine($"POCO: {appSetting?.User} {appSetting?.ApiKey}");

public class AppSettings
{
    public string User { get; set; } = null!;
    public string ApiKey { get; set; } = null!;
}
