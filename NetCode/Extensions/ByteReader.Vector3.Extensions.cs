using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetCode;

public static class ByteReaderVector3Extensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ReadVector3(this ByteReader reader)
    {
        return new Vector3(reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat());
    }
}