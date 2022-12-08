using System.Numerics;
using System.Runtime.CompilerServices;
using NetCode.Limits;

namespace NetCode;

public static class BitWriterVector3Extensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(this BitWriter writer, Vector3 value)
    {
        writer.Write(value.X);
        writer.Write(value.Y);
        writer.Write(value.Z);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(this BitWriter writer, Vector3 value, Vector3Limit limit)
    {
#if DEBUG
        if (value.X < limit.X.Min 
            || value.X > limit.X.Max
            || value.Y < limit.Y.Min
            || value.Y > limit.Y.Max
            || value.Z < limit.Z.Min
            || value.Z > limit.Z.Max)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException();
        }
#endif
        
        writer.Write(value.X, limit.X);
        writer.Write(value.Y, limit.Y);
        writer.Write(value.Z, limit.Y);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteDiffIfChanged(this BitWriter writer, Vector3 baseline, Vector3 updated)
    {
        var diff = updated - baseline;
        if (diff.Length() < BitWriterFloatExtensions.DefaultFloatPrecision)
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
    public static void WriteDiffIfChanged(this BitWriter writer, Vector3 baseline, Vector3 updated, Vector3Limit diffLimit)
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
    public static void WriteValueIfChanged(this BitWriter writer, Vector3 baseline, Vector3 updated, Vector3Limit limit)
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
    public static void WriteDiffIfChanged(this BitWriter writer, Vector3 baseline, Vector3 updated, Vector3Limit limit, Vector3Limit diffLimit)
    {
        var diff = updated - baseline;
        if (diff.LengthSquared() < limit.PrecisionSquare)
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