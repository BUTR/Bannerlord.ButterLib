using TaleWorlds.SaveSystem;

namespace Bannerlord.ButterLib
{
    /// <summary>
    /// Base class of ButterLib's saving system.
    /// </summary>
    public abstract class ButterLibSaveableTypeDefiner : SaveableTypeDefiner
    {
        // 2 000 000 000 + 2018 (NexusMods Id) * 1000
        private const int ButterLibBase = 2002018000;
        protected ButterLibSaveableTypeDefiner(int saveBaseId) : base(ButterLibBase + saveBaseId) { }
        //Reserves:
        //--CampaignIdentifier: 00-04.
    }
}