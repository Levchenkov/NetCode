using System.Runtime.CompilerServices;

namespace NetCode;

public static class ByteWriterDoubleExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(this ByteWriter writer, double value)
    {
#if NETSTANDARD2_0
        writer.Write(BitConverterNetstandard20.DoubleToInt64Bits(value));
#else
        writer.Write(BitConverter.DoubleToInt64Bits(value));
#endif
    }
}