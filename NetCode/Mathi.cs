using System.Runtime.CompilerServices;

namespace NetCode;

public static class Mathi
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Ceiling(int a, int b)
    {
        var (quotient, remainder) = Math.DivRem(a, b);
        if (remainder == 0)
        {
            return quotient;
        }

        return quotient + 1;
    }
}