using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetCode;

public static class BitReaderExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ReadFloat(this BitReader reader)
    {
#if NETSTANDARD2_0
        return BitConverterNetstandard20.Int32BitsToSingle(reader.ReadInt());
#else
        return BitConverter.Int32BitsToSingle(reader.ReadInt());
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ReadVector3(this BitReader reader)
    {
        return new Vector3(reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat());
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ReadInt(this BitReader reader, int min, int max)
    {
        if (min > max)
        {
            ThrowHelper.ThrowArgumentException();
        }
        
        var range = max - min;
        var bitsRequired = Mathi.BitsRequired((uint)range);
        var value = (int)reader.ReadBits(bitsRequired);
        return value + min;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ReadFloat(this BitReader reader, float min, float max, float precision)
    {
        if (min > max)
        {
            ThrowHelper.ThrowArgumentException();
        }
        
        var result = reader.ReadFloat(new FloatLimit(min, max, precision));

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ReadFloat(this BitReader reader, FloatLimit limit)
    {
        uint integerValue = reader.ReadBits(limit.NumberOfBits);
        float normalizedValue = integerValue / (float)limit.MaxIntegerValue;

        return normalizedValue * limit.Delta + limit.Min;
    }
}