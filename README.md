# Bannerlord.ButterLib
<p align="center">
  <a href="https://github.com/BUTR/Bannerlord.ButterLib" alt="Logo">
  <img src="https://github.com/BUTR/Bannerlord.ButterLib/blob/dev/resources/Butter.png?raw=true" /></a>
  </br>
  <a href="https://github.com/BUTR/Bannerlord.ButterLib" alt="Lines Of Code">
  <img src="https://tokei.rs/b1/github/BUTR/Bannerlord.ButterLib?category=code" /></a>
  <a href="https://www.codefactor.io/repository/github/butr/bannerlord.butterlib"><img src="https://www.codefactor.io/repository/github/butr/bannerlord.butterlib/badge" alt="CodeFactor" /></a>
  </br>
  <a href="https://github.com/BUTR/Bannerlord.ButterLib/actions?query=workflow%3ATest"><img src="https://github.com/BUTR/Bannerlord.ButterLib/workflows/Test/badge.svg?branch=dev&event=push" alt="Test" /></a>
  <a href="https://codecov.io/gh/BUTR/Bannerlord.ButterLib"><img src="https://codecov.io/gh/BUTR/Bannerlord.ButterLib/branch/dev/graph/badge.svg" />
   </a>
  </br>
  <a href="https://www.nuget.org/packages/Bannerlord.ButterLib" alt="NuGet Bannerlord.ButterLib">
  <img src="https://img.shields.io/nuget/v/Bannerlord.ButterLib.svg?label=NuGet%20Bannerlord.ButterLib&colorB=blue" /></a>
  <a href="https://butr.github.io/Bannerlord.ButterLib" alt="Documentation">
  <img src="https://img.shields.io/badge/Documentation-%F0%9F%94%8D-blue?style=flat" /></a>
  </br>
  <a href="https://www.nexusmods.com/mountandblade2bannerlord/mods/2018" alt="Nexus ButterLib">
  <img src="https://img.shields.io/badge/Nexus-ButterLib-yellow.svg" /></a>
  <a href="https://www.nexusmods.com/mountandblade2bannerlord/mods/2018" alt="ButterLib">
  <img src="https://img.shields.io/endpoint?url=https%3A%2F%2Fnexusmods-version-pzk4e0ejol6j.runkit.sh%3FgameId%3Dmountandblade2bannerlord%26modId%3D2018" /></a>
  <a href="https://www.nexusmods.com/mountandblade2bannerlord/mods/2018" alt="Nexus ButterLib">
  <img src="https://img.shields.io/endpoint?url=https%3A%2F%2Fnexusmods-downloads-ayuqql60xfxb.runkit.sh%2F%3Ftype%3Dunique%26gameId%3D3174%26modId%3D2018" /></a>
  <a href="https://www.nexusmods.com/mountandblade2bannerlord/mods/2018" alt="Nexus ButterLib">
  <img src="https://img.shields.io/endpoint?url=https%3A%2F%2Fnexusmods-downloads-ayuqql60xfxb.runkit.sh%2F%3Ftype%3Dtotal%26gameId%3D3174%26modId%3D2018" /></a>
  <a href="https://www.nexusmods.com/mountandblade2bannerlord/mods/2018" alt="Nexus ButterLib">
  <img src="https://img.shields.io/endpoint?url=https%3A%2F%2Fnexusmods-downloads-ayuqql60xfxb.runkit.sh%2F%3Ftype%3Dviews%26gameId%3D3174%26modId%3D2018" /></a>
  </br>
</p>

Extension library for Mount & Blade II: Bannerlord.

## Highlighted features:
* [CampaignIdentifier](https://butr.github.io/Bannerlord.ButterLib/articles/CampaignIdentifier/Overview.html) - Associates unique string ID with every campaign based on the initial character.  
* [DistanceMatrix](https://butr.github.io/Bannerlord.ButterLib/articles/DistanceMatrix/Overview.html) - A generic class that pairs given objects of type MBObject and for each pair calculates the distance between the objects that formed it.  
* [DelayedSubModule](https://butr.github.io/Bannerlord.ButterLib/articles/DelayedSubModule/Overview.html) - Execute code after specific SubModule method.  
* [SubModuleWrappers](https://butr.github.io/Bannerlord.ButterLib/articles/SubModuleWrappers/Overview.html) - Wraps MBSubModulebase for easier calling of protected internal metods. 
* [SaveSystem](https://butr.github.io/Bannerlord.ButterLib/articles/SaveSystem/Overview.html) - Provides helper methods for the game's save system.
* [AccessTools2](https://butr.github.io/Bannerlord.ButterLib/api/Bannerlord.ButterLib.Common.Helpers.AccessTools2.html) - Adds delegate related functions.  
* [SymbolExtensions2](https://butr.github.io/Bannerlord.ButterLib/api/Bannerlord.ButterLib.Common.Helpers.SymbolExtensions2.html) - More lambda functions. 
* [AlphanumComparatorFast](https://butr.github.io/Bannerlord.ButterLib/api/Bannerlord.ButterLib.Common.Helpers.AlphanumComparatorFast.html) - Alphanumeric sort. This sorting algorithm logically handles numbers in strings.  

Check the [/Articles](https://butr.github.io/Bannerlord.ButterLib/articles/index.html) section in the documentation to see all available features!

## Installation
This module should be one of the highest in loading order. Ideally, it should be second in load order after ``Bannerlord.Harmony``.

## For Players
This mod is a dependency mod that does not provide anything by itself. You need to additionaly install mods that use it.
