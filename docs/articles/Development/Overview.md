> [!WARNING]
> This article is an work in progress!
>

### Used Terminology:
* Game Reference Assemblies - 
* Game Stable Version - 
* Game Beta Version - 

### Used Environment Variables:  
* `GITHUB_ACTIONS` - used to indicate that the current environment is a CI.  
* `BANNERLORD_GAME_DIR` - path to the Bannerlord game directory.  
  
There are also two additional game path variables, used in the case when you can setup two game installations - the current Stable game version and the current Beta game version.  
* `BANNERLORD_STABLE_DIR` - points to the Stable game installation.  
* `BANNERLORD_BETA_DIR` - points to the Beta game installation.  
  
An example of such case is to buy the game on `Steam` and `GOG`. `Steam` provides a Beta version, while `GOG` provides the Stable one.  

### Solution
The solution (`Bannerlord.ButterLib.sln`) has 4 configurations available.  
* `Stable Debug`  
* `Stable Release`  
* `Beta Debug`  
* `Beta Release`  
  
They are used to the full potential if you have set up the additional variables `BANNERLORD_STABLE_DIR` and `BANNERLORD_BETA_DIR`.
Basically, when a `Stable` configuration is chosen, the modules will be copied after the project build into `%Stable Game Path%/Modules`.  
Same with `Beta`, will be copied into `%Beta Game Path%/Modules`.  
When those variables are not provided, the `BANNERLORD_GAME_DIR` is used for both. This means that in both configurations the modules will be copied to the same game installations for Stable and Beta game versions.

### supported-game-versions.txt
This is a TXT file that contains all supported game version by ButterLib currently, in specific order:  
* First version entry is the current Beta.  
* Second is the current Stable.  
* Last is the minimal version supported.  
* Everything in-between can be in any order.  
  
The solution parses the file and uses the version to get the right Game Reference Assemblies to build against.