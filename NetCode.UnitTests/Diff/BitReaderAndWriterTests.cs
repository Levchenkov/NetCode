using FluentAssertions;
using Xunit;

namespace NetCode.UnitTests.Diff;

public class BitReaderAndWriterTests
{
    [Fact]
    public void WriteAndReadInt()
    {
        var writer = new BitWriter();
        
        writer.WriteValueIfChanged(41, 42);
        writer.Flush();
        
        var data = writer.Array;

        var reader = new BitReader(data);
        var updated = reader.ReadInt(41);

        updated.Should().Be(42);
    }
    
    [Fact]
    public void WriteAndReadUInt()
    {
        var writer = new BitWriter();
        
        writer.WriteValueIfChanged(41u, 42u);
        writer.Flush();
        
        var data = writer.Array;

        var reader = new BitReader(data);
        var updated = reader.ReadUInt(41u);

        updated.Should().Be(42u);
    }
    
    [Fact]
    public void WriteAndReadFloat()
    {
        var writer = new BitWriter();

        var baseline = 42f;
        var updated = 42.42f;
        
        writer.WriteValueIfChanged(baseline, updated);
        writer.Flush();
        
        var data = writer.Array;

        var reader = new BitReader(data);

        reader.ReadFloat(baseline).Should().Be(updated);
    }
}