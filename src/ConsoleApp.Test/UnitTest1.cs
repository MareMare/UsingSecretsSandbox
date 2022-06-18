using Microsoft.Extensions.Configuration;

namespace ConsoleApp.Test;

public class UnitTest1 : IClassFixture<Fixture>
{
    private readonly Fixture _fixture;

    public UnitTest1(Fixture fixture)
    {
        this._fixture = fixture;
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
        var actual = this._fixture.Config["username"];
        Assert.Equal("ゆーざTest", actual);
    }

    [Fact]
    public void Test_ApiKey_from_config()
    {
        var actual = this._fixture.Config["apikey"];
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