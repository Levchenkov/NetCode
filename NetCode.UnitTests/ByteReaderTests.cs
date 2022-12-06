using System;
using FluentAssertions;
using Xunit;

namespace NetCode.UnitTests;

public class ByteReaderTests
{
    [Fact]
    public void ArrayContains8Bytes_ReadULong_ULongShouldBeRead()
    {
        var array = new byte[]
        {
            0b_00001111,
            0b_11110000,
            0b_01010101,
            0b_10101010,
            
            0b_00110011,
            0b_11001100,
            0b_00000000,
            0b_11111111
        };

        var byteReader = new ByteReader(array);
        var value = byteReader.ReadULong();

        value.Should().Be(0b_11111111_00000000_11001100_00110011_10101010_01010101_11110000_00001111);
    }
    
    [Fact]
    public void ArrayContains8Bytes_ReadLong_LongShouldBeRead()
    {
        var array = new byte[]
        {
            0b_00001111,
            0b_11110000,
            0b_01010101,
            0b_10101010,
            
            0b_00110011,
            0b_11001100,
            0b_00000000,
            0b_11111111
        };

        var byteReader = new ByteReader(array);
        var value = byteReader.ReadLong();

        value.Should().Be(unchecked((long)0b_11111111_00000000_11001100_00110011_10101010_01010101_11110000_00001111));
    }
    
    [Fact]
    public void ArrayContains4Bytes_ReadUInt_UIntShouldBeRead()
    {
        var array = new byte[]
        {
            0b_00001111,
            0b_11110000,
            0b_01010101,
            0b_10101010
        };

        var byteReader = new ByteReader(array);
        var value = byteReader.ReadUInt();

        value.Should().Be(0b_10101010_01010101_11110000_00001111);
    }
    
    [Fact]
    public void ArrayContains4Bytes_ReadInt_IntShouldBeRead()
    {
        var array = new byte[]
        {
            0b_00001111,
            0b_11110000,
            0b_01010101,
            0b_10101010
        };

        var byteReader = new ByteReader(array);
        var value = byteReader.ReadInt();

        value.Should().Be(unchecked((int)0b_10101010_01010101_11110000_00001111));
    }
    
    [Fact]
    public void ArrayContains2Bytes_ReadUShort_UShortShouldBeRead()
    {
        var array = new byte[]
        {
            0b_00001111,
            0b_11110000
        };

        var byteReader = new ByteReader(array);
        var value = byteReader.ReadUShort();

        value.Should().Be(0b_11110000_00001111);
    }
    
    [Fact]
    public void ArrayContains2Bytes_ReadShort_ShortShouldBeRead()
    {
        var array = new byte[]
        {
            0b_00001111,
            0b_11110000
        };

        var byteReader = new ByteReader(array);
        var value = byteReader.ReadShort();

        value.Should().Be(unchecked((short)0b_11110000_00001111));
    }
    
    [Fact]
    public void ArrayContains1Byte_ReadByte_ByteShouldBeRead()
    {
        var array = new byte[]
        {
            0b_11110000
        };

        var byteReader = new ByteReader(array);
        var value = byteReader.ReadByte();

        value.Should().Be(0b_11110000);
    }

    [Fact]
    public void EmptyArray_ReadByte_ExceptionExpected()
    {
        var byteReader = new ByteReader(Array.Empty<byte>());

        byteReader.Capacity.Should().Be(0);

        Action action = () => byteReader.ReadByte();

        action.Should().Throw<IndexOutOfRangeException>();
    }

    [Fact]
    public void ArrayContains1Byte_ReadUInt_ExceptionExpected()
    {
        var array = new byte[]
        {
            0b_11110000
        };

        var byteReader = new ByteReader(array);
        Action action = () => byteReader.ReadUInt();

        action.Should().Throw<IndexOutOfRangeException>();
    }
    
