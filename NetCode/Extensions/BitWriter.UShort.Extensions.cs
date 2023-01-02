using System.Runtime.CompilerServices;
using NetCode.Limits;

namespace NetCode;

public static class BitWriterUShortExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(this BitWriter writer, ushort value, UShortLimit limit)
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
    public static void WriteValueIfChanged(this BitWriter writer, ushort baseline, ushort updated)
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
    public static void WriteValueIfChanged(this BitWriter writer, ushort baseline, ushort updated, UShortLimit limit)
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
    public static void WriteDiffIfChanged(this BitWriter writer, ushort baseline, ushort updated, UShortLimit limit, UShortLimit diffLimit)
    {
        if (baseline.Equals(updated))
        {
            writer.Write(false);
        }
        else
        {
            writer.Write(true);
            
            var diff = (ushort)(updated - baseline);

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