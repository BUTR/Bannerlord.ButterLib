using Bannerlord.ButterLib.Implementation.Common.Extensions;
using Bannerlord.ButterLib.Implementation.Common.Helpers;

using System.Linq;
using System.Reflection;

using TaleWorlds.Core;
using TaleWorlds.Library;

namespace Bannerlord.ButterLib.Implementation
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
                // Do not rely on DI at this stage
                var impl = new ApplicationVersionUtilsImplementation();
                var ext = new ApplicationVersionExtensionsImplementation();
                var gameVersion = impl.TryParse(impl.GameVersionStr(), out var v) ? v : (ApplicationVersion?) null;
                var metadatas = typeof(SubModule).Assembly.GetCustomAttributes<AssemblyMetadataAttribute>();
                var supportedVersionStr = metadatas.FirstOrDefault(a => a.Key == "GameVersion")?.Value;
                var supportedVersion = !string.IsNullOrEmpty(supportedVersionStr) && impl.TryParse(supportedVersionStr, out var sv)
                    ? sv
                    : (ApplicationVersion?) null;
                return gameVersion != null && supportedVersion != null && ext.IsSameWithoutRevision(gameVersion.Value, supportedVersion.Value);
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