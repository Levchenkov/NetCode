using System;
using FluentAssertions;
using Xunit;

namespace NetCode.UnitTests;

public class BitReaderTests
{
    [Fact]
    public void ReadBits_ArrayHas8BitsAndRead3Bits_ResultShouldHave3Bits()
    {
        var bitReader = new BitReader(new byte[] {0b_1010_1010});

        var value = bitReader.ReadBits(3);

        value.Should().Be(0b_010);
    }
    
    [Fact]
    public void ReadBits_ArrayHas8BitsRead7BitsAndRead1Bit_ShouldRead()
    {
        var bitReader = new BitReader(new byte[] {0b_1010_1010});

        var value = bitReader.ReadBits(7);

        value.Should().Be(0b_010_1010);
        
        value = bitReader.ReadBits(1);

        value.Should().Be(0b_1);
    }
    
    [Fact]
    public void ReadBits_ArrayHas8BitsRead8BitsAndRead1Bit_ExceptionExpected()
    {
        var bitReader = new BitReader(new byte[] {0b_1010_1010});

        var value = bitReader.ReadBits(8);

        value.Should().Be(0b_1010_1010);
        
        Action action = () => bitReader.ReadBits(1);

        action.Should().Throw<Exception>();
    }
    
    [Fact]
    public void ReadBits_ArrayHas8BitsRead9Bits_ExceptionExpected()
    {
        var bitReader = new BitReader(new byte[] {0b_1010_1010});

        Action action = () => bitReader.ReadBits(9);

        action.Should().Throw<Exception>();
    }
    
    [Fact]
    public void ReadBits_ProvideNegativeBitCount_ExceptionExpected()
    {
        var bitReader = new BitReader(new byte[] {0b_1010_1010});

        Action action = () => bitReader.ReadBits(-1);

        action.Should().Throw<Exception>();
    }
    
    [Fact]
    public void ReadBits_ProvideMoreThan32BitCount_ExceptionExpected()
    {
        var bitReader = new BitReader(new byte[] {0b_1010_1010});

        Action action = () => bitReader.ReadBits(33);

        action.Should().Throw<Exception>();
    }
    
    [Fact]
    public void ReadBits_EmptyArrayRead1Bit_ExceptionExpected()
    {
        var bitReader = new BitReader(Array.Empty<byte>());

        Action action = () => bitReader.ReadBits(1);

        action.Should().Throw<Exception>();
    }
    
    [Fact]
    public void ReadBits_SetArrayAndRead1Bit_ShouldRead()
    {
        var bitReader = new BitReader(Array.Empty<byte>());
        bitReader.SetArray(new byte[] {byte.MaxValue});

        var bits = bitReader.ReadBits(1);
        bits.Should().Be(1);
    }
    
    [Fact]
    public void ReadBits_SetArrayAndReadAllAndSetArrayAndReadAll_ShouldRead()
    {
        var bitReader = new BitReader(Array.Empty<byte>());
        bitReader.SetArray(new byte[] {byte.MaxValue});

        var bits = bitReader.ReadBits(8);
        bits.Should().Be(byte.MaxValue);
        
        bitReader.SetArray(new byte[] {byte.MaxValue});

        var bits2 = bitReader.ReadBits(8);
        bits2.Should().Be(byte.MaxValue);
    }
    
    [Fact]
    public void ReadBits_ReadAllAndResetAndReadAll_ShouldRead()
    {
        var bitReader = new BitReader(new byte[] {byte.MaxValue});

        var bits = bitReader.ReadBits(8);
        bits.Should().Be(byte.MaxValue);
        
        bitReader.Reset();

        var bits2 = bitReader.ReadBits(8);
        bits2.Should().Be(byte.MaxValue);
    }
    
    [Fact]
    public void ReadByte_AlignedRead_ShouldRead()
    {
        var bitReader = new BitReader(new byte[] {0b_11110000});

        var value = bitReader.ReadByte();
        value.Should().Be(0b_11110000);
    }
    
    [Fact]
    public void ReadByte_UnAlignedRead_ShouldRead()
    {
        var bitReader = new BitReader(new byte[] { 0b_11100001, 0b_1 });

        var readBits = bitReader.ReadBits(1);
        readBits.Should().Be(1);

        var value = bitReader.ReadByte();
        value.Should().Be(0b_11110000);
    }
    
    [Fact]
    public void ReadShort_AlignedRead_ShouldRead()
    {
        var bitReader = new BitReader(new byte[] {0b_00001111, 0b_11110000});

        var value = bitReader.ReadShort();
        value.Should().Be(unchecked((short)0b_11110000_00001111));
    }
    
    [Fact]
    public void ReadShort_UnAlignedRead_ShouldRead()
    {
        var bitReader = new BitReader(new byte[] {0b_00011111, 0b_11100000, 0b_1});

        var readBits = bitReader.ReadBits(1);
        readBits.Should().Be(1);
        
        var value = bitReader.ReadShort();
        value.Should().Be(unchecked((short)0b_11110000_00001111));
    }
    
    [Fact]
    public void ReadUShort_AlignedRead_ShouldRead()
    {
        var bitReader = new BitReader(new byte[] {0b_00001111, 0b_11110000});

        var value = bitReader.ReadUShort();
        value.Should().Be(0b_11110000_00001111);
    }
    
    [Fact]
    public void ReadUShort_UnAlignedRead_ShouldRead()
    {
        var bitReader = new BitReader(new byte[] {0b_00011111, 0b_11100000, 0b_1});

        var readBits = bitReader.ReadBits(1);
        readBits.Should().Be(1);
        
        var value = bitReader.ReadUShort();
        value.Should().Be(0b_11110000_00001111);
    }
    
    [Fact]
    public void ReadInt_AlignedRead_ShouldRead()
    {
        var bitReader = new BitReader(new byte[] {0b_00001111, 0b_11110000, 0b_01010101, 0b_10101010});

        var value = bitReader.ReadInt();
        value.Should().Be(unchecked((int)0b_10101010_01010101_11110000_00001111));
    }
    
    [Fact]
    public void ReadInt_UnAlignedRead_ShouldRead()
    {
        var bitReader = new BitReader(new byte[] {0b_00011111, 0b_11100000, 0b_10101011, 0b_01010100, 0b_1});

        var readBits = bitReader.ReadBits(1);
        readBits.Should().Be(1);
        
        var value = bitReader.ReadInt();
        value.Should().Be(unchecked((int)0b_10101010_01010101_11110000_00001111));
    }
    
    [Fact]
    public void ReadUInt_AlignedRead_ShouldRead()
    {
        var bitReader = new BitReader(new byte[] {0b_00001111, 0b_11110000, 0b_01010101, 0b_10101010});

        var value = bitReader.ReadUInt();
        value.Should().Be(0b_10101010_01010101_11110000_00001111);
    }
    
    [Fact]
    public void ReadUInt_UnAlignedRead_ShouldRead()
    {
        var bitReader = new BitReader(new byte[] {0b_00011111, 0b_11100000, 0b_10101011, 0b_01010100, 0b_1});

        var readBits = bitReader.ReadBits(1);
        readBits.Should().Be(1);
        
        var value = bitReader.ReadUInt();
        value.Should().Be(0b_10101010_01010101_11110000_00001111);
    }

    [Fact]
    public void ReadBool_ArrayWithSingleSetBit_ShouldReadTrue()
    {
        var bitReader = new BitReader(new byte[] { 0b_1 });

        var value = bitReader.ReadBool();
        value.Should().Be(true);
    }
    
    [Fact]
    public void ReadBool_ArrayWithSingleEmptyBit_ShouldReadFalse()
    {
        var bitReader = new BitReader(new byte[] { 0b_0 });

        var value = bitReader.ReadBool();
        value.Should().Be(false);
    }
}