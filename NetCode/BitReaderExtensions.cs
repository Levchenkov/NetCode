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
}