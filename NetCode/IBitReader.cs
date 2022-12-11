namespace NetCode;

public interface IBitReader
{
    int BytesCount { get; }

    void SetArray(byte[] data);

    void Reset();
    
    uint ReadBits(int bitCount);

    bool ReadBool();
    
    byte ReadByte();
    
    ushort ReadUShort();
    
    short ReadShort();
    
    uint ReadUInt();
    
    int ReadInt();
}