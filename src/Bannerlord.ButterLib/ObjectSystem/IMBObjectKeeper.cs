using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.ObjectSystem
{
    /// <summary>
    /// Keeps all MBObjectBase references for custom serialization
    /// </summary>
    public interface IMBObjectKeeper
    {
        void Keep(MBObjectBase mbObject);
        void Sync(IDataStore dataStore);
    }
}