$impl = $PWD.Path + '/bannerlord-implementations';
$path = $PWD.Path + '/bannerlord';
$fzip = $PWD.Path + '/bannerlord.zip';
$final = $path + '/Modules/Bannerlord.ButterLib/bin/Win64_Shipping_Client/';

$proj = 'src/Bannerlord.ButterLib.Implementation/Bannerlord.ButterLib.Implementation.csproj';
$pdll = $path + '/Modules/Bannerlord.ButterLib/bin/Win64_Shipping_Client/Bannerlord.ButterLib.Implementation.*.dll';
$ppdb = $path + '/Modules/Bannerlord.ButterLib/bin/Win64_Shipping_Client/Bannerlord.ButterLib.Implementation.*.pdb';

# The folders are required to be created before executing the script
New-Item -ItemType directory -Force -Path $impl;
New-Item -ItemType directory -Force -Path $path;

$gameversions = Get-Content -Raw -Path supported-game-versions.json | ConvertFrom-Json;
# Get all implementations except the minimal version (last element)
For ($i=0; $i -le $gameversions.Length - 2; $i++)
{
    $gameversion = $gameversions[$i];
    $version = $gameversion.substring(1);
    dotnet build $proj --configuration Release -p:GameVersion=$version -p:GameFolder="$path";

    # Copy Implementations to the Implementations folder
    Copy-Item $pdll $impl/;
    Copy-Item $ppdb $impl/;
}
#
$supportedVersions = [system.String]::Join(";", ($gameversions | ForEach-Object { $_.substring(1) }));
# Build the minimal version. We will use Bannerlord.ButterLib.dll from there
$gameversion = $gameversions[-1];
$version = $gameversion.substring(1);
dotnet build $proj --configuration Release --force --% -p:SupportedVersions="$supportedVersions" -p:GameVersion=$version -p:GameFolder="$path";

# Copy Implementations to the Module
Copy-Item $impl/* $final;

# Delete Implementations folder
Remove-Item -Recurse $impl;

Compress-Archive -Path "$path/*" -DestinationPath "$fzip" -Force