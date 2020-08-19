using System;
using System.Collections.Generic;

using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.DistanceMatrix
{
    internal abstract class DistanceMatrixStatic<T> where T : MBObjectBase
    {
        public abstract DistanceMatrix<T> Create();
        public abstract DistanceMatrix<T> Create(Func<IEnumerable<T>> customListGetter, Func<T, T, float> customDistanceCalculator);
    }
}