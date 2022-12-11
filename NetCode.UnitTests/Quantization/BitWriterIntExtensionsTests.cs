using FluentAssertions;
using Xunit;

namespace NetCode.UnitTests.Quantization;

public class BitWriterIntExtensionsTests
{
    [Fact]
    public void WriteValueMinMax_SmallValueIsLimitedTo8PossibleValues_ShouldWrite3Bits()
    {
        var writer = new BitWriter();
        
        writer.Write(5, 0, 7);
    
        writer.BitsCount.Should().Be(3);
        writer.Flush();
    
        writer.BytesCount.Should().Be(1);
        
        writer.Array[0].Should().Be(5);
    }
    
    [Fact]
    public void WriteValueMinMax_MiddleValueIsLimitedTo8PossibleValues_ShouldWrite3Bits()
    {
        var writer = new BitWriter();
        
        writer.Write(100501, 100500, 100507);
    
        writer.BitsCount.Should().Be(3);
        writer.Flush();
    
        writer.BytesCount.Should().Be(1);
        
        writer.Array[0].Should().Be(0b_001);
    }
    
    [Fact]
    public void WriteValueMinMax_BigValueIsLimitedTo8PossibleValues_ShouldWrite3Bits()
    {
        var writer = new BitWriter();
        
        writer.Write(int.MaxValue - 1, int.MaxValue - 7, int.MaxValue);
    
        writer.BitsCount.Should().Be(3);
        writer.Flush();
    
        writer.BytesCount.Should().Be(1);
        
        writer.Array[0].Should().Be(0b_110);
    }
    
    [Fact]
    public void WriteValueMinMax_NegativeValueIsLimitedTo8PossibleValues_ShouldWrite3Bits()
    {
        var writer = new BitWriter();
        
        writer.Write(int.MinValue + 1, int.MinValue, int.MinValue + 7);
    
        writer.BitsCount.Should().Be(3);
        writer.Flush();
    
        writer.BytesCount.Should().Be(1);
        
        writer.Array[0].Should().Be(0b_001);
    }
    
    [Fact]
    public void WriteValueMinMax_ValueIsLimitedTo256PossibleValues_ShouldWrite8Bits()
    {
        var writer = new BitWriter();
        
        writer.Write(254, 0, 255);
    
        writer.BitsCount.Should().Be(8);
        writer.Flush();
    
        writer.BytesCount.Should().Be(1);
        
        writer.Array[0].Should().Be(254);
    }
    
    [Fact]
    public void WriteValueMinMax_ValueIsLimitedTo257PossibleValues_ShouldWrite9Bits()
    {
        var writer = new BitWriter();
        
        writer.Write(255, 0, 256);
    
        writer.BitsCount.Should().Be(9);
        writer.Flush();
    
        writer.BytesCount.Should().Be(2);
        
        writer.Array[0].Should().Be(0b_1111_1111);
        writer.Array[1].Should().Be(0b_0000_0000);
    }
    
    [Fact]
    public void WriteValueMinMax_ValueIsLimitedTo258PossibleValues_ShouldWrite9Bits()
    {
        var writer = new BitWriter();
        
        writer.Write(256, 0, 257);
    
        writer.BitsCount.Should().Be(9);
        writer.Flush();
    
        writer.BytesCount.Should().Be(2);
        
        writer.Array[0].Should().Be(0b_0000_0000);
        writer.Array[1].Should().Be(0b_0000_0001);
    }

}