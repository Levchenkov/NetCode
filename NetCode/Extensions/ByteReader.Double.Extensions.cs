using System.Runtime.CompilerServices;

namespace NetCode;

public static class ByteReaderDoubleExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double ReadDouble(this ByteReader reader)
    {
#if NETSTANDARD2_0
        return BitConverterNetstandard20.Int64BitsToDouble(reader.ReadLong());
#else
        return BitConverter.Int64BitsToDouble(reader.ReadLong());
#endif
    }
}