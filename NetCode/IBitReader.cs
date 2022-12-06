namespace NetCode;

public interface IBitReader
{
    int BytesCount { get; }

    void SetArray(byte[] data);

    void Reset();
    
    uint ReadBits(int bitCount);
    
    byte ReadByte();
    
    ushort ReadUShort();
    
    short ReadShort();
    
    uint ReadUint();
    
    int ReadInt();
}