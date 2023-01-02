using FluentAssertions;
using Xunit;

namespace NetCode.UnitTests;

public class SevenBitEncodingTests
{
    [Fact]
    public void WriteCompressed_WriteUInt1ByteNumber_BytesCountShouldBe1()
    {
        var bitWriter = new BitWriter();
        bitWriter.WriteCompressed((uint)16);
        bitWriter.Flush();

        bitWriter.BytesCount.Should().Be(1);
    }
    
    [Fact]
    public void WriteCompressed_WriteUInt7BitValue_BytesCountShouldBe1()
    {
        var bitWriter = new BitWriter();
        bitWriter.WriteCompressed((uint)0b_00000000_00000000_00000000_01111111);
        bitWriter.Flush();

        bitWriter.BytesCount.Should().Be(1);
    }
    
    [Fact]
    public void WriteCompressed_WriteUInt8BitValue_BytesCountShouldBe2()
    {
        var bitWriter = new BitWriter();
        bitWriter.WriteCompressed((uint)0b_00000000_00000000_00000000_11111111);
        bitWriter.Flush();

        bitWriter.BytesCount.Should().Be(2);
    }
    
    [Fact]
    public void WriteCompressed_WriteUInt14BitValue_BytesCountShouldBe2()
    {
        var bitWriter = new BitWriter();
        bitWriter.WriteCompressed((uint)0b_00000000_00000000_00111111_11111111);
        bitWriter.Flush();

        bitWriter.BytesCount.Should().Be(2);
    }
    
    [Fact]
    public void WriteCompressed_WriteUInt15BitValue_BytesCountShouldBe3()
    {
        var bitWriter = new BitWriter();
        bitWriter.WriteCompressed((uint)0b_00000000_00000000_01111111_11111111);
        bitWriter.Flush();

        bitWriter.BytesCount.Should().Be(3);
    }
    
    [Fact]
    public void WriteCompressed_WriteUInt21BitValue_BytesCountShouldBe3()
    {
        var bitWriter = new BitWriter();
        bitWriter.WriteCompressed((uint)0b_00000000_00011111_11111111_11111111);
        bitWriter.Flush();

        bitWriter.BytesCount.Should().Be(3);
    }
    
    [Fact]
    public void WriteCompressed_WriteUInt22BitValue_BytesCountShouldBe4()
    {
        var bitWriter = new BitWriter();
        bitWriter.WriteCompressed((uint)0b_00000000_00111111_11111111_11111111);
        bitWriter.Flush();

        bitWriter.BytesCount.Should().Be(4);
    }
    
    [Fact]
    public void WriteCompressed_WriteUInt28BitValue_BytesCountShouldBe4()
    {
        var bitWriter = new BitWriter();
        bitWriter.WriteCompressed((uint)0b_00001111_11111111_11111111_11111111);
        bitWriter.Flush();

        bitWriter.BytesCount.Should().Be(4);
    }
    
    [Fact]
    public void WriteCompressed_WriteUInt29BitValue_BytesCountShouldBe5()
    {
        var bitWriter = new BitWriter();
        bitWriter.WriteCompressed((uint)0b_00011111_11111111_11111111_11111111);
        bitWriter.Flush();

        bitWriter.BytesCount.Should().Be(5);
    }
}