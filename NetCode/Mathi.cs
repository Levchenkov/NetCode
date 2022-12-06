using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetCode;

public static class Mathi
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Ceiling(int a, int b)
    {
        var (quotient, remainder) = DivRem(a, b);
        if (remainder == 0)
        {
            return quotient;
        }

        return quotient + 1;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (int Quotient, int Remainder) DivRem(int left, int right)
    {
        int quotient = left / right;
        return (quotient, left - (quotient * right));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int BitsRequired(uint range)
    {
        return range == 0 ? 1 : BitOperations.Log2(range) + 1;
    }
}