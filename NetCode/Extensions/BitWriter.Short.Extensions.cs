using System.Runtime.CompilerServices;
using NetCode.Limits;

namespace NetCode;

public static class BitWriterShortExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteUnsafe(this BitWriter writer, short value, ShortLimit limit)
    {
        writer.WriteBits(limit.BitCount, (uint)(value - limit.Min));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(this BitWriter writer, short value, ShortLimit limit)
    {
        if (value < limit.Min || value > limit.Max)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException();
        }

        writer.WriteUnsafe(value, limit);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteValueIfChanged(this BitWriter writer, short baseline, short updated)
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
    public static void WriteValueIfChanged(this BitWriter writer, short baseline, short updated, ShortLimit limit)
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
    public static void WriteDiffIfChanged(this BitWriter writer, short baseline, short updated, ShortLimit limit, ShortLimit diffLimit)
    {
        if (baseline.Equals(updated))
        {
            writer.Write(false);
        }
        else
        {
            writer.Write(true);

            var diff = (short)(updated - baseline);

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