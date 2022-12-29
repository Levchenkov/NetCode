namespace NetCode;

public static class BitWriterSevenBitEncodingExtensions
{
    public static void WriteCompressed(this BitWriter writer, int value) 
    {
        uint zigzag = (uint)((value << 1) ^ (value >> 31));

        WriteCompressed(writer, zigzag);
    }
    
    public static void WriteCompressed(this BitWriter writer, uint value) 
    {

        do {
            uint buffer = value & 0b_111_1111u;
            value >>= 7;

            if (value > 0)
                buffer |= 0b_1000_0000u;

            writer.WriteBits(8, buffer);
        }

        while (value > 0);
    }
}