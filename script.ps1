        $impl = $PWD.Path + '/bannerlord-implementations';
        $path = $PWD.Path + '/bannerlord';
        $final = $path + '/Modules/Bannerlord.ButterLib/bin/Win64_Shipping_Client/';

        $proj = 'src/Bannerlord.ButterLib.Implementation/Bannerlord.ButterLib.Implementation.csproj';
        $pdll = $path + '/Modules/Bannerlord.ButterLib/bin/Win64_Shipping_Client/Bannerlord.ButterLib.Implementation.*.dll';
        $ppdb = $path + '/Modules/Bannerlord.ButterLib/bin/Win64_Shipping_Client/Bannerlord.ButterLib.Implementation.*.pdb';

        # The folders are required to be created before executing the script
        New-Item -ItemType directory -Force -Path $impl;
        New-Item -ItemType directory -Force -Path $path;

        $gameversions = Get-Content -Path supported-game-versions.txt;
        # Get all implementations except the minimal version (last element)
        # Last entry is the Minimal version. We will use Bannerlord.ButterLib.dll from there
        For ($i = 0; $i -le $gameversions.Length - 1; $i++)
        {
            $gameversion = $gameversions[$i];
            $version = $gameversion.substring(1);
            dotnet clean $proj --configuration Release;
            dotnet build $proj --configuration Release -p:OverrideGameVersion=$gameversion -p:GameFolder="$path";

            # Copy Implementations to the Implementations folder
            Copy-Item $pdll $impl/;
            Copy-Item $ppdb $impl/;
        }

        # Copy Implementations to the Module
        Copy-Item $impl/* $final;

        # Delete Implementations folder
        Remove-Item -Recurse $impl;