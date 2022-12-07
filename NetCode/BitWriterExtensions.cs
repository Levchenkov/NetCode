using System.Numerics;
using System.Runtime.CompilerServices;
using NetCode.Limits;

namespace NetCode;

public static class BitWriterExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(this BitWriter writer, Vector3 value)
    {
        writer.Write(value.X);
        writer.Write(value.Y);
        writer.Write(value.Z);
    }

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
    public static void Write(this BitWriter writer, int value, int min, int max)
    {
        if (value < min || value > max)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException();
        }
        
        writer.Write(value, new IntLimit(min, max));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(this BitWriter writer, int value, IntLimit limit)
    {
        if (value < limit.Min || value > limit.Max)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException();
        }
        writer.WriteBits(limit.BitCount, (uint)(value - limit.Min));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(this BitWriter writer, uint value, uint min, uint max)
    {
        if (value < min || value > max)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException();
        }
        
        writer.Write(value, new UIntLimit(min, max));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(this BitWriter writer, uint value, UIntLimit limit)
    {
        if (value < limit.Min || value > limit.Max)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException();
        }
        writer.WriteBits(limit.BitCount, value - limit.Min);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(this BitWriter writer, float value, float min, float max, float precision)
    {
        if (value < min || value > max)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException();
        }
        
        writer.Write(value, new FloatLimit(min, max, precision));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(this BitWriter writer, float value, FloatLimit limit)
    {
        if (value < limit.Min || value > limit.Max)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException();
        }
        
        float normalizedValue = Mathf.Clamp((value - limit.Min) / limit.Delta, 0, 1);
        uint integerValue = (uint)Math.Floor(normalizedValue * limit.MaxIntegerValue + 0.5f);

        writer.WriteBits(limit.BitCount, integerValue);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(this BitWriter writer, Vector3 value, Vector3Limit limit)
    {
        if (value.X < limit.X.Min 
            || value.X > limit.X.Max
            || value.Y < limit.Y.Min
            || value.Y > limit.Y.Max
            || value.Z < limit.Z.Min
            || value.Z > limit.Z.Max)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException();
        }
        
        writer.Write(value.X, limit.X);
        writer.Write(value.Y, limit.Y);
        writer.Write(value.Z, limit.Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteDiff(this BitWriter writer, int baseline, int updated)
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
    public static void WriteDiff(this BitWriter writer, int baseline, int updated, IntLimit diffLimit)
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
    public static void WriteDiff(this BitWriter writer, uint baseline, uint updated)
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
    public static void WriteDiff(this BitWriter writer, uint baseline, uint updated, UIntLimit diffLimit)
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
    
    public const float DefaultFloatPrecision = 0.0000001f;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteDiff(this BitWriter writer, float baseline, float updated)
    {
        if (Math.Abs(baseline - updated) < DefaultFloatPrecision)
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
    public static void WriteDiff(this BitWriter writer, float baseline, float updated, FloatLimit diffLimit)
    {
        if (Math.Abs(baseline - updated) < diffLimit.Precision)
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
    public static void WriteDiff(this BitWriter writer, Vector3 baseline, Vector3 updated)
    {
        var diff = updated - baseline;
        if (diff.Length() < DefaultFloatPrecision)
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
    public static void WriteDiff(this BitWriter writer, Vector3 baseline, Vector3 updated, Vector3Limit diffLimit)
    {
        var diff = updated - baseline;
        if (diff.Length() < diffLimit.Precision)
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
    public static void WriteIfChanged(this BitWriter writer, Vector3 baseline, Vector3 updated, Vector3Limit limit)
    {
        var diff = updated - baseline;
        if (diff.Length() < limit.Precision)
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
    public static void WriteDiff(this BitWriter writer, Vector3 baseline, Vector3 updated, Vector3Limit limit, Vector3Limit diffLimit)
    {
        var diff = updated - baseline;
        if (diff.Length() < limit.Precision)
        {
            writer.Write(false);
        }
        else
        {
            writer.Write(true);

            if ((diffLimit.X.Min < diff.X && diff.X < diffLimit.X.Max)
                && (diffLimit.Y.Min < diff.Y && diff.Y < diffLimit.Y.Max)
                && (diffLimit.Z.Min < diff.Z && diff.Z < diffLimit.Z.Max))
            {
                // is diff inside of diff limit, then we will use diffLimit
                writer.Write(true);
                writer.Write(diff, diffLimit);
            }
            else
            {
                writer.Write(false);
                writer.Write(updated, limit);
            }
        }
    }
}