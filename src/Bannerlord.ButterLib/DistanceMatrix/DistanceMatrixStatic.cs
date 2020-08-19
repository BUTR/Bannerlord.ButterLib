using System;
using System.Collections.Generic;

using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.DistanceMatrix
{
    internal abstract class DistanceMatrixStatic<T> where T : MBObjectBase
    {
        public abstract DistanceMatrix<T> Create();
        public abstract DistanceMatrix<T> Create(Func<IEnumerable<T>> customListGetter, Func<T, T, float> customDistanceCalculator);

        public abstract float CalculateDistanceBetweenHeroes(Hero hero1, Hero hero2);

        public abstract float CalculateDistanceBetweenClans(Clan clan1, Clan clan2, List<(ulong owners, float distance, float weight)> settlementOwnersPairedList);

        public abstract float CalculateDistanceBetweenKingdoms(Kingdom kingdom1, Kingdom kingdom2, DistanceMatrix<Settlement> settlementDistanceMatrix);

        public abstract List<(ulong owners, float distance, float weight)> GetSettlementOwnersPairedList(DistanceMatrix<Settlement> settlementDistanceMatrix);
    }
}