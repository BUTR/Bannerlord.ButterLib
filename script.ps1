
$temp = $PWD.Path + '/bannerlord-temp';
$impl = $PWD.Path + '/bannerlord-implementations';
$path = $PWD.Path + '/bannerlord';

New-Item -ItemType directory -Force -Path $temp;
New-Item -ItemType directory -Force -Path $impl;
$array = Get-Content -Raw -Path test.json | ConvertFrom-Json;
ForEach ($entry in $array) 
{
    $gameversion = $entry.version;
    $version = $gameversion.substring(1);
    dotnet build src/Bannerlord.ButterLib.Implementation/Bannerlord.ButterLib.Implementation.csproj --configuration Stable_Release -p:GameVersion=$version -p:GameFolder="$temp";
    Copy-Item $temp/Modules/Bannerlord.ButterLib/bin/Win64_Shipping_Client/Bannerlord.ButterLib.Implementation.$version.dll $impl/;
    Copy-Item $temp/Modules/Bannerlord.ButterLib/bin/Win64_Shipping_Client/Bannerlord.ButterLib.Implementation.$version.pdb $impl/;
}

dotnet build src/Bannerlord.ButterLib.Implementation/Bannerlord.ButterLib.Implementation.csproj --configuration Beta_Release -p:GameFolder="$temp";
Copy-Item $temp/Modules/Bannerlord.ButterLib/bin/Win64_Shipping_Client/Bannerlord.ButterLib.Implementation.$version.dll $impl/;
Copy-Item $temp/Modules/Bannerlord.ButterLib/bin/Win64_Shipping_Client/Bannerlord.ButterLib.Implementation.$version.pdb $impl/;

dotnet build src/Bannerlord.ButterLib.Implementation/Bannerlord.ButterLib.Implementation.csproj --configuration Stable_Release -p:GameFolder="$path";

Copy-Item $impl/ $path/Modules/Bannerlord.ButterLib/bin/Win64_Shipping_Client/;
