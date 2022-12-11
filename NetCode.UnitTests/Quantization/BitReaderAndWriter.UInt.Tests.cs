using System;
using FluentAssertions;
using Xunit;

namespace NetCode.UnitTests.Quantization;

public class BitReaderAndWriter_UInt_Tests
{
    [Theory]
    [InlineData(1, 0, 10, 4)]
    [InlineData(0, 0, 10, 4)]
    [InlineData(10, 0, 10, 4)]
    
    [InlineData(0, 0, 0, 1)]
    [InlineData(1, 1, 1, 1)]
    
    [InlineData(0, 0, 1, 1)]
    
    [InlineData(1, 1, 2, 1)]
    [InlineData(2, 1, 2, 1)]
    
    [InlineData(0, uint.MinValue, uint.MaxValue, 32)]
    [InlineData(42, uint.MinValue, uint.MaxValue, 32)]
    [InlineData(uint.MinValue, uint.MinValue, uint.MaxValue, 32)]
    [InlineData(uint.MaxValue, uint.MinValue, uint.MaxValue, 32)]
    public void WriteReadUInt_PositiveCases_ValueShouldBeTheSame(uint value, uint min, uint max, int bitCount)
    {
        var writer = new BitWriter();

        writer.Write(value, min, max);
        writer.BitsCount.Should().Be(bitCount);
        
        writer.Flush();

        var data = writer.Array;

        var reader = new BitReader(data);
        reader.ReadUInt(min, max).Should().Be(value);
    }
    
    [Theory]
    [InlineData(1, 0, 0)]
    [InlineData(2, 1, 1)]
    [InlineData(2, 0, 1)]
    [InlineData(10, 1, 5)]
    public void WriteUInt_ValueOutOfLimitRangeForDebug_ExceptionExpected(uint value, uint min, uint max)
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
    public void WriteUInt_RangeIsNotValidForDebug_ExceptionExpected(uint value, uint min, uint max)
    {
        var writer = new BitWriter();

        Action action = () => writer.Write(value, min, max);

        action.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(1, 0, 0)]
    [InlineData(2, 1, 1)]
    [InlineData(2, 0, 1)]
    [InlineData(10, 1, 5)]
    public void WriteUInt_ValueOutOfLimitRangeForRelease_ShouldNotThrow(uint value, uint min, uint max)
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