using System.Numerics;
using System.Runtime.CompilerServices;
using NetCode.Limits;

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
    public static int ReadInt(this BitReader reader, int min, int max) => reader.ReadInt(new IntLimit(min, max));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ReadInt(this BitReader reader, IntLimit limit)
    {
        var value = (int)reader.ReadBits(limit.BitCount);
        return value + limit.Min;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint ReadUInt(this BitReader reader, uint min, uint max) => reader.ReadUInt(new UIntLimit(min, max));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint ReadUInt(this BitReader reader, UIntLimit limit)
    {
        var value = reader.ReadBits(limit.BitCount);
        return value + limit.Min;
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
    public static Vector3 ReadVector3(this BitReader reader, Vector3Limit limit)
    {
        return new Vector3(reader.ReadFloat(limit.X), reader.ReadFloat(limit.Y), reader.ReadFloat(limit.Z));
    }
}