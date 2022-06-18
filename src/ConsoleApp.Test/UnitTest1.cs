using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace ConsoleApp.Test;

public class UnitTest1
{
    private readonly IConfiguration _config;
    private readonly ITestOutputHelper _testOutputHelper;

    public UnitTest1(ITestOutputHelper testOutputHelper)
    {
        // e.g.
        // secrets.json for UnitTest
        // {
        //    "username": "ゆーざTest",
        //    "apikey": "きーTest"
        // }
        this._config = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddUserSecrets<UnitTest1>(true)
            .Build();
        this._testOutputHelper = testOutputHelper;
        this.SetEnvironmentVariablesFromUserSecrets();
    }

    private void SetEnvironmentVariablesFromUserSecrets()
    {
        foreach (var kvp in this._config.GetChildren())
        {
            this._testOutputHelper.WriteLine($"{kvp.Key}={kvp.Value}");
            Environment.SetEnvironmentVariable(kvp.Key, kvp.Value);
        }
    }

    [Fact]
    public void Test_Config()
    {
        var config = new ConfigurationBuilder()
            .AddUserSecrets<UnitTest1>(true)
            .Build();
        Assert.NotNull(config);
        
        var count = config.Providers.Count();
        Assert.Equal(1, count);
    }

    [Fact]
    public void Test_User_from_config()
    {
        var actual = this._config["username"];
        Assert.Equal("ゆーざTest", actual);
    }

    [Fact]
    public void Test_ApiKey_from_config()
    {
        var actual = this._config["apikey"];
        Assert.Equal("きーTest", actual);
    }

    [Fact]
    public void Test_User_from_env()
    {
        var actual = Environment.GetEnvironmentVariable("username");
        Assert.Equal("ゆーざTest", actual);
    }

    [Fact]
    public void Test_ApiKey_from_env()
    {
        var actual = Environment.GetEnvironmentVariable("apikey");
        Assert.Equal("きーTest", actual);
    }
}