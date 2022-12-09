using System.Numerics;
using System.Runtime.CompilerServices;
using NetCode.Limits;

namespace NetCode;

public static class BitReaderVector3Extensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ReadVector3(this BitReader reader)
    {
        return new Vector3(reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat());
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ReadVector3(this BitReader reader, Vector3Limit limit)
    {
        return new Vector3(reader.ReadFloat(limit.X), reader.ReadFloat(limit.Y), reader.ReadFloat(limit.Z));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ReadVector3(this BitReader reader, Vector3 baseline)
    {
        var isChanged = reader.ReadBool();
        if (isChanged)
        {
            return reader.ReadVector3();
        }

        return baseline;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ReadVector3(this BitReader reader, Vector3 baseline, Vector3Limit limit)
    {
        var isChanged = reader.ReadBool();
        if (isChanged)
        {
            return reader.ReadVector3(limit);
        }

        return baseline;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ReadVector3(this BitReader reader, Vector3 baseline, Vector3Limit limit, Vector3Limit diffLimit)
    {
        var isChanged = reader.ReadBool();
        if (isChanged)
        {
            var isDiff = reader.ReadBool();

            if (isDiff)
            {
                var diff = reader.ReadVector3(diffLimit);
                return baseline + diff;
            }

            return reader.ReadVector3(limit);
        }

        return baseline;
    }
}