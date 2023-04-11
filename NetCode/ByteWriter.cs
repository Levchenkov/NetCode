using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace NetCode;

public sealed class ByteWriter : IByteWriter
{
    private const int DefaultCapacity = 1500;

    private byte[] _data;
    private int _capacity;
    private int _count;

    public ByteWriter(int capacity = DefaultCapacity):this(new byte[capacity])
    {
    }

    public ByteWriter(byte[] data)
    {
        _data = data;
        _capacity = _data.Length; 
        _count = 0;
    }

    public int Capacity => _capacity;

    public int Count => _count;

    public byte[] Array => _data;

    public void SetArray(byte[] data) => SetArray(data, 0);

    public void SetArray(byte[] data, int offset)
    {
        if (offset > data.Length)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException();
        }
        
        _data = data;
        _capacity = _data.Length;
        _count = offset;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear()
    {
        _count = 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write(byte value)
    {
        var size = 1;
        if (size + Count > _capacity)
        {
            ThrowHelper.ThrowIndexOutOfRangeException();
        }
        _data[Count] = value;
        _count += size;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write(short value)
    {
        if (!BitConverter.IsLittleEndian)
        {
            value = BinaryPrimitives.ReverseEndianness(value);
        }
        
        WriteInternal(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write(ushort value)
    {
        if (!BitConverter.IsLittleEndian)
        {
            value = BinaryPrimitives.ReverseEndianness(value);
        }
        
        WriteInternal(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write(int value)
    {
        if (!BitConverter.IsLittleEndian)
        {
            value = BinaryPrimitives.ReverseEndianness(value);
        }
        
        WriteInternal(value);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write(uint value)
    {
        if (!BitConverter.IsLittleEndian)
        {
            value = BinaryPrimitives.ReverseEndianness(value);
        }
        
        WriteInternal(value);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write(long value)
    {
        if (!BitConverter.IsLittleEndian)
        {
            value = BinaryPrimitives.ReverseEndianness(value);
        }
        
        WriteInternal(value);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write(ulong value)
    {
        if (!BitConverter.IsLittleEndian)
        {
            value = BinaryPrimitives.ReverseEndianness(value);
        }
        
        WriteInternal(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void WriteInternal<T>(T value)
        where T : unmanaged
    {
        var size = Unsafe.SizeOf<T>();
        if (size + _count > _capacity)
        {
            ThrowHelper.ThrowIndexOutOfRangeException();
        }
        
        Unsafe.WriteUnaligned(ref _data[_count], value);
        _count += size;
    }
}