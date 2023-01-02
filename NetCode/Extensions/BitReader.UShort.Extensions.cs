using System.Runtime.CompilerServices;
using NetCode.Limits;

namespace NetCode;

public static class BitReaderUShortExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort ReadUShort(this BitReader reader, UShortLimit limit)
    {
        var value = (ushort)reader.ReadBits(limit.BitCount);
        return (ushort)(value + limit.Min);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort ReadUShort(this BitReader reader, ushort baseline)
    {
        var isChanged = reader.ReadBool();
        if (isChanged)
        {
            return reader.ReadUShort();
        }

        return baseline;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort ReadUShort(this BitReader reader, ushort baseline, UShortLimit limit)
    {
        var isChanged = reader.ReadBool();
        if (isChanged)
        {
            return reader.ReadUShort(limit);
        }

        return baseline;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort ReadUShort(this BitReader reader, ushort baseline, UShortLimit limit, UShortLimit diffLimit)
    {
        var isChanged = reader.ReadBool();
        if (isChanged)
        {
            var isDiff = reader.ReadBool();

            if (isDiff)
            {
                var diff = reader.ReadUShort(diffLimit);
                return (ushort)(baseline + diff);
            }

            return reader.ReadUShort(limit);
        }

        return baseline;
    }
}