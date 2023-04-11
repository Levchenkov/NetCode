namespace NetCode;

public interface IBitWriter
{
    int BitsCount { get; }
    
    int BytesCount { get; }
    
    int Capacity { get; }
    
    byte[] Array { get; }

    void SetArray(byte[] data);
    
    void SetArray(byte[] data, int offset);

    void Clear();

    void WriteBits(int bitCount, uint value);

    void Write(bool value);
    
    void Write(byte value);
    
    void Write(ushort value);
    
    void Write(short value);
    
    void Write(uint value);
    
    void Write(int value);

    void Flush();
}