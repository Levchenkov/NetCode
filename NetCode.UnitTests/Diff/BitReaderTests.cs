using System;
using FluentAssertions;
using Xunit;

namespace NetCode.UnitTests.Diff;

public class BitReaderTests
{
    [Fact]
    public void ReadInt_ReaderContainsSingle0Bit_UpdatedValueShouldBeTheSameAsBaseline()
    {
        var bitReader = new BitReader(new byte[] {0b_0});

        var updated = bitReader.ReadInt(10);

        updated.Should().Be(10);
    }
    
    [Fact]
    public void ReadInt_ReaderContains1BitAndUpdatedValue_UpdatedValueShouldBeRead()
    {
        var bitReader = new BitReader(new byte[] { 0b_00010101, 0b_00000000, 0b_00000000, 0b_00000000, 0b_00000000 });

        var updated = bitReader.ReadInt(0b_111);

        updated.Should().Be(0b_1010);
    }
    
    [Fact]
    public void ReadUInt_ReaderContainsSingle0Bit_UpdatedValueShouldBeTheSameAsBaseline()
    {
        var bitReader = new BitReader(new byte[] {0b_0});

        var updated = bitReader.ReadUInt(10);

        updated.Should().Be(10);
    }
    
    [Fact]
    public void ReadUInt_ReaderContains1BitAndUpdatedValue_UpdatedValueShouldBeRead()
    {
        var bitReader = new BitReader(new byte[] { 0b_00010101, 0b_00000000, 0b_00000000, 0b_00000000, 0b_00000000 });

        var updated = bitReader.ReadUInt(0b_111);

        updated.Should().Be(0b_1010);
    }
    
    [Fact]
    public void ReadFloat_ReaderContainsSingle0Bit_UpdatedValueShouldBeTheSameAsBaseline()
    {
        var bitReader = new BitReader(new byte[] {0b_0});

        var updated = bitReader.ReadFloat(10f);

        updated.Should().Be(10f);
    }
    
    [Fact]
    public void ReadFloat_ReaderContains1BitAndUpdatedValue_UpdatedValueShouldBeRead()
    {
        var bitReader = new BitReader(new byte[] {  0b_10101011, 0b_10011000, 0b_11100001, 0b_01010101, 0b_00000001});

        var updated = bitReader.ReadFloat(123f);

        updated.Should().Be(BitConverter.UInt32BitsToSingle(0b_10101010_11110000_11001100_01010101));
    }
    
    [Fact]
    public void ReadString_ReaderContainsSingle0Bit_UpdatedValueShouldBeTheSameAsBaseline()
    {
        var bitReader = new BitReader(new byte[] {0b_0});

        var updated = bitReader.ReadString("abc");

        updated.Should().Be("abc");
    }
    
    [Fact]
    public void ReadString_ReaderContains1BitAndUpdatedValue_UpdatedValueShouldBeRead()
    {
        var bitReader = new BitReader(new byte[] {  0b_00001101, 0b_11000010, 0b_11000100, 0b_11000110, 0b_0});
    
        var updated = bitReader.ReadString("abb");
    
        updated.Should().Be("abc");
    }
}