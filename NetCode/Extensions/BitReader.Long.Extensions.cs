using System.Runtime.CompilerServices;

namespace NetCode;

public static class BitReaderLongExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long ReadLong(this BitReader reader)
    {
        uint high = reader.ReadUInt();
        uint low = reader.ReadUInt();
        
        long value = high;
        value = value << 32;
        value = value | low;
        
        return value;
    }
}