using System.Runtime.CompilerServices;

namespace NetCode;

public static class BitWriterLongExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(this BitWriter writer, long value)
    {
        uint low = (uint)value;
        uint high = (uint)(value >> 32);
        writer.Write(high);
        writer.Write(low);
    }        
}