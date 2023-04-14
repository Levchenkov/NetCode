using System.Runtime.CompilerServices;

namespace NetCode;

public sealed class BitReader : IBitReader
{
    private static readonly uint[] Masks;
    
    private ByteReader _byteReader;
    private ulong _buffer;
    private int _bitsInBuffer;
    
    static BitReader()
    {
        Masks = new uint[33];
        for (int i = 1; i < Masks.Length - 1; i++)
        {
            var mask = (1u << i) - 1;
            Masks[i] = mask;
        }
        
        Masks[32] = uint.MaxValue;
    }

    public BitReader() : this(new ByteReader())
    {
    }

    public BitReader(byte[] data) : this(new ByteReader(data))
    {
    }

    public BitReader(ByteReader byteReader)
    {
        _byteReader = byteReader;
    }

    public int Start => _byteReader.Start;
    
    public int End => _byteReader.End;

    /// <summary>
    /// Returns the number of bytes and bits that can be read from this instance.
    /// Example. We read 3 bits from 2 bytes array: 0b_00001111_00001111. Tuple (Bytes: 1, Bits: 5) will be returned. 
    ///                                                   ^ 
    /// </summary>
    public (int Bytes, int Bits) RemainingToRead
    {
        get
        {
            var (quotient, remainder) = Mathi.DivRem(_bitsInBuffer, 8);

            return (_byteReader.RemainingToRead + quotient, remainder);
        }
    }

    /// <summary>
    /// Returns the pointer with current position of reading value.
    /// Example. We read 3 bits from 2 bytes array: 0b_00001111_00001111. Tuple (Bytes: 0, Bits: 3) will be returned. 
    ///                                                   ^
    /// </summary>
    public (int Bytes, int Bits) Head
    {
        get
        {
            var (quotient, remainder) = Mathi.DivRem(_bitsInBuffer, 8);

            if (remainder == 0)
            {
                return (_byteReader.Head - quotient, 0);    
            }
            
            return (_byteReader.Head - quotient - 1, 8 - remainder);
        }
    } 

    public void SetArray(byte[] data) => SetArray(data, 0);

    public void SetArray(byte[] data, int offset) => SetArray(data, offset, data.Length - offset);

    public void SetArray(byte[] data, int start, int length)
    {
        _byteReader.SetArray(data, start, length);
        
        _buffer = 0;
        _bitsInBuffer = 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Reset()
    {
        _byteReader.Reset();
        _buffer = 0;
        _bitsInBuffer = 0;
    }
    
    public uint ReadBits(int bitCount)
    {
        if (bitCount > _bitsInBuffer)
        {
            FillBuffer(bitCount);
        }
        
        uint value = (uint)_buffer & Masks[bitCount];
        _buffer >>= bitCount;
        _bitsInBuffer -= bitCount;
        return value;
    }

    public bool ReadBool()
    {
        var bits = ReadBits(1);
        return bits == 1;
    }

    private void FillBuffer(int bitCount)
    {
        (ulong temp, int readBytes) = _byteReader.TryReadUInt();
        temp <<= _bitsInBuffer;
        _buffer |= temp;
        _bitsInBuffer += readBytes * 8;
        if (bitCount > _bitsInBuffer)
        {
            ThrowHelper.ThrowIndexOutOfRangeException();
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte ReadByte()
    {
        if (_bitsInBuffer == 0)
        {
            return _byteReader.ReadByte();
        }

        return (byte)ReadBits(8);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ushort ReadUShort()
    {
        if (_bitsInBuffer == 0)
        {
            return _byteReader.ReadUShort();
        }

        return (ushort)ReadBits(16);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public short ReadShort()
    {
        if (_bitsInBuffer == 0)
        {
            return _byteReader.ReadShort();
        }

        return (short)ReadBits(16);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint ReadUInt()
    {
        if (_bitsInBuffer == 0)
        {
            return _byteReader.ReadUInt();
        }

        return ReadBits(32);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ReadInt()
    {
        if (_bitsInBuffer == 0)
        {
            return _byteReader.ReadInt();
        }

        return (int)ReadBits(32);
    }
}