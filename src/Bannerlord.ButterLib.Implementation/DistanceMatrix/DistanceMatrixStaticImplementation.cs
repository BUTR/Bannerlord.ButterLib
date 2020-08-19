using Bannerlord.ButterLib.DistanceMatrix;

using System;
using System.Collections.Generic;

using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.Implementation.DistanceMatrix
{
    internal sealed class DistanceMatrixStaticImplementation<T> : DistanceMatrixStatic<T> where T : MBObjectBase
    {
        public override DistanceMatrix<T> Create() => new DistanceMatrixImplementation<T>();

        public override DistanceMatrix<T> Create(Func<IEnumerable<T>> customListGetter, Func<T, T, float> customDistanceCalculator) =>
            new DistanceMatrixImplementation<T>(customListGetter, customDistanceCalculator);
    }
}
