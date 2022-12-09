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
}