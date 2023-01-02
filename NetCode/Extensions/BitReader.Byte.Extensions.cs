using System.Runtime.CompilerServices;
using NetCode.Limits;

namespace NetCode;

public static class BitReaderByteExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte ReadByte(this BitReader reader, ByteLimit limit)
    {
        var value = (byte)reader.ReadBits(limit.BitCount);
        return (byte)(value + limit.Min);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte ReadByte(this BitReader reader, byte baseline)
    {
        var isChanged = reader.ReadBool();
        if (isChanged)
        {
            return reader.ReadByte();
        }

        return baseline;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte ReadByte(this BitReader reader, byte baseline, ByteLimit limit)
    {
        var isChanged = reader.ReadBool();
        if (isChanged)
        {
            return reader.ReadByte(limit);
        }

        return baseline;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte ReadByte(this BitReader reader, byte baseline, ByteLimit limit, ByteLimit diffLimit)
    {
        var isChanged = reader.ReadBool();
        if (isChanged)
        {
            var isDiff = reader.ReadBool();

            if (isDiff)
            {
                var diff = reader.ReadByte(diffLimit);
                return (byte)(baseline + diff);
            }

            return reader.ReadByte(limit);
        }

        return baseline;
    }
}