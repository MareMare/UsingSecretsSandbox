// See https://aka.ms/new-console-template for more information

using System.Reflection;
using Microsoft.Extensions.Configuration;

Console.WriteLine("Hello, World!");
var builder = new ConfigurationBuilder()
    .AddUserSecrets(Assembly.GetExecutingAssembly(), true);

var configuration = builder.Build();
var appSetting = configuration
    .GetSection(nameof(AppSettings))
    .Get<AppSettings>();
Console.WriteLine($"{appSetting.User} {appSetting.ApiKey}");

public class AppSettings
{
    public string User { get; set; }
    public string ApiKey { get; set; }
}