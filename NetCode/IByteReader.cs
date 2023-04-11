namespace NetCode;

public interface IByteReader
{
    int Capacity { get; }
    
    int Length { get; }

    void SetArray(byte[] data);

    void SetArray(byte[] data, int start, int length);

    void Reset();
    
    byte ReadByte();

    short ReadShort();

    ushort ReadUShort();
    
    int ReadInt();

    uint ReadUInt();
    
    long ReadLong();
    
    ulong ReadULong();

    (uint Value, int ReadBytes) TryReadUInt();
}