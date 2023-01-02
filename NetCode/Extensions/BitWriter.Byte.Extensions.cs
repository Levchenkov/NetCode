using System.Runtime.CompilerServices;
using NetCode.Limits;

namespace NetCode;

public static class BitWriterByteExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(this BitWriter writer, byte value, ByteLimit limit)
    {
#if DEBUG
        if (value < limit.Min || value > limit.Max)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException();
        }
#endif
        
        writer.WriteBits(limit.BitCount, (uint)(value - limit.Min));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteValueIfChanged(this BitWriter writer, byte baseline, byte updated)
    {
        if (baseline == updated)
        {
            writer.Write(false);
        }
        else
        {
            writer.Write(true);
            writer.Write(updated);
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteValueIfChanged(this BitWriter writer, byte baseline, byte updated, ByteLimit limit)
    {
        if (baseline.Equals(updated))
        {
            writer.Write(false);
        }
        else
        {
            writer.Write(true);
            writer.Write(updated, limit);
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteDiffIfChanged(this BitWriter writer, byte baseline, byte updated, ByteLimit limit, ByteLimit diffLimit)
    {
        if (baseline.Equals(updated))
        {
            writer.Write(false);
        }
        else
        {
            writer.Write(true);
            
            var diff = (byte)(updated - baseline);

            if (diffLimit.Min < diff && diff < diffLimit.Max)
            {
                // if diff inside of diff limit, then we will write diff
                writer.Write(true);
                writer.Write(diff, diffLimit);
            }
            else
            {
                // otherwise we will write updated value
                writer.Write(false);
                writer.Write(updated, limit);
            }
        }
    }
}