using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.GameUtility;

public class ArrayComparer : IEqualityComparer<int[]>
{
    public bool Equals(int[]? x, int[]? y)
    {
        if (x != null && y != null)
           return x.SequenceEqual(y) || x.SequenceEqual(y);
        return false;
    }

    public int GetHashCode([DisallowNull] int[] obj)
    {
        return String.Join(",", obj).GetHashCode();
    }
}
