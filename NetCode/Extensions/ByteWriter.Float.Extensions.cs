using System.Runtime.CompilerServices;

namespace NetCode;

public static class ByteWriterFloatExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(this ByteWriter writer, float value)
    {
#if NETSTANDARD2_0
        writer.Write(BitConverterNetstandard20.SingleToInt32Bits(value));
#else
        writer.Write(BitConverter.SingleToInt32Bits(value));
#endif
    }
}