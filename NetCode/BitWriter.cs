using System.Runtime.CompilerServices;

namespace NetCode;

public sealed class BitWriter : IBitWriter
{
    private const int DefaultCapacity = 1500;
    
    private static readonly uint[] Masks;
    
    private readonly ByteWriter _byteWriter;
    private ulong _buffer;
    private int _bitsInBuffer;

    static BitWriter()
    {
        Masks = new uint[33];
        for (int i = 1; i < Masks.Length - 1; i++)
        {
            var mask = (1u << i) - 1;
            Masks[i] = mask;
        }
        
        Masks[32] = uint.MaxValue;
    }

    public BitWriter(int capacity = DefaultCapacity) : this(new ByteWriter(capacity))
    {
    }

    public BitWriter(byte[] data) : this(new ByteWriter(data))
    {
    }

    public BitWriter(ByteWriter byteWriter)
    {
        _byteWriter = byteWriter;
    }

    public int BitsCount => _byteWriter.Count * 8 + _bitsInBuffer;
    
    public int BytesCount => _byteWriter.Count + Mathi.Ceiling(_bitsInBuffer, 8);

    public int Capacity => _byteWriter.Capacity;

    public byte[] Array => _bitsInBuffer == 0 ? _byteWriter.Array : throw new InvalidOperationException("Writer should be flushed first.");

    public void SetArray(byte[] data) => SetArray(data, 0);

    public void SetArray(byte[] data, int offset)
    {
        _byteWriter.SetArray(data, offset);
        
        _bitsInBuffer = 0;
        _buffer = 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear()
    {
        _byteWriter.Clear();
        _bitsInBuffer = 0;
        _buffer = 0;
    }

    public void WriteBits(int bitCount, uint value)
    {
        value &= Masks[bitCount];

        _buffer |= (ulong)value << _bitsInBuffer;
        _bitsInBuffer += bitCount;
    
        if (_bitsInBuffer >= 32)
        {
            uint write = (uint)_buffer;
            _byteWriter.Write(write);
            
            _buffer >>= 32;
            _bitsInBuffer -= 32;
        }
    }

    public void Write(bool value) => WriteBits(1, value ? 1u : 0u);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write(byte value)
    {
        if (_bitsInBuffer == 0)
        {
            _byteWriter.Write(value);
        }
        else
        {
            WriteBits(8, value);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write(ushort value)
    {
        if (_bitsInBuffer == 0)
        {
            _byteWriter.Write(value);
        }
        else
        {
            WriteBits(16, value);
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write(short value)
    {
        Write((ushort)value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write(uint value)
    {
        if (_bitsInBuffer == 0)
        {
            _byteWriter.Write(value);
        }
        else
        {
            WriteBits(32, value);
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write(int value)
    {
        Write((uint)value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Flush()
    {
        while (_bitsInBuffer > 0)
        {
            _byteWriter.Write((byte)_buffer);

            _buffer >>= 8;
            _bitsInBuffer -= 8;
        }

        _buffer = 0;
        _bitsInBuffer = 0;
    }
}