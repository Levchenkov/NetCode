using System;
using FluentAssertions;
using Xunit;

namespace NetCode.UnitTests.Quantization;

public class QuantizationIntReaderWriterTests
{
    [Theory]
    [InlineData(1, 0, 10, 4)]
    [InlineData(0, 0, 10, 4)]
    [InlineData(10, 0, 10, 4)]
    
    [InlineData(0, -10, 10, 5)]
    
    [InlineData(-1, -10, 0, 4)]
    [InlineData(-10, -10, 0, 4)]
    [InlineData(0, -10, 0, 4)]
    
    [InlineData(0, 0, 0, 1)]
    [InlineData(1, 1, 1, 1)]
    [InlineData(-1, -1, -1, 1)]
    
    [InlineData(0, 0, 1, 1)]
    [InlineData(0, -1, 0, 1)]
    
    [InlineData(1, 1, 2, 1)]
    [InlineData(2, 1, 2, 1)]
    
    [InlineData(-2, -2, -1, 1)]
    [InlineData(-1, -2, -1, 1)]
    
    [InlineData(0, 0, int.MaxValue, 31)]
    [InlineData(42, 0, int.MaxValue, 31)]
    [InlineData(int.MaxValue, 0, int.MaxValue, 31)]
    
    [InlineData(int.MinValue, int.MinValue, -1, 31)]
    [InlineData(-42, int.MinValue, -1, 31)]
    [InlineData(-1, int.MinValue, -1, 31)]
    
    [InlineData(0, int.MinValue, int.MaxValue, 32)]
    [InlineData(42, int.MinValue, int.MaxValue, 32)]
    [InlineData(-42, int.MinValue, int.MaxValue, 32)]
    [InlineData(int.MinValue, int.MinValue, int.MaxValue, 32)]
    [InlineData(int.MaxValue, int.MinValue, int.MaxValue, 32)]
    public void WriteReadInt_PositiveCases_ValueShouldBeTheSame(int value, int min, int max, int bitCount)
    {
        var writer = new BitWriter();

        writer.Write(value, min, max);
        writer.BitsCount.Should().Be(bitCount);
        
        writer.Flush();

        var data = writer.Array;

        var reader = new BitReader(data);
        reader.ReadInt(min, max).Should().Be(value);
    }
    
    [Theory]
    [InlineData(1, 0, 0)]
    [InlineData(-1, 0, 0)]
    
    [InlineData(2, 1, 1)]
    [InlineData(-2, -1, -1)]
    
    [InlineData(2, 0, 1)]
    [InlineData(-1, 0, 1)]
    
    [InlineData(-2, -1, 0)]
    [InlineData(1, -1, 0)]
    
    [InlineData(10, 1, 5)]
    [InlineData(-10, -5, -1)]
    
    [InlineData(-10, -5, 5)]
    [InlineData(10, -5, 5)]
    public void WriteInt_ValueOutOfLimitRangeForDebug_ExceptionExpected(int value, int min, int max)
    {
        #if !DEBUG
        return;
        #endif
        
        var writer = new BitWriter();

        Action action = () => writer.Write(value, min, max);

        action.Should().Throw<ArgumentOutOfRangeException>();
    }
    
    [Theory]
    [InlineData(1, 1, 0)]
    [InlineData(0, 1, 0)]
    [InlineData(-1, 0, -1)]
    [InlineData(0, 0, -1)]
    public void WriteInt_RangeIsNotValidForDebug_ExceptionExpected(int value, int min, int max)
    {
        var writer = new BitWriter();

        Action action = () => writer.Write(value, min, max);

        action.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(1, 0, 0)]
    [InlineData(-1, 0, 0)]
    
    [InlineData(2, 1, 1)]
    [InlineData(-2, -1, -1)]
    
    [InlineData(2, 0, 1)]
    [InlineData(-1, 0, 1)]
    
    [InlineData(-2, -1, 0)]
    [InlineData(1, -1, 0)]
    
    [InlineData(10, 1, 5)]
    [InlineData(-10, -5, -1)]
    
    [InlineData(-10, -5, 5)]
    [InlineData(10, -5, 5)]
    public void WriteInt_ValueOutOfLimitRangeForRelease_ExceptionExpected(int value, int min, int max)
    {
#if DEBUG
        return;
#endif
        
        var writer = new BitWriter();

        Action action = () =>
        {
            writer.Write(value, min, max);
            writer.Flush();
        };

        action.Should().NotThrow();
    }
}