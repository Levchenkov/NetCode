using System.Runtime.CompilerServices;
using NetCode.Limits;

namespace NetCode;

public static class BitReaderUIntExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint ReadUInt(this BitReader reader, uint min, uint max) => reader.ReadUInt(new UIntLimit(min, max));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint ReadUInt(this BitReader reader, UIntLimit limit)
    {
        var value = reader.ReadBits(limit.BitCount);
        return value + limit.Min;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint ReadUInt(this BitReader reader, uint baseline)
    {
        var isChanged = reader.ReadBool();
        if (isChanged)
        {
            return reader.ReadUInt();
        }

        return baseline;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint ReadUInt(this BitReader reader, uint baseline, UIntLimit limit)
    {
        var isChanged = reader.ReadBool();
        if (isChanged)
        {
            return reader.ReadUInt(limit);
        }

        return baseline;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint ReadUInt(this BitReader reader, uint baseline, UIntLimit limit, UIntLimit diffLimit)
    {
        var isChanged = reader.ReadBool();
        if (isChanged)
        {
            var isDiff = reader.ReadBool();

            if (isDiff)
            {
                var diff = reader.ReadUInt(diffLimit);
                return baseline + diff;
            }

            return reader.ReadUInt(limit);
        }

        return baseline;
    }
}