using System.Runtime.CompilerServices;

namespace NetCode;

public static class BitWriterDoubleExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(this BitWriter writer, double value)
    {
#if NETSTANDARD2_0
        writer.Write(BitConverterNetstandard20.DoubleToInt64Bits(value));
#else
        writer.Write(BitConverter.DoubleToInt64Bits(value));
#endif
    }        
}