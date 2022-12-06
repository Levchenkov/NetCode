using System;
using FluentAssertions;
using Xunit;

namespace NetCode.UnitTests;

public class ByteWriterTests
{
    [Fact]
    public void DefaultCtor_WriteULong_ArrayShouldContainBytes()
    {
        var byteWriter = new ByteWriter();

        ulong value = 0b_11111111_00000000_11001100_00110011_10101010_01010101_11110000_00001111; 
        byteWriter.Write(value);

        byteWriter.Count.Should().Be(8);
        byteWriter.Array[0].Should().Be((byte) value);
        byteWriter.Array[1].Should().Be((byte) (value >> 8));
        byteWriter.Array[2].Should().Be((byte) (value >> 16));
        byteWriter.Array[3].Should().Be((byte) (value >> 24));
        byteWriter.Array[4].Should().Be((byte) (value >> 32));
        byteWriter.Array[5].Should().Be((byte) (value >> 40));
        byteWriter.Array[6].Should().Be((byte) (value >> 48));
        byteWriter.Array[7].Should().Be((byte) (value >> 56));
    }
    
    [Fact]
    public void DefaultCtor_WriteLong_ArrayShouldContainBytes()
    {
        var byteWriter = new ByteWriter();

        long value = unchecked((long)0b_11111111_00000000_11001100_00110011_10101010_01010101_11110000_00001111); 
        byteWriter.Write(value);

        byteWriter.Count.Should().Be(8);
        byteWriter.Array[0].Should().Be((byte) value);
        byteWriter.Array[1].Should().Be((byte) (value >> 8));
        byteWriter.Array[2].Should().Be((byte) (value >> 16));
        byteWriter.Array[3].Should().Be((byte) (value >> 24));
        byteWriter.Array[4].Should().Be((byte) (value >> 32));
        byteWriter.Array[5].Should().Be((byte) (value >> 40));
        byteWriter.Array[6].Should().Be((byte) (value >> 48));
        byteWriter.Array[7].Should().Be((byte) (value >> 56));
    }
    
    [Fact]
    public void DefaultCtor_WriteUInt_ArrayShouldContainBytes()
    {
        var byteWriter = new ByteWriter();

        uint value = 0b_10101010_01010101_11001100_11110000;
        byteWriter.Write(value);

        byteWriter.Count.Should().Be(4);
        byteWriter.Array[0].Should().Be(0b11110000);
        byteWriter.Array[1].Should().Be(0b11001100);
        byteWriter.Array[2].Should().Be(0b01010101);
        byteWriter.Array[3].Should().Be(0b10101010);
    }
    
    [Fact]
    public void DefaultCtor_WriteInt_ArrayShouldContainBytes()
    {
        var byteWriter = new ByteWriter();
        
        int value = unchecked((int)0b_10101010_01010101_11001100_11110000);
        byteWriter.Write(value);

        byteWriter.Count.Should().Be(4);
        byteWriter.Array[0].Should().Be(0b11110000);
        byteWriter.Array[1].Should().Be(0b11001100);
        byteWriter.Array[2].Should().Be(0b01010101);
        byteWriter.Array[3].Should().Be(0b10101010);
    }
    
    [Fact]
    public void DefaultCtor_WriteUShort_ArrayShouldContainBytes()
    {
        var byteWriter = new ByteWriter();

        ushort value = 0b_10101010_01010101;
        byteWriter.Write(value);

        byteWriter.Count.Should().Be(2);
        byteWriter.Array[0].Should().Be(0b01010101);
        byteWriter.Array[1].Should().Be(0b10101010);
    }
    
    [Fact]
    public void DefaultCtor_WriteShort_ArrayShouldContainBytes()
    {
        var byteWriter = new ByteWriter();
        
        short value = unchecked((short)0b_10101010_01010101);
        byteWriter.Write(value);

        byteWriter.Count.Should().Be(2);
        byteWriter.Array[0].Should().Be(0b01010101);
        byteWriter.Array[1].Should().Be(0b10101010);
    }
    
    [Fact]
    public void DefaultCtor_WriteByte_ArrayShouldContainByte()
    {
        var byteWriter = new ByteWriter();
        
        byte value = 0b10101010;
        byteWriter.Write(value);

        byteWriter.Count.Should().Be(1);
        byteWriter.Array[0].Should().Be(0b10101010);
    }

    [Fact]
    public void EmptyByteWriter_WriteByte_ExceptionExpected()
    {
        var emptyByteWriter = new ByteWriter(Array.Empty<byte>());
        emptyByteWriter.Capacity.Should().Be(0);
        
        Action action = () => emptyByteWriter.Write(byte.MaxValue);

        action.Should().Throw<IndexOutOfRangeException>();
    }
    
    [Fact]
    public void SizeOfArrayIs1Byte_WriteShort_ExceptionExpected()
    {
        var byteWriter = new ByteWriter(new byte[1]);

        Action action = () => byteWriter.Write(short.MaxValue);
        
        action.Should().Throw<IndexOutOfRangeException>();
    }
    
    [Fact]
    public void EmptyByteWriter_SetNonEmptyArrayAndWriteShort_ArrayShouldContainBytes()
    {
        var byteWriter = new ByteWriter(Array.Empty<byte>());
        byteWriter.SetArray(new byte[2]);

        ushort value = 0b_10101010_01010101;
        byteWriter.Write(value);

        byteWriter.Count.Should().Be(2);
        byteWriter.Array[0].Should().Be(0b_01010101);
        byteWriter.Array[1].Should().Be(0b_10101010);
    }

    [Fact]
    public void SizeOfArrayIs1Byte_WriteByteAndClearAndWriteByte_ArrayShouldContainTheLastWrittenByte()
    {
        var byteWriter = new ByteWriter(new byte[1]);
        byteWriter.Write(byte.MaxValue);
        byteWriter.Clear();

        byteWriter.Count.Should().Be(0);
        byteWriter.Write((byte)0b_10101010);

        byteWriter.Array[0].Should().Be(0b_10101010);
    }
}