namespace NetCode;

public interface IByteReader
{
    int Start { get; }
    
    int End { get; }
    
    int RemainingToRead { get; }
    
    int Head { get; }

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