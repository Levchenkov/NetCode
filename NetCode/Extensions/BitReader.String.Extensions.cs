using System.Buffers;

namespace NetCode;

public static class BitReaderStringExtensions
{
    public static string ReadString(this BitReader reader) => ReadUtf8String(reader);

    public static string ReadString(this BitReader reader, string baseline)
    {
        var isChanged = reader.ReadBool();
        if (isChanged)
        {
            return reader.ReadString();
        }

        return baseline;
    }
    
    public static string ReadUtf8String(this BitReader reader)
    {
        var length = reader.ReadCompressedInt();

#if NETSTANDARD2_0
        
        var chars = ArrayPool<char>.Shared.Rent(length);
        for (int i = 0; i < length; i++)
        {
            chars[i] = (char)reader.ReadByte();
        }
        var s = new string(chars, 0, length);
        ArrayPool<char>.Shared.Return(chars);
        
#else
        
        var s = string.Create(length, reader, (span, bitReader) =>
        {
            for (int i = 0; i < span.Length; i++)
            {
                span[i] = (char)bitReader.ReadByte();
            }
        });

#endif
        
        return string.Intern(s);
    }
    
    public static string ReadUnicodeString(this BitReader reader)
    {
        var length = reader.ReadCompressedInt();

#if NETSTANDARD2_0
        
        var chars = ArrayPool<char>.Shared.Rent(length);
        for (int i = 0; i < length; i++)
        {
            chars[i] = (char)reader.ReadUShort();
        }
        var s = new string(chars, 0, length);
        ArrayPool<char>.Shared.Return(chars);
        
#else
        var s = string.Create(length, reader, (span, bitReader) =>
        {
            for (int i = 0; i < span.Length; i++)
            {
                span[i] = (char)bitReader.ReadUShort();
            }
        });
        
#endif
        
        return string.Intern(s);
    }
}