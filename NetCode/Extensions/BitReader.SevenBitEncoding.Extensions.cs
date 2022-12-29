namespace NetCode;

public static class BitReaderSevenBitEncodingExtensions
{
    public static int ReadCompressedInt(this BitReader reader)
    {
        uint value = reader.ReadCompressedUInt();
        int zagzig = (int)((value >> 1) ^ (-(int)(value & 1)));

        return zagzig;
    }
    
    public static uint ReadCompressedUInt(this BitReader reader)
    {
        uint buffer = 0b_0u;
        uint value = 0b_0u;
        int shift = 0;

        do {
            buffer = reader.ReadBits(8);

            value |= (buffer & 0b_111_1111u) << shift;
            shift += 7;
        }
        while ((buffer & 0b_1000_0000u) > 0);

        return value;
    }
}