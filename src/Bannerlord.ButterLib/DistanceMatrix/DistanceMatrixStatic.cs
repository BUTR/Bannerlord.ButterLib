using System;
using System.Collections.Generic;

using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.DistanceMatrix
{
    internal abstract class DistanceMatrixStatic
    {
        public abstract DistanceMatrix<T> Create<T>()
            where T : MBObjectBase;
        public abstract DistanceMatrix<T> Create<T>(Func<IEnumerable<T>> customListGetter, Func<T, T, float> customDistanceCalculator)
            where T : MBObjectBase;

        public abstract float CalculateDistanceBetweenHeroes(Hero hero1, Hero hero2);

        public abstract float CalculateDistanceBetweenClans(Clan clan1, Clan clan2, IEnumerable<(ulong Owners, float Distance, float Weight)> settlementOwnersPairedList);

        public abstract float CalculateDistanceBetweenKingdoms(Kingdom kingdom1, Kingdom kingdom2, DistanceMatrix<Settlement> settlementDistanceMatrix);

        public abstract List<(ulong Owners, float Distance, float Weight)> GetSettlementOwnersPairedList(DistanceMatrix<Settlement> settlementDistanceMatrix);
    }
}