    [Fact]
    public void ArrayContains1Byte_TryReadUInt_ShouldBeOk()
    {
        var array = new byte[]
        {
            0b_11110000
        };

        var byteReader = new ByteReader(array);
        byteReader.Length.Should().Be(1);
        
        var (value, readBytes) = byteReader.TryReadUInt();

        value.Should().Be(0b_11110000);
        readBytes.Should().Be(1);
    }
    
    [Fact]
    public void ArrayContains4Bytes_TryReadUInt_ShouldBeOk()
    {
        var array = new byte[]
        {
            0b_11110000,
            0b_11110000,
            0b_11110000,
            0b_11110000
        };

        var byteReader = new ByteReader(array);
        byteReader.Length.Should().Be(4);
        
        var (value, readBytes) = byteReader.TryReadUInt();

        value.Should().Be(0b_11110000_11110000_11110000_11110000);
        readBytes.Should().Be(4);
    }
    
    [Fact]
    public void ArrayContains5Bytes_TryReadUInt_ShouldBeOk()
    {
        var array = new byte[]
        {
            0b_11110000,
            0b_11110000,
            0b_11110000,
            0b_11110000,
            0b_11110000
        };

        var byteReader = new ByteReader(array);
        byteReader.Length.Should().Be(5);
        
        var (value, readBytes) = byteReader.TryReadUInt();

        value.Should().Be(0b_11110000_11110000_11110000_11110000);
        readBytes.Should().Be(4);
        byteReader.Length.Should().Be(1);
    }
    
    [Fact]
    public void ArrayContains1Byte_TryReadUIntTwice_FirstShouldHaveDataSecondShouldBeEmpty()
    {
        var array = new byte[]
        {
            0b_11110000
        };

        var byteReader = new ByteReader(array);
        var (value, readBytes) = byteReader.TryReadUInt();

        value.Should().Be(0b_11110000);
        readBytes.Should().Be(1);
        
        (value, readBytes) = byteReader.TryReadUInt();
        value.Should().Be(default);
        readBytes.Should().Be(0);
    }
    
    [Fact]
    public void ArrayContains3Bytes_ReadShort_ShortShouldBeRead()
    {
        var array = new byte[]
        {
            0b_00001111,
            0b_11110000,
            0b_10101010
        };

        var byteReader = new ByteReader(array);
        var value = byteReader.ReadShort();

        value.Should().Be(unchecked((short)0b_11110000_00001111));
    }
    
    [Fact]
    public void ArrayContains4Bytes_ReadDataSeveralTimes_LenghtShouldBeValid()
    {
        var array = new byte[]
        {
            0b_11001100,
            0b_00001111,
            0b_11110000,
            0b_10101010
        };

        var byteReader = new ByteReader(array);
        byteReader.Length.Should().Be(4);
        
        byteReader.ReadByte();
        byteReader.Length.Should().Be(3);
        
        byteReader.ReadShort();
        byteReader.Length.Should().Be(1);

        byteReader.ReadByte();
        byteReader.Length.Should().Be(0);
    }
    
    [Fact]
    public void ArrayContains4Bytes_TryReadDataSeveralTimes_LenghtShouldBeValid()
    {
        var array = new byte[]
        {
            0b_11001100,
            0b_00001111,
            0b_11110000,
            0b_10101010
        };

        var byteReader = new ByteReader(array);
        byteReader.Length.Should().Be(4);
        
        byteReader.ReadByte();
        byteReader.Length.Should().Be(3);
        
        byteReader.ReadShort();
        byteReader.Length.Should().Be(1);

        byteReader.TryReadUInt();
        byteReader.Length.Should().Be(0);
    }
    
    [Fact]
    public void ArrayContains2Bytes_ReadShortAndResetAndReadShort_ShouldBeRead()
    {
        var array = new byte[]
        {
            0b_00001111,
            0b_11110000
        };

        var byteReader = new ByteReader(array);
        var value = byteReader.ReadShort();

        value.Should().Be(unchecked((short)0b_11110000_00001111));
        
        byteReader.Reset();
        
        var value2 = byteReader.ReadShort();

        value2.Should().Be(unchecked((short)0b_11110000_00001111));
    }
}