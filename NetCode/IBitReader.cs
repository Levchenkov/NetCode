namespace NetCode;

public interface IBitReader
{
    int BytesCount { get; }

    void SetArray(byte[] data);
    
    void SetArray(byte[] data, int offset);

    void SetArray(byte[] data, int start, int length);

    void Reset();
    
    uint ReadBits(int bitCount);

    bool ReadBool();
    
    byte ReadByte();
    
    ushort ReadUShort();
    
    short ReadShort();
    
    uint ReadUInt();
    
    int ReadInt();
}