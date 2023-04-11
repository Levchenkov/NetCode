namespace NetCode;

public interface IByteWriter
{
    int Capacity { get; }
    
    int Count { get; }
    
    byte[] Array { get; }

    void SetArray(byte[] data);
    
    void SetArray(byte[] data, int offset);

    void Clear();
    
    void Write(byte value);

    void Write(short value);

    void Write(ushort value);

    void Write(int value);

    void Write(uint value);
    
    void Write(long value);
    
    void Write(ulong value);
}