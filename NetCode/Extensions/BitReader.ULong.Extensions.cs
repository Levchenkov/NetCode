using System.Runtime.CompilerServices;

namespace NetCode;

public static class BitReaderULongExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong ReadULong(this BitReader reader)
    {
        uint high = reader.ReadUInt();
        uint low = reader.ReadUInt();
        
        ulong value = high;
        value = value << 32;
        value = value | low;
        
        return value;
    }
}