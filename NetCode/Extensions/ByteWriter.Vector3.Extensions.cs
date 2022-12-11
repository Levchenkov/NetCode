using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetCode;

public static class ByteWriterVector3Extensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(this ByteWriter writer, Vector3 value)
    {
        writer.Write(value.X);
        writer.Write(value.Y);
        writer.Write(value.Z);
    }
}