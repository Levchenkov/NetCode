using System.Runtime.CompilerServices;

namespace NetCode;

#if NETSTANDARD2_0

public static class BitConverterNetstandard20
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe int SingleToInt32Bits(float value) => *((int*)&value);

    /// <summary>
    /// Converts the specified 32-bit unsigned integer to a single-precision floating point number.
    /// </summary>
    /// <param name="value">The number to convert.</param>
    /// <returns>A single-precision floating point number whose bits are identical to <paramref name="value"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe float Int32BitsToSingle(int value) => *((float*)&value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe long DoubleToInt64Bits(double value) => *((long*)&value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe double Int64BitsToDouble(long value) => *((double*)&value);
}

#endif