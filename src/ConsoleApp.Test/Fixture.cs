using Microsoft.Extensions.Configuration;

namespace ConsoleApp.Test;

public class Fixture
{
    private readonly IConfiguration _config;

    public Fixture()
    {
        // e.g.
        // secrets.json for UnitTest
        // {
        //    "username": "ゆーざTest",
        //    "apikey": "きーTest"
        // }
        this._config = new ConfigurationBuilder()
            //.AddJsonFile("sample.json", optional: true) // for variable-substitution
            .AddEnvironmentVariables()                  // for dotnet test env in github actions
            .AddUserSecrets<UnitTest1>()                // for local
            .Build();
        //this.SetEnvironmentVariablesFromUserSecrets();
    }

    public IConfiguration Config => this._config;

    private void SetEnvironmentVariablesFromUserSecrets()
    {
        foreach (var kvp in this._config.GetChildren())
        {
            Environment.SetEnvironmentVariable(kvp.Key, kvp.Value);
        }
    }
}
