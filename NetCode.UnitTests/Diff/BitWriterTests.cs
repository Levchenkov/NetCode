using System;
using System.Numerics;
using FluentAssertions;
using Xunit;

namespace NetCode.UnitTests.Diff;

public class BitWriterTests
{
    [Fact]
    public void WriteValueIfChanged_IntBaseAndUpdatedAreTheSame_ArrayShouldContainSingle0Bit()
    {
        var writer = new BitWriter();
        
        writer.WriteValueIfChanged(0b_1010, 0b_1010);
        writer.BitsCount.Should().Be(1);
        
        writer.Flush();

        writer.Array[0].Should().Be(0b_0);
    }
    
    [Fact]
    public void WriteValueIfChanged_IntBaseAndUpdatedAreDifferent_ArrayShouldContainBit1AndUpdatedValue()
    {
        var writer = new BitWriter();
        
        writer.WriteValueIfChanged(0b_1010, 0b_1011);
        writer.BitsCount.Should().Be(33);
        
        writer.Flush();

        writer.Array[0].Should().Be(0b_1_0111);
    }
    
    [Fact]
    public void WriteValueIfChanged_UIntBaseAndUpdatedAreTheSame_ArrayShouldContainSingle0Bit()
    {
        var writer = new BitWriter();
        
        writer.WriteValueIfChanged(0b_1010u, 0b_1010u);
        writer.BitsCount.Should().Be(1);
        
        writer.Flush();

        writer.Array[0].Should().Be(0b_0);
    }
    
    [Fact]
    public void WriteValueIfChanged_UIntBaseAndUpdatedAreDifferent_ArrayShouldContainBit1AndUpdatedValue()
    {
        var writer = new BitWriter();
        
        writer.WriteValueIfChanged(0b_1010u, 0b_1011u);
        writer.BitsCount.Should().Be(33);
        
        writer.Flush();

        writer.Array[0].Should().Be(0b_1_0111);
    }
    
    [Fact]
    public void WriteValueIfChanged_FloatBaseAndUpdatedAreTheSame_ArrayShouldContainSingle0Bit()
    {
        var writer = new BitWriter();
        
        writer.WriteValueIfChanged((float)0b_1010, (float)0b_1010);
        writer.BitsCount.Should().Be(1);
        
        writer.Flush();

        writer.Array[0].Should().Be(0b_0);
    }
    
    [Fact]
    public void WriteValueIfChanged_FloatBaseAndUpdatedAreDifferent_ArrayShouldContainBit1AndUpdatedValue()
    {
        var writer = new BitWriter();

        var updated = BitConverter.UInt32BitsToSingle(0b_10101010_11110000_11001100_01010101);
        var baseline = updated - 1f;
        writer.WriteValueIfChanged(baseline, updated);
        writer.BitsCount.Should().Be(33);
        
        writer.Flush();

        writer.Array[0].Should().Be(0b_10101011);
    }
}