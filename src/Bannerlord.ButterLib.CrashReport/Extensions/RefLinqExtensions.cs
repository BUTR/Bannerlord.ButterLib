using Bannerlord.ButterLib.CrashReportWindow.Utils;

using HonkPerf.NET.RefLinq.Enumerators;

using System.Collections.Generic;

namespace Bannerlord.ButterLib.CrashReportWindow.Extensions;

public static class RefLinqExtensions
{
    public static RefLinqEnumerable<T, IListEnumerator<T>> ToRefLinq<T>(this IList<T> c) => new(new(c));
}