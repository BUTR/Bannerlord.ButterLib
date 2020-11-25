$test1 = 'tests/Bannerlord.ButterLib.Tests/Bannerlord.ButterLib.Tests.csproj';
$test2 = 'tests/Bannerlord.ButterLib.Implementation.Tests/Bannerlord.ButterLib.Implementation.Tests.csproj';

# Test Supported Versions
$array = Get-Content -Raw -Path test.json | ConvertFrom-Json;
ForEach ($entry in $array) 
{
    $gameversion = $entry.version;
    $version = $gameversion.substring(1);

    dotnet test $test1 --configuration Debug -p:GameVersion=$version;
    dotnet test $test1 --configuration Release -p:GameVersion=$version;
    dotnet test $test2 --configuration Stable_Debug -p:GameVersion=$version;
    dotnet test $test2 --configuration Stable_Release -p:GameVersion=$version;
}

dotnet test $test1 --configuration Debug;
dotnet test $test1 --configuration Release;

# Test Beta Implementation dll's
dotnet test $test2 --configuration Stable_Debug;
dotnet test $test2 --configuration Stable_Release;

# Test Stable Module
dotnet test $test2 --configuration Beta_Debug -p:GameVersion=$version;
dotnet test $test2 --configuration Beta_Release -p:GameVersion=$version;
