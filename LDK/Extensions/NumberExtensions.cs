using System.Numerics;

namespace LDK.Extensions;

public static class NumberExtensions
{
    public static bool IsInRange<Tself>(this Tself number, Tself a, Tself b)
        where Tself : INumber<Tself>
    {
        return number >= a && number <= b;
    }
}
