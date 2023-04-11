using System.Runtime.CompilerServices;

namespace NetCode;

public static class BitReaderDoubleExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double ReadDouble(this BitReader reader)
    {
#if NETSTANDARD2_0
        return BitConverterNetstandard20.Int64BitsToDouble(reader.ReadLong());
#else
        return BitConverter.Int64BitsToDouble(reader.ReadLong());
#endif
    }
}