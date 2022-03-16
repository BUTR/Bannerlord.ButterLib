using Bannerlord.ButterLib.ObjectSystem;

using System.Collections.Generic;
using System.Linq;

using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.Implementation.ObjectSystem
{
    internal class MBObjectKeeper : IMBObjectKeeper
    {
        private readonly Dictionary<MBGUID, MBObjectBase> _references = new();

        public void Keep(MBObjectBase mbObject)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (mbObject is null) return;

            _references[mbObject.Id] = mbObject;
        }

        public void Sync(IDataStore dataStore)
        {
            if (dataStore.IsSaving)
            {
                var refs = _references.Values.ToList();
                // We only save the currently populated list, ignoring the previous one.
                // This way we avoid managing the references
                dataStore.SyncData("keepReferences", ref refs);
            }
        }
    }
}