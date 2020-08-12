using TaleWorlds.SaveSystem;

namespace Bannerlord.ButterLib
{
    internal abstract class ButterLibSaveableTypeDefiner : SaveableTypeDefiner
    {
        private const int ButterLibBase = 2002018000;
        protected ButterLibSaveableTypeDefiner(int saveBaseId) : base(ButterLibBase + saveBaseId) { }
        //Reserves:
        //--CampaignIdentifier: 00-09.
    }
}
