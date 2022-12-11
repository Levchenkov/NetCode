using System.Runtime.CompilerServices;
using NetCode.Limits;

namespace NetCode;

public static class BitReaderIntExtensions
{    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ReadInt(this BitReader reader, int min, int max) => reader.ReadInt(new IntLimit(min, max));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ReadInt(this BitReader reader, IntLimit limit)
    {
        var value = (int)reader.ReadBits(limit.BitCount);
        return value + limit.Min;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ReadInt(this BitReader reader, int baseline)
    {
        var isChanged = reader.ReadBool();
        if (isChanged)
        {
            return reader.ReadInt();
        }

        return baseline;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ReadInt(this BitReader reader, int baseline, IntLimit limit)
    {
        var isChanged = reader.ReadBool();
        if (isChanged)
        {
            return reader.ReadInt(limit);
        }

        return baseline;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ReadInt(this BitReader reader, int baseline, IntLimit limit, IntLimit diffLimit)
    {
        var isChanged = reader.ReadBool();
        if (isChanged)
        {
            var isDiff = reader.ReadBool();

            if (isDiff)
            {
                var diff = reader.ReadInt(diffLimit);
                return baseline + diff;
            }

            return reader.ReadInt(limit);
        }

        return baseline;
    }
}