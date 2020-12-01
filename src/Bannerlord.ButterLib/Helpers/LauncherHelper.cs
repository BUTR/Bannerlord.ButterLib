using Bannerlord.ButterLib.Common.Helpers;
using Bannerlord.ButterLib.Helpers.ModuleInfo;

using System;
using System.Linq;

using TaleWorlds.MountAndBlade.Launcher.UserDatas;

namespace Bannerlord.ButterLib.Helpers
{
    internal static class LauncherHelper
    {
        public static void FixLoadingOrder()
        {
            var existingModules = ExtendedModuleInfo.GetExtendedModules().ToArray();

            var userDataManager = new UserDataManager();
            userDataManager.LoadUserData();
            var userData = userDataManager.UserData;
            var userGameTypeData = userData.SingleplayerData;

            var modules = userGameTypeData.ModDatas.Select(md =>
            {
                var result = Array.Find(existingModules, m => md.Id == m.Id);
                if (result is not null)
                    result.IsSelected = md.IsSelected;
                return result;
            }).Where(m => m is not null).Cast<ExtendedModuleInfo>().ToArray();
            var sortedModules = ModuleSorter.Sort(modules);

            userGameTypeData.ModDatas.Clear();
            userGameTypeData.ModDatas.AddRange(sortedModules.Select(m => new UserModData(m.Id, m.IsSelected)));
            userDataManager.SaveUserData();
        }
    }
}