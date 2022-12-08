using System.Runtime.CompilerServices;
using NetCode.Limits;

namespace NetCode;

public static class BitWriterFloatExtensions
{
    public const float DefaultFloatPrecision = 0.0000001f;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(this BitWriter writer, float value)
    {
#if NETSTANDARD2_0
        writer.Write(BitConverterNetstandard20.SingleToInt32Bits(value));
#else
        writer.Write(BitConverter.SingleToInt32Bits(value));
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(this BitWriter writer, float value, float min, float max, float precision)
    {
#if DEBUG
        if (value < min || value > max)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException();
        }
#endif
        
        writer.Write(value, new FloatLimit(min, max, precision));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(this BitWriter writer, float value, FloatLimit limit)
    {
#if DEBUG
        if (value < limit.Min || value > limit.Max)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException();
        }
#endif

        float normalizedValue = Mathf.Clamp((value - limit.Min) / limit.Delta, 0, 1);
        uint integerValue = (uint)Math.Floor(normalizedValue * limit.MaxIntegerValue + 0.5f);

        writer.WriteBits(limit.BitCount, integerValue);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteDiffIfChanged(this BitWriter writer, float baseline, float updated)
    {
        var diff = updated - baseline;
        if (Math.Abs(diff) < DefaultFloatPrecision)
        {
            writer.Write(false);
        }
        else
        {
            writer.Write(true);
            writer.Write(diff);
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteDiffIfChanged(this BitWriter writer, float baseline, float updated, FloatLimit diffLimit)
    {
        var diff = updated - baseline;
        if (Math.Abs(diff) < diffLimit.Precision)
        {
            writer.Write(false);
        }
        else
        {
            writer.Write(true);
            writer.Write(diff, diffLimit);
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteValueIfChanged(this BitWriter writer, float baseline, float updated, FloatLimit limit)
    {
        if (Math.Abs(updated - baseline) < limit.Precision)
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
    public static void WriteDiffIfChanged(this BitWriter writer, float baseline, float updated, FloatLimit limit, FloatLimit diffLimit)
    {
        var diff = updated - baseline;
        if (Math.Abs(diff) < limit.Precision)
        {
            writer.Write(false);
        }
        else
        {
            writer.Write(true);

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