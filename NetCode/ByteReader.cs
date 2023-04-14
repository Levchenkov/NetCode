using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace NetCode;

public sealed class ByteReader : IByteReader
{
    private byte[] _data;
    private int _start;
    private int _end;
    private int _head;

    public ByteReader() : this(Array.Empty<byte>())
    {
    }

    public ByteReader(byte[] data)
    {
        _data = data;
        _end = _data.Length;
        _head = _start = 0;
    }

    public int Start => _start;
    
    public int End => _end;

    public int RemainingToRead => _end - _head;

    public int Head => _head;

    public void SetArray(byte[] data) => SetArray(data, 0, data.Length);
    
    public void SetArray(byte[] data, int start, int length)
    {
        if (start + length > data.Length)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException();
        }
        
        _data = data;
        _end = length + start;
        _head = _start = start;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Reset()
    {
        _head = _start;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte ReadByte()
    {
        var size = 1;
        if (_head + size > _end)
        {
            ThrowHelper.ThrowIndexOutOfRangeException();
        }

        var value = _data[_head];
        _head += size;
        return value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public short ReadShort()
    {
        var result = Read<short>();
        if (!BitConverter.IsLittleEndian)
        {
            result = BinaryPrimitives.ReverseEndianness(result);
        }
        return result;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ushort ReadUShort()
    {
        var result = Read<ushort>();
        if (!BitConverter.IsLittleEndian)
        {
            result = BinaryPrimitives.ReverseEndianness(result);
        }
        return result;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ReadInt()
    {
        var result = Read<int>();
        if (!BitConverter.IsLittleEndian)
        {
            result = BinaryPrimitives.ReverseEndianness(result);
        }
        return result;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint ReadUInt()
    {
        var result = Read<uint>();
        if (!BitConverter.IsLittleEndian)
        {
            result = BinaryPrimitives.ReverseEndianness(result);
        }
        return result;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (uint Value, int ReadBytes) TryReadUInt()
    {
        var (value, readBits) = TryRead<uint>();
        if (!BitConverter.IsLittleEndian)
        {
            value = BinaryPrimitives.ReverseEndianness(value);
        }
        return (value, readBits);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long ReadLong()
    {
        var result = Read<long>();
        if (!BitConverter.IsLittleEndian)
        {
            result = BinaryPrimitives.ReverseEndianness(result);
        }
        return result;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong ReadULong()
    {
        var result = Read<ulong>();
        if (!BitConverter.IsLittleEndian)
        {
            result = BinaryPrimitives.ReverseEndianness(result);
        }
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private T Read<T>()
        where T : unmanaged
    {
        var size = Unsafe.SizeOf<T>();
        if (_head + size > _end)
        {
            ThrowHelper.ThrowIndexOutOfRangeException();
        }

        var value = Unsafe.ReadUnaligned<T>(ref _data[_head]);
        _head += size;
        return value;
    }
    
    /// <summary>
    /// Can return 8, 16, 24 or 32 read bits for int parameter.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private (T Value, int ReadBytes) TryRead<T>()
        where T : unmanaged
    {
        var size = Unsafe.SizeOf<T>();
        if (RemainingToRead < size)
        {
            size = RemainingToRead;

            if (size == 0)
            {
                return (default, 0);
            }
        }

        var value = Unsafe.ReadUnaligned<T>(ref _data[_head]);
        _head += size;
        return (value, size);
    }
}