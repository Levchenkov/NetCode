using System.Runtime.CompilerServices;
using NetCode.Limits;

namespace NetCode;

public static class BitReaderFloatExtensions
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
    public static float ReadFloat(this BitReader reader, float min, float max, float precision)
    {
        var result = reader.ReadFloat(new FloatLimit(min, max, precision));
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ReadFloat(this BitReader reader, FloatLimit limit)
    {
        uint integerValue = reader.ReadBits(limit.BitCount);
        float normalizedValue = integerValue / (float)limit.MaxIntegerValue;

        return normalizedValue * limit.Delta + limit.Min;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ReadFloat(this BitReader reader, float baseline)
    {
        var isChanged = reader.ReadBool();
        if (isChanged)
        {
            return reader.ReadFloat();
        }

        return baseline;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ReadFloat(this BitReader reader, float baseline, FloatLimit limit)
    {
        var isChanged = reader.ReadBool();
        if (isChanged)
        {
            return reader.ReadFloat(limit);
        }

        return baseline;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ReadFloat(this BitReader reader, float baseline, FloatLimit limit, FloatLimit diffLimit)
    {
        var isChanged = reader.ReadBool();
        if (isChanged)
        {
            var isDiff = reader.ReadBool();

            if (isDiff)
            {
                var diff = reader.ReadFloat(diffLimit);
                return baseline + diff;
            }

            return reader.ReadFloat(limit);
        }

        return baseline;
    }
}