using System.Numerics;
using System.Runtime.CompilerServices;

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
        if (min > max)
        {
            ThrowHelper.ThrowArgumentException();
        }

        if (value < min || value > max)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException();
        }
        
        var range = max - min;
        var bitsRequired = Mathi.BitsRequired((uint)range);
        writer.WriteBits(bitsRequired, (uint)(value - min));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(this BitWriter writer, uint value, uint min, uint max)
    {
        if (min > max)
        {
            ThrowHelper.ThrowArgumentException();
        }

        if (value < min || value > max)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException();
        }
        
        var range = max - min;
        var bitsRequired = Mathi.BitsRequired(range);
        writer.WriteBits(bitsRequired, value - min);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(this BitWriter writer, float value, float min, float max, float precision)
    {
        if (min > max)
        {
            ThrowHelper.ThrowArgumentException();
        }

        if (value < min || value > max)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException();
        }
        
        writer.Write(value, new FloatLimit(min, max, precision));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(this BitWriter writer, float value, FloatLimit limit)
    {
        float normalizedValue = Mathf.Clamp((value - limit.Min) / limit.Delta, 0, 1);
        uint integerValue = (uint)Math.Floor(normalizedValue * limit.MaxIntegerValue + 0.5f);

        writer.WriteBits(limit.NumberOfBits, integerValue);
    }
}