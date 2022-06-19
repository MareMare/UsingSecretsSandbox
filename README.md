# Secrets をローカルとGitHubで使用する方法

ローカルと GitHub 上の CI の両方でシークレットを扱う方法を考えてみます。

## ソリューションとプロジェクトの準備

```ps1
dotnet new console -o ConsoleApp
dotnet new xunit -o UnitTests
dotnet new sln
dotnet sln add ConsoleApp
dotnet sln add UnitTests
```

## シークレットの準備

```ps1
dotnet user-secrets -p ConsoleApp init
dotnet user-secrets -p UnitTests init
```

## アプリ用のシークレット

アプリ用のシークレットを POCO にマッピングする方法でやってみます。

### 1. NuGet パッケージの参照追加

```ps1
dotnet add ConsoleApp package Microsoft.Extensions.Configuration.Binder
dotnet add ConsoleApp package Microsoft.Extensions.Configuration.UserSecrets
```

### 2. シークレットの設定

```ps1
dotnet user-secrets -p ConsoleApp set "AppSettings:User" "ゆーざ"
dotnet user-secrets -p ConsoleApp set "AppSettings:ApiKey" "きー"
```

### 3. POCO クラスの定義

```cs
public class AppSettings
{
    public string User { get; set; }
    public string ApiKey { get; set; }
}
```

### 4. コードからシークレット取得

```cs
using System.Reflection;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddUserSecrets(Assembly.GetExecutingAssembly())
    .Build();

var appSetting = config
    .GetSection(nameof(AppSettings))
    .Get<AppSettings>();

Console.WriteLine($"{appSetting.User} {appSetting.ApiKey}");
```

## Unit Test 用のシークレット(環境変数版)

Unit Test 用のシークレットは GitHub Actions からも指定できるようにするため、
JSON ではなく KeyValueペアとする方法でやってみます。

### 1. NuGet パッケージの参照追加

```ps1
dotnet add UnitTests package Microsoft.Extensions.Configuration.EnvironmentVariables
dotnet add UnitTests package Microsoft.Extensions.Configuration.UserSecrets
```

### 2. シークレットの設定

```ps1
dotnet user-secrets -p UnitTests set "User" "Testゆーざ"
dotnet user-secrets -p UnitTests set "ApiKey" "Testきー"
```

### 3. コードからシークレット取得

```cs
using Microsoft.Extensions.Configuration;

namespace UnitTests;

public class UnitTest1
{
    private readonly IConfiguration _config;
    public UnitTest1()
    {
        _config = new ConfigurationBuilder()
            .AddUserSecrets<UnitTest1>() // (A) for local
            .AddEnvironmentVariables()   // (B) for dotnet test env in github actions
            .Build();
    }

    [Fact]
    public void Test_User()
    {
        var actual = _config["user"];
        Assert.Equal("Testゆーざ", actual);
    }

    [Fact]
    public void Test_ApiKey()
    {
        var actual = _config["apikey"];
        Assert.Equal("Testきー", actual);
    }
}
```

## GitHub Actions

### 1. GitHub Actions secrets の準備

![](doc/GitHub-Actions-secrets.png)

### 2. Workflow の設定

```yml
# ...
      - name: 🧪 Test
        working-directory: src
        run: dotnet test --configuration $env:Configuration --no-build --verbosity normal
        env:
          user: ゆーざTest
          apikey: ${{ secrets.SAMPLEAPIKEY }}
          Configuration: ${{ matrix.configuration }}
# ...
```

## 参考
* [Azure 向けの GitHub Actions の variable substitution を使用する \| Microsoft Docs](https://docs.microsoft.com/ja-jp/azure/developer/github/github-variable-substitution)
* [Using secrets safely in development with \.NET Core – Sam Learns Azure](https://samlearnsazure.blog/2020/06/17/using-secrets-safely-in-development-with-net-core/)
* [Avoid Secrets in DotNet Core Tests\.](https://patrickhuber.github.io/2017/07/26/avoid-secrets-in-dot-net-core-tests.html)
* [Using User Secrets Configuration In \.NET \- \.NET Core Tutorials](https://dotnetcoretutorials.com/2022/04/28/using-user-secrets-configuration-in-net/)

## 参考（その２）
* [暗号化されたシークレット \- GitHub Docs](https://docs.github.com/ja/actions/security-guides/encrypted-secrets)
* [Managing Secrets in \.NET Console Apps](https://swharden.com/blog/2021-10-09-console-secrets/)
* [integration testing \- How to configure \.net core 3\.1 appsettings to run tests on Github actions \- Stack Overflow](https://stackoverflow.com/questions/62220945/how-to-configure-net-core-3-1-appsettings-to-run-tests-on-github-actions)
* [Microsoft\.Extensions\.Configuration\.UserSecrets 6\.0\.0\-preview\.1\.21102\.12 throwing secrets\.json error in CI/CD pipelines · Issue \#48485 · dotnet/runtime](https://github.com/dotnet/runtime/issues/48485)
* [Dotnet6 upgrade with recommended solution by samsmithnz · Pull Request \#3 · samsmithnz/UserSecretsRegression](https://github.com/samsmithnz/UserSecretsRegression/pull/3/files)

## 参考（その３）
* [How to manage secrets in \.NET locally and on GitHub? \- Maytham Fahmi](https://itbackyard.com/how-to-manage-secrets-in-net-locally-and-on-github/)

