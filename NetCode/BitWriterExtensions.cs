using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetCode;

public static class BitWriterExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(this BitWriter writer, Vector3 value)
    {
        writer.Write(value.X);
        writer.Write(value.Y);
        writer.Write(value.Z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(this BitWriter writer, float value)
    {
#if NETSTANDARD2_0
        writer.Write(BitConverterNetstandard20.SingleToInt32Bits(value));
#else
        writer.Write(BitConverter.SingleToInt32Bits(value));
#endif
    }
}