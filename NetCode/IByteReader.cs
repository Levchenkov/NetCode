namespace NetCode;

public interface IByteReader
{
    int Capacity { get; }
    
    int Length { get; }

    void SetArray(byte[] data);

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