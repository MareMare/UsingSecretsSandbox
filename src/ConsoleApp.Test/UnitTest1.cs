namespace ConsoleApp.Test;

public class UnitTest1 : IClassFixture<Fixture>
{
    private readonly Fixture _fixture;

    public UnitTest1(Fixture fixture)
    {
        this._fixture = fixture;
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
}