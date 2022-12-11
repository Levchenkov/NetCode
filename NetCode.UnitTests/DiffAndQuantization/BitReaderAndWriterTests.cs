using FluentAssertions;
using NetCode.Limits;
using Xunit;

namespace NetCode.UnitTests.DiffAndQuantization;

public class BitReaderAndWriterTests
{
    [Fact]
    public void WriteValueIfChangedAndReadInt_ValueChanged_ReadValueShouldBeEqualToUpdated()
    {
        var writer = new BitWriter();
        
        writer.WriteValueIfChanged(10, 50, Limits.IntLimit);
        writer.Flush();

        var data = writer.Array;

        var reader = new BitReader(data);

        reader.ReadInt(10, Limits.IntLimit).Should().Be(50);
    }
    
    [Fact]
    public void WriteValueIfChangedAndReadInt_ValueIsNotChanged_ReadValueShouldBeEqualToBaseline()
    {
        var writer = new BitWriter();
        
        writer.WriteValueIfChanged(10, 10, Limits.IntLimit);
        writer.Flush();

        var data = writer.Array;

        var reader = new BitReader(data);

        reader.ReadInt(10, Limits.IntLimit).Should().Be(10);
    }
    
    [Fact]
    public void WriteValueIfChangedAndReadUInt_ValueChanged_ReadValueShouldBeEqualToUpdated()
    {
        var writer = new BitWriter();
        
        writer.WriteValueIfChanged(10u, 50u, Limits.UIntLimit);
        writer.Flush();

        var data = writer.Array;

        var reader = new BitReader(data);

        reader.ReadUInt(10u, Limits.UIntLimit).Should().Be(50);
    }
    
    [Fact]
    public void WriteValueIfChangedAndReadUInt_ValueIsNotChanged_ReadValueShouldBeEqualToBaseline()
    {
        var writer = new BitWriter();
        
        writer.WriteValueIfChanged(10u, 10u, Limits.UIntLimit);
        writer.Flush();

        var data = writer.Array;

        var reader = new BitReader(data);

        reader.ReadUInt(10u, Limits.UIntLimit).Should().Be(10);
    }
    
    [Fact]
    public void WriteValueIfChangedAndReadFloat_ValueChanged_ReadValueShouldBeEqualToUpdated()
    {
        var writer = new BitWriter();
        
        writer.WriteValueIfChanged(1.0f, 5.0f, Limits.FloatLimit);
        writer.Flush();

        var data = writer.Array;

        var reader = new BitReader(data);

        reader.ReadFloat(1.0f, Limits.FloatLimit).Should().BeApproximately(5.0f, Limits.FloatLimit.Precision);
    }
    
    [Fact]
    public void WriteValueIfChangedAndReadFloat_ValueIsNotChanged_ReadValueShouldBeEqualToBaseline()
    {
        var writer = new BitWriter();
        
        writer.WriteValueIfChanged(1.0f, 1.0f, Limits.FloatLimit);
        writer.Flush();

        var data = writer.Array;

        var reader = new BitReader(data);

        reader.ReadFloat(1.0f, Limits.FloatLimit).Should().BeApproximately(1.0f, Limits.FloatLimit.Precision);
    }
    
    [Fact]
    public void WriteDiffIfChangedAndReadInt_DiffSuitsDiffLimits_ReadValueShouldBeEqualToWritten()
    {
        var writer = new BitWriter();
        
        writer.WriteDiffIfChanged(10, 11, Limits.IntLimit, DiffLimits.IntLimit);

        writer.BitsCount.Should().Be(2 + 3);
        writer.Flush();

        var data = writer.Array;

        var reader = new BitReader(data);

        reader.ReadInt(10, Limits.IntLimit, DiffLimits.IntLimit).Should().Be(11);
    }
    
    [Fact]
    public void WriteDiffIfChangedAndReadInt_DiffDoesNotSuitDiffLimits_ReadValueShouldBeEqualToWritten()
    {
        var writer = new BitWriter();
        
        writer.WriteDiffIfChanged(10, 50, Limits.IntLimit, DiffLimits.IntLimit);

        writer.BitsCount.Should().Be(2 + 7);
        writer.Flush();

        var data = writer.Array;

        var reader = new BitReader(data);

        reader.ReadInt(10, Limits.IntLimit, DiffLimits.IntLimit).Should().Be(50);
    }
    
    [Fact]
    public void WriteDiffIfChangedAndReadInt_ValueIsNotChanged_ReadValueShouldBeEqualToBaseline()
    {
        var writer = new BitWriter();
        
        writer.WriteDiffIfChanged(10, 10, Limits.IntLimit, DiffLimits.IntLimit);

        writer.BitsCount.Should().Be(1);
        writer.Flush();

        var data = writer.Array;

        var reader = new BitReader(data);

        reader.ReadInt(10, Limits.IntLimit, DiffLimits.IntLimit).Should().Be(10);
    }
    
    [Fact]
    public void WriteDiffIfChangedAndReadFloat_DiffSuitsDiffLimits_ReadValueShouldBeEqualToWritten()
    {
        var writer = new BitWriter();
        
        writer.WriteDiffIfChanged(1.0f, 1.1f, Limits.FloatLimit, DiffLimits.FloatLimit);

        writer.BitsCount.Should().Be(2 + 5);
        writer.Flush();

        var data = writer.Array;

        var reader = new BitReader(data);

        reader.ReadFloat(1.0f, Limits.FloatLimit, DiffLimits.FloatLimit).Should().BeApproximately(1.1f, DiffLimits.FloatLimit.Precision);
    }
    
    [Fact]
    public void WriteDiffIfChangedAndReadFloat_DiffDoesNotSuitDiffLimits_ReadValueShouldBeEqualToWritten()
    {
        var writer = new BitWriter();
        
        writer.WriteDiffIfChanged(1.0f, 5.0f, Limits.FloatLimit, DiffLimits.FloatLimit);

        writer.BitsCount.Should().Be(2 + 7);
        writer.Flush();

        var data = writer.Array;

        var reader = new BitReader(data);

        reader.ReadFloat(1.0f, Limits.FloatLimit, DiffLimits.FloatLimit).Should().BeApproximately(5.0f, DiffLimits.FloatLimit.Precision);
    }
    
    [Fact]
    public void WriteDiffIfChangedAndReadFloat_ValueIsNotChanged_ReadValueShouldBeEqualToBaseline()
    {
        var writer = new BitWriter();
        
        writer.WriteDiffIfChanged(1.0f, 1.0f, Limits.FloatLimit, DiffLimits.FloatLimit);

        writer.BitsCount.Should().Be(1);
        writer.Flush();

        var data = writer.Array;

        var reader = new BitReader(data);

        reader.ReadFloat(1.0f, Limits.FloatLimit, DiffLimits.FloatLimit).Should().BeApproximately(1.0f, DiffLimits.FloatLimit.Precision);
    }

    public static class Limits
    {
        public static IntLimit IntLimit = new IntLimit(0, 100);

        public static UIntLimit UIntLimit = new UIntLimit(0, 100);

        public static FloatLimit FloatLimit = new FloatLimit(0f, 10f, 0.1f);
    }
    
    public static class DiffLimits
    {
        public static IntLimit IntLimit = new IntLimit(-2, 2);

        public static FloatLimit FloatLimit = new FloatLimit(-1f, 1f, 0.1f);
    }
}