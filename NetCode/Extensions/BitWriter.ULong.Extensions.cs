using System.Runtime.CompilerServices;

namespace NetCode;

public static class BitWriterULongExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(this BitWriter writer, ulong value)
    {
        uint low = (uint)value;
        uint high = (uint)(value >> 32);
        writer.Write(high);
        writer.Write(low);
    }        
}