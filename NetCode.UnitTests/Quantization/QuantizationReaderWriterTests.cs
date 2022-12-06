using FluentAssertions;
using Xunit;

namespace NetCode.UnitTests.Quantization;

public class QuantizationReaderWriterTests
{
    [Fact]
    public void WriteAndReadTheSameData()
    {
        var floatLimit = new FloatLimit(50f, 200f, 0.1f);
        var array = new byte[100];
        var writer = new QuantizationBitWriter(array);

        writer.Write(100, 50, 200);
        writer.Write(100f, floatLimit);
        writer.Flush();

        var data = writer.Array;

        var reader = new QuantizationBitReader(data);
        reader.ReadInt(50, 200).Should().Be(100);
        reader.ReadFloat(floatLimit).Should().BeApproximately(100f, 0.1f);
    }
}