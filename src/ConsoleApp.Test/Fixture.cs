using Microsoft.Extensions.Configuration;

namespace ConsoleApp.Test;

public class Fixture
{
    private static readonly Lazy<IConfiguration> LazyConfig = new (Fixture.CreateConfig, true);

    public IConfiguration Config => Fixture.LazyConfig.Value;

    private static IConfiguration CreateConfig()
    {
        // e.g.
        // secrets.json for UnitTest
        // {
        //    "username": "ゆーざTest",
        //    "apikey": "きーTest"
        // }
        var config = new ConfigurationBuilder()
            //.AddJsonFile("sample.json", optional: true) // for variable-substitution
            .AddEnvironmentVariables()                  // for dotnet test env in github actions
            .AddUserSecrets<Fixture>()                  // for local
            .Build();
        return config;
    }
}
