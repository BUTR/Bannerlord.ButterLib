using Bannerlord.ButterLib.Common.Helpers;

using System.Linq;
using System.Reflection;

using TaleWorlds.Core;
using TaleWorlds.Library;

namespace Bannerlord.ButterLib
{
    /// <summary>
    /// Prevents the game from initializing the library if it was compiled for the wrong version.
    /// </summary>
    public class SubModuleProtector : SubModule
    {
        private static bool IsCorrectBranch
        {
            get
            {
                var gameVersion = ApplicationVersionUtils.GameVersion();
                var metadatas = typeof(SubModule).Assembly.GetCustomAttributes<AssemblyMetadataAttribute>();
                var supportedVersionStr = metadatas.FirstOrDefault(a => a.Key == "GameVersion")?.Value;
                var supportedVersion = !string.IsNullOrEmpty(supportedVersionStr) && ApplicationVersionUtils.TryParse(supportedVersionStr, out var sv)
                    ? sv
                    : (ApplicationVersion?) null;
                return gameVersion != null && supportedVersion != null && gameVersion.Value.IsSameWithoutRevision(supportedVersion.Value);
            }
        }

        protected override void OnSubModuleLoad()
        {
            if (!IsCorrectBranch)
                return;

            base.OnSubModuleLoad();
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            if (!IsCorrectBranch)
                return;

            base.OnBeforeInitialModuleScreenSetAsRoot();
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            if (!IsCorrectBranch)
                return;

            base.OnGameStart(game, gameStarterObject);
        }
    }
}