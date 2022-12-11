using Xunit;

namespace NetCode.UnitTests.Quantization;

public class QuantizationBitWriterTests
{
    [Fact]
    public void T()
    {
        var bitWriter = new BitWriter();
        
        // positive
        bitWriter.Write(1, 0, 10);
        bitWriter.Write(-1, -10, 0);
        
        bitWriter.Write(1, 1, 1);
        bitWriter.Write(1, 1, 2);
        bitWriter.Write(2, 1, 2);
        
        bitWriter.Write(-1, -1, -1);
        bitWriter.Write(-2, -2, 1);
        bitWriter.Write(-1, -2, 1);
        
        bitWriter.Write(0, 0, 0);
        bitWriter.Write(0, 0, 1);
        bitWriter.Write(0, -1, 0);
        
        // negative case for value
        bitWriter.Write(1, 0, 0);
        bitWriter.Write(-1, 0, 0);
        
        bitWriter.Write(2, 1, 1);
        bitWriter.Write(-2, -1, -1);
        
        bitWriter.Write(2, 0, 1);
        bitWriter.Write(-1, 0, 1);
        
        bitWriter.Write(-2, -1, 0);
        bitWriter.Write(1, -1, 0);
        
        bitWriter.Write(10, 1, 5);
        bitWriter.Write(-10, -5, -1);
        
        bitWriter.Write(10, -5, 5);
        bitWriter.Write(-10, -5, 5);
        
        // negative case for range
        
        bitWriter.Write(1, 1, 0);
        bitWriter.Write(0, 1, 0);
        
        bitWriter.Write(-1, 0, -1);
        bitWriter.Write(0, 0, -1);
        
    }
}