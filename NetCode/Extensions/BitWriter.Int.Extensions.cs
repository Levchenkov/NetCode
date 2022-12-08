using System.Numerics;
using System.Runtime.CompilerServices;
using NetCode.Limits;

namespace NetCode;

public static class BitWriterIntExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(this BitWriter writer, int value, int min, int max)
    {
#if DEBUG
        if (value < min || value > max)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException();
        }
#endif
        
        writer.Write(value, new IntLimit(min, max));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(this BitWriter writer, int value, IntLimit limit)
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
    public static void WriteDiffIfChanged(this BitWriter writer, int baseline, int updated)
    {
        if (baseline.Equals(updated))
        {
            writer.Write(false);
        }
        else
        {
            writer.Write(true);
            
            var diff = updated - baseline;
            writer.Write(diff);
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteValueIfChanged(this BitWriter writer, int baseline, int updated)
    {
        if (baseline.Equals(updated))
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
    public static void WriteDiffIfChanged(this BitWriter writer, int baseline, int updated, IntLimit diffLimit)
    {
        if (baseline.Equals(updated))
        {
            writer.Write(false);
        }
        else
        {
            writer.Write(true);
            
            var diff = updated - baseline;
            writer.Write(diff, diffLimit);
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteValueIfChanged(this BitWriter writer, int baseline, int updated, IntLimit limit)
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
    public static void WriteDiffIfChanged(this BitWriter writer, int baseline, int updated, IntLimit limit, IntLimit diffLimit)
    {
        if (baseline.Equals(updated))
        {
            writer.Write(false);
        }
        else
        {
            writer.Write(true);
            
            var diff = updated - baseline;

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