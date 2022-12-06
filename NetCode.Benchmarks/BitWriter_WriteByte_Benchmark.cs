using BenchmarkDotNet.Attributes;
using NetStack.Serialization;

namespace NetCode.Benchmarks;

/// <summary>
/// BenchmarkDotNet=v0.13.1, OS=macOS Big Sur 11.5.2 (20G95) [Darwin 20.6.0]
/// Intel Core i7-9750H CPU 2.60GHz, 1 CPU, 12 logical and 6 physical cores
/// .NET SDK=6.0.100
/// [Host]     : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT
/// DefaultJob : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT
/// 
/// |                      Method |     Mean |   Error |  StdDev | Ratio | RatioSD |
/// |---------------------------- |---------:|--------:|--------:|------:|--------:|
/// |   BitWriter_Align_WriteByte | 392.4 ns | 1.38 ns | 1.22 ns |  1.00 |    0.00 |
/// | BitWriter_UnAlign_WriteByte | 651.4 ns | 3.38 ns | 3.16 ns |  1.66 |    0.01 |
/// |     BitBuffer_Align_AddByte | 995.3 ns | 5.96 ns | 5.28 ns |  2.54 |    0.01 |
/// |   BitBuffer_UnAlign_AddByte | 996.1 ns | 6.84 ns | 6.40 ns |  2.54 |    0.02 |
/// 
/// </summary>
public class BitWriter_WriteByte_Benchmark
{
    private const int WriteCount = 255;
    private const int BitsPerWrite = 8;
    
    private BitWriter _bitWriter;
    private BitBuffer _bitBuffer;

    private byte[] _resultArray;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _bitWriter = new BitWriter();
        _bitBuffer = new BitBuffer();

        var arrayLength = (int)Math.Ceiling((float) WriteCount * BitsPerWrite / 8);
        _resultArray = new byte[arrayLength];
    }

    [Benchmark(Baseline = true)]
    public byte BitWriter_Align_WriteByte()
    {
        _bitWriter.Clear();

        for (byte i = 0; i < WriteCount; i++)
        {
            _bitWriter.Write(i);
        }
        
        _bitWriter.Flush();
        
        return _bitWriter.Array[0];
    }
    
    [Benchmark]
    public byte BitWriter_UnAlign_WriteByte()
    {
        _bitWriter.Clear();
        _bitWriter.WriteBits(1, 1);
        
        for (byte i = 0; i < WriteCount; i++)
        {
            _bitWriter.Write(i);
        }
        
        _bitWriter.Flush();
        
        return _bitWriter.Array[0];
    }
    
    [Benchmark]
    public byte BitBuffer_Align_AddByte()
    {
        _bitBuffer.Clear();

        for (byte i = 0; i < WriteCount; i++)
        {
            _bitBuffer.AddByte(i);
        }

        _bitBuffer.ToArray(_resultArray);
        
        return _resultArray[0];
    }
    
    [Benchmark]
    public byte BitBuffer_UnAlign_AddByte()
    {
        _bitBuffer.Clear();
        _bitBuffer.Add(1, 1);

        for (byte i = 0; i < WriteCount; i++)
        {
            _bitBuffer.AddByte(i);
        }

        _bitBuffer.ToArray(_resultArray);

        return _resultArray[0];
    }
}