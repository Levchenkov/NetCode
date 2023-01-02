using System;
using System.Numerics;
using FluentAssertions;
using Xunit;

namespace NetCode.UnitTests;

public class BitReaderAndWriterTests
{
    private static byte @byte = 0b_11110000;
    private static ushort @short = 0b_11110000_00001111;
    private static uint @int = 0b_10101010_01010101_11110000_00001111;
    private static float @float = BitConverter.UInt32BitsToSingle(@int);
    
    [Fact]
    public void WriteAndReadTheSameData()
    {
        var array = new byte[100];
        var bitWriter = new BitWriter(array);
        
        bitWriter.WriteBits(19, 0b_10101010_01010101_11110000_00001111);
        bitWriter.Write(@byte);
        bitWriter.Write(@short);
        bitWriter.Write(@int);
        bitWriter.Write(@float);
        bitWriter.Flush();

        var data = bitWriter.Array;
        data.Should().BeSameAs(array);

        var bitReader = new BitReader(data);
        
        bitReader.ReadBits(19).Should().Be(0b_101_11110000_00001111);
        bitReader.ReadByte().Should().Be(@byte);
        bitReader.ReadUShort().Should().Be(@short);
        bitReader.ReadUInt().Should().Be(@int);
        bitReader.ReadFloat().Should().Be(@float);
    }

    [Fact]
    public void WriteReadString_Utf8_ShouldBeTheSame()
    {
        var array = new byte[100];
        var bitWriter = new BitWriter(array);

        var s = "qwertyuiopasdfghjklzxcvbnm";
        bitWriter.WriteUtf8String(s);
        bitWriter.Flush();
        
        var data = bitWriter.Array;
        var length = bitWriter.BytesCount;
        length.Should().Be(s.Length + 1);

        var bitReader = new BitReader(data);
        var result = bitReader.ReadUtf8String();
        result.Should().Be(s);
        string.IsInterned(result).Should().Be(result);
    }
    
    [Fact]
    public void WriteReadString_Unicode_ShouldBeTheSame()
    {
        var array = new byte[100];
        var bitWriter = new BitWriter(array);

        var s = "qwertyuiopasdfghjklzxcvbnm";
        bitWriter.WriteUnicodeString(s);
        bitWriter.Flush();
        
        var data = bitWriter.Array;
        var length = bitWriter.BytesCount;
        length.Should().Be(s.Length * 2 + 1);

        var bitReader = new BitReader(data);
        var result = bitReader.ReadUnicodeString();
        result.Should().Be(s);
        string.IsInterned(result).Should().Be(result);
    }
}