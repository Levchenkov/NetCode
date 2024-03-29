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

        byteReader.End.Should().Be(0);

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
        byteReader.RemainingToRead.Should().Be(1);
        
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
        byteReader.RemainingToRead.Should().Be(4);
        
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
        byteReader.RemainingToRead.Should().Be(5);
        
        var (value, readBytes) = byteReader.TryReadUInt();

        value.Should().Be(0b_11110000_11110000_11110000_11110000);
        readBytes.Should().Be(4);
        byteReader.RemainingToRead.Should().Be(1);
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
    public void ArrayContains4Bytes_ReadDataSeveralTimes_RemainingToReadShouldBeValid()
    {
        var array = new byte[]
        {
            0b_11001100,
            0b_00001111,
            0b_11110000,
            0b_10101010
        };

        var byteReader = new ByteReader(array);
        byteReader.RemainingToRead.Should().Be(4);
        
        byteReader.ReadByte();
        byteReader.RemainingToRead.Should().Be(3);
        
        byteReader.ReadShort();
        byteReader.RemainingToRead.Should().Be(1);

        byteReader.ReadByte();
        byteReader.RemainingToRead.Should().Be(0);
    }
    
    [Fact]
    public void ArrayContains4Bytes_TryReadDataSeveralTimes_RemainingToReadShouldBeValid()
    {
        var array = new byte[]
        {
            0b_11001100,
            0b_00001111,
            0b_11110000,
            0b_10101010
        };

        var byteReader = new ByteReader(array);
        byteReader.RemainingToRead.Should().Be(4);
        
        byteReader.ReadByte();
        byteReader.RemainingToRead.Should().Be(3);
        
        byteReader.ReadShort();
        byteReader.RemainingToRead.Should().Be(1);

        byteReader.TryReadUInt();
        byteReader.RemainingToRead.Should().Be(0);
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

    [Fact]
    public void EmptyReader_SetArrayAndReadShort_ShouldBeRead()
    {
        var byteReader = new ByteReader(Array.Empty<byte>());
        
        var array = new byte[]
        {
            0b_00001111,
            0b_11110000
        };
        
        byteReader.SetArray(array);

        byteReader.RemainingToRead.Should().Be(2);
        byteReader.ReadByte().Should().Be(0b_00001111);
        byteReader.ReadByte().Should().Be(0b_11110000);
    }
    
    [Fact]
    public void EmptyReader_SetArrayWithStartEq0AndReadShort_ShouldBeRead()
    {
        var byteReader = new ByteReader(Array.Empty<byte>());
        
        var array = new byte[]
        {
            0b_00001111,
            0b_11110000
        };
        
        byteReader.SetArray(array, 0, 2);
        
        byteReader.RemainingToRead.Should().Be(2);
        byteReader.ReadByte().Should().Be(0b_00001111);
        byteReader.ReadByte().Should().Be(0b_11110000);
    }
    
    [Fact]
    public void EmptyReader_SetArrayWithStartEq1AndReadByte_ShouldBeRead()
    {
        var byteReader = new ByteReader(Array.Empty<byte>());
        
        var array = new byte[]
        {
            0b_00001111,
            0b_11110000
        };
        
        byteReader.SetArray(array, 1, 1);
        
        byteReader.RemainingToRead.Should().Be(1);
        byteReader.ReadByte().Should().Be(0b_11110000);
    }
    
    [Fact]
    public void EmptyReader_SetArrayWithLengthMoreThanArrayLength_ExceptionExpected()
    {
        var byteReader = new ByteReader(Array.Empty<byte>());
        
        Action action = () => byteReader.SetArray(new byte[2], 0, 3);

        action.Should().Throw<ArgumentOutOfRangeException>();
    }
    
    [Fact]
    public void EmptyReader_SetArrayWithStartMoreThanArrayLength_ExceptionExpected()
    {
        var byteReader = new ByteReader(Array.Empty<byte>());
        
        Action action = () => byteReader.SetArray(new byte[2], 3, 2);
        
        action.Should().Throw<ArgumentOutOfRangeException>();
    }
    
    [Fact]
    public void SetArray_ReadDataSeveralTimes_RemainingToReadShouldBeValid()
    {
        var array = new byte[]
        {
            0b_11001100,
            0b_00001111,
            0b_11110000,
            0b_10101010
        };

        var byteReader = new ByteReader();
        byteReader.SetArray(array);
        
        byteReader.Start.Should().Be(0);
        byteReader.End.Should().Be(4);
        byteReader.Head.Should().Be(0);
        byteReader.RemainingToRead.Should().Be(4);
        
        byteReader.ReadByte();
        
        byteReader.Start.Should().Be(0);
        byteReader.End.Should().Be(4);
        byteReader.Head.Should().Be(1);
        byteReader.RemainingToRead.Should().Be(3);
        
        byteReader.ReadShort();
        
        byteReader.Start.Should().Be(0);
        byteReader.End.Should().Be(4);
        byteReader.Head.Should().Be(3);
        byteReader.RemainingToRead.Should().Be(1);

        byteReader.TryReadUInt();
        
        byteReader.Start.Should().Be(0);
        byteReader.End.Should().Be(4);
        byteReader.Head.Should().Be(4);
        byteReader.RemainingToRead.Should().Be(0);
    }
    
    [Fact]
    public void SetArrayWithStart_ReadDataSeveralTimes_RemainingToReadShouldBeValid()
    {
        var array = new byte[]
        {
            0b_11001100,
            0b_00001111,
            0b_11110000,
            0b_10101010
        };

        var byteReader = new ByteReader();
        byteReader.SetArray(array, 1, 3);
        
        byteReader.Start.Should().Be(1);
        byteReader.End.Should().Be(4);
        byteReader.Head.Should().Be(1);
        byteReader.RemainingToRead.Should().Be(3);
        
        byteReader.ReadByte();
        
        byteReader.Start.Should().Be(1);
        byteReader.End.Should().Be(4);
        byteReader.Head.Should().Be(2);
        byteReader.RemainingToRead.Should().Be(2);
        
        byteReader.ReadShort();
        
        byteReader.Start.Should().Be(1);
        byteReader.End.Should().Be(4);
        byteReader.Head.Should().Be(4);
        byteReader.RemainingToRead.Should().Be(0);

        byteReader.TryReadUInt();
        
        byteReader.Start.Should().Be(1);
        byteReader.End.Should().Be(4);
        byteReader.Head.Should().Be(4);
        byteReader.RemainingToRead.Should().Be(0);
        
        byteReader.Reset();
        
        byteReader.Start.Should().Be(1);
        byteReader.End.Should().Be(4);
        byteReader.Head.Should().Be(1);
        byteReader.RemainingToRead.Should().Be(3);
    }
}