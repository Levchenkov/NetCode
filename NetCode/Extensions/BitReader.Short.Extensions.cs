namespace NetCode;

public static class BitReaderShortExtensions
{
    public static short ReadShort(this BitReader reader, short baseline)
    {
        var isChanged = reader.ReadBool();
        if (isChanged)
        {
            return reader.ReadShort();
        }

        return baseline;
    }
    
    public static ushort ReadUShort(this BitReader reader, ushort baseline)
    {
        var isChanged = reader.ReadBool();
        if (isChanged)
        {
            return reader.ReadUShort();
        }

        return baseline;
    }
}