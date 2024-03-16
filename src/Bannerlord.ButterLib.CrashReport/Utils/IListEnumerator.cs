using HonkPerf.NET.RefLinq.Enumerators;

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Bannerlord.ButterLib.CrashReportWindow.Utils;

public struct IListEnumerator<T> : IRefEnumerable<T>
{
    private readonly IList<T> _list;
    private int _curr;

    public IListEnumerator(IList<T> list)
    {
        _list = list;
        _curr = -1;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool MoveNext()
    {
        _curr++;
        return _curr < _list.Count;
    }

    public T Current => _list[_curr];
}