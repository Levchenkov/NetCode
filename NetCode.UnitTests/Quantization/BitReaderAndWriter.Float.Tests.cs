using System;
using FluentAssertions;
using Xunit;

namespace NetCode.UnitTests.Quantization;

public class BitReaderAndWriter_Float_Tests
{
    [Theory]
    [InlineData(1f, 0f, 10f, 1f, 4)]
    [InlineData(1f, 0f, 10f, 0.1f, 7)]
    [InlineData(1f, 0f, 10f, 0.01f, 10)]
    [InlineData(1f, 0f, 10f, 0.001f, 14)]
    [InlineData(1f, 0f, 10f, 0.0001f, 17)]
    
    [InlineData(0f, 0f, 10f, 1f, 4)]
    [InlineData(0f, 0f, 10f, 0.1f, 7)]
    [InlineData(0f, 0f, 10f, 0.01f, 10)]
    [InlineData(0f, 0f, 10f, 0.001f, 14)]
    [InlineData(0f, 0f, 10f, 0.0001f, 17)]
    
    [InlineData(10f, 0f, 10f, 1f, 4)]
    [InlineData(10f, 0f, 10f, 0.1f, 7)]
    [InlineData(10f, 0f, 10f, 0.01f, 10)]
    [InlineData(10f, 0f, 10f, 0.001f, 14)]
    [InlineData(10f, 0f, 10f, 0.0001f, 17)]
    
    [InlineData(0f, -10f, 10f, 1f, 5)]
    [InlineData(0f, -10f, 10f, 0.1f, 8)]
    [InlineData(0f, -10f, 10f, 0.01f, 11)]
    [InlineData(0f, -10f, 10f, 0.001f, 15)]
    [InlineData(0f, -10f, 10f, 0.0001f, 18)]
    
    [InlineData(-1f, -10f, 0f, 1f, 4)]
    [InlineData(-1f, -10f, 0f, 0.1f, 7)]
    [InlineData(-1f, -10f, 0f, 0.01f, 10)]
    [InlineData(-1f, -10f, 0f, 0.001f, 14)]
    [InlineData(-1f, -10f, 0f, 0.0001f, 17)]
    
    [InlineData(-10f, -10f, 0f, 1f, 4)]
    [InlineData(-10f, -10f, 0f, 0.1f, 7)]
    [InlineData(-10f, -10f, 0f, 0.01f, 10)]
    [InlineData(-10f, -10f, 0f, 0.001f, 14)]
    [InlineData(-10f, -10f, 0f, 0.0001f, 17)]
    
    [InlineData(0f, -10f, 0f, 1f, 4)]
    [InlineData(0f, -10f, 0f, 0.1f, 7)]
    [InlineData(0f, -10f, 0f, 0.01f, 10)]
    [InlineData(0f, -10f, 0f, 0.001f, 14)]
    [InlineData(0f, -10f, 0f, 0.0001f, 17)]

    [InlineData(0f, 0f, 1f, 1f, 1)]
    [InlineData(0f, 0f, 1f, 0.1f, 4)]
    [InlineData(0f, 0f, 1f, 0.01f, 7)]
    [InlineData(0f, 0f, 1f, 0.001f, 10)]
    [InlineData(0f, 0f, 1f, 0.0001f, 14)]
    
    [InlineData(0f, -1f, 0f, 1f, 1)]
    [InlineData(0f, -1f, 0f, 0.1f, 4)]
    [InlineData(0f, -1f, 0f, 0.01f, 7)]
    [InlineData(0f, -1f, 0f, 0.001f, 10)]
    [InlineData(0f, -1f, 0f, 0.0001f, 14)]
    
    [InlineData(1f, 1f, 2f, 1f, 1)]
    [InlineData(1f, 1f, 2f, 0.1f, 4)]
    [InlineData(1f, 1f, 2f, 0.01f, 7)]
    [InlineData(1f, 1f, 2f, 0.001f, 10)]
    [InlineData(1f, 1f, 2f, 0.0001f, 14)]
    
    [InlineData(2f, 1f, 2f, 1f, 1)]
    [InlineData(2f, 1f, 2f, 0.1f, 4)]
    [InlineData(2f, 1f, 2f, 0.01f, 7)]
    [InlineData(2f, 1f, 2f, 0.001f, 10)]
    [InlineData(2f, 1f, 2f, 0.0001f, 14)]
    
    [InlineData(-2f, -2f, -1f, 1f, 1)]
    [InlineData(-2f, -2f, -1f, 0.1f, 4)]
    [InlineData(-2f, -2f, -1f, 0.01f, 7)]
    [InlineData(-2f, -2f, -1f, 0.001f, 10)]
    [InlineData(-2f, -2f, -1f, 0.0001f, 14)]
    
    [InlineData(-1f, -2f, -1f, 1f, 1)]
    [InlineData(-1f, -2f, -1f, 0.1f, 4)]
    [InlineData(-1f, -2f, -1f, 0.01f, 7)]
    [InlineData(-1f, -2f, -1f, 0.001f, 10)]
    [InlineData(-1f, -2f, -1f, 0.0001f, 14)]
    public void WriteReadFloat_PositiveCases_ValueShouldBeApproximatelyTheSame(float value, float min, float max, float precision, int bitCount)
    {
        var writer = new BitWriter();

        writer.Write(value, min, max, precision);
        writer.BitsCount.Should().Be(bitCount);
        
        writer.Flush();

        var data = writer.Array;

        var reader = new BitReader(data);
        reader.ReadFloat(min, max, precision).Should().BeApproximately(value, precision);
    }

    [Theory]
    [InlineData(0f, 0f, 0f, 1f)]
    [InlineData(0f, 0f, 0f, 0.1f)]
    [InlineData(0f, 0f, 0f, 0.01f)]
    [InlineData(0f, 0f, 0f, 0.001f)]
    [InlineData(0f, 0f, 0f, 0.0001f)]
    
    [InlineData(1f, 1f, 1f, 1f)]
    [InlineData(1f, 1f, 1f, 0.1f)]
    [InlineData(1f, 1f, 1f, 0.01f)]
    [InlineData(1f, 1f, 1f, 0.001f)]
    [InlineData(1f, 1f, 1f, 0.0001f)]
    
    [InlineData(-1f, -1f, -1f, 1f)]
    [InlineData(-1f, -1f, -1f, 0.1f)]
    [InlineData(-1f, -1f, -1f, 0.01f)]
    [InlineData(-1f, -1f, -1f, 0.001f)]
    [InlineData(-1f, -1f, -1f, 0.0001f)]
    public void WriteReadFloat_MinAndMaxAreTheSame_ExceptionExpected(float value, float min, float max, float precision)
    {
        var writer = new BitWriter();

        Action action = () => writer.Write(value, min, max, precision);

        action.Should().Throw<ArgumentException>();
    }
    
    [Theory]
    [InlineData(2f, 0f, 1f, 0.1f)]
    [InlineData(-1f, 0f, 1f, 0.1f)]
    
    [InlineData(-2f, -1f, 0f, 0.1f)]
    [InlineData(1f, -1f, 0f, 0.1f)]
    
    [InlineData(10f, 1f, 5f, 0.1f)]
    [InlineData(-10f, -5f, -1f, 0.1f)]
    
    [InlineData(-10f, -5f, 5f, 0.1f)]
    [InlineData(10f, -5f, 5f, 0.1f)]
    public void WriteFloat_ValueOutOfLimitRangeForDebug_ExceptionExpected(float value, float min, float max, float precision)
    {
        #if !DEBUG
        return;
        #endif
        
        var writer = new BitWriter();

        Action action = () => writer.Write(value, min, max, precision);

        action.Should().Throw<ArgumentOutOfRangeException>();
    }
    
    [Theory]
    [InlineData(1f, 1f, 0f, 0.1f)]
    [InlineData(0f, 1f, 0f, 0.1f)]
    [InlineData(-1f, 0f, -1f, 0.1f)]
    [InlineData(0f, 0f, -1f, 0.1f)]
    public void WriteFloat_RangeIsNotValidForDebug_ExceptionExpected(float value, float min, float max, float precision)
    {
        var writer = new BitWriter();

        Action action = () => writer.Write(value, min, max, precision);

        action.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(2f, 0f, 1f, 0.1f)]
    [InlineData(-1f, 0f, 1f, 0.1f)]
    
    [InlineData(-2f, -1f, 0f, 0.1f)]
    [InlineData(1f, -1f, 0f, 0.1f)]
    
    [InlineData(10f, 1f, 5f, 0.1f)]
    [InlineData(-10f, -5f, -1f, 0.1f)]
    
    [InlineData(-10f, -5f, 5f, 0.1f)]
    [InlineData(10f, -5f, 5f, 0.1f)]
    public void WriteFloat_ValueOutOfLimitRangeForRelease_ShouldNotThrow(float value, float min, float max, float precision)
    {
#if DEBUG
        return;
#endif
        
        var writer = new BitWriter();

        Action action = () =>
        {
            writer.Write(value, min, max, precision);
            writer.Flush();
        };

        action.Should().NotThrow();
    }
}