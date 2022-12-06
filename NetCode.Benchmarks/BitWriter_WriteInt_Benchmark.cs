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
/// |                     Method |       Mean |    Error |   StdDev | Ratio | RatioSD |
/// |--------------------------- |-----------:|---------:|---------:|------:|--------:|
/// |   BitWriter_Align_IntWrite |   520.6 ns |  3.64 ns |  3.40 ns |  1.00 |    0.00 |
/// | BitWriter_UnAlign_IntWrite |   851.2 ns |  6.46 ns |  5.72 ns |  1.63 |    0.02 |
/// |     BitBuffer_Align_AddInt | 2,118.7 ns | 13.12 ns | 12.27 ns |  4.07 |    0.05 |
/// |   BitBuffer_UnAlign_AddInt | 1,945.2 ns | 11.14 ns |  9.30 ns |  3.74 |    0.03 |
/// 
/// 
/// </summary>
public class BitWriter_WriteInt_Benchmark
{
    private const int WriteCount = 255;
    private const int BitsPerWrite = 32;
    
    private BitWriter _bitWriter;
    private BitBuffer _bitBuffer;

    private byte[] _resultArray;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _bitWriter = new BitWriter();
        _bitBuffer = new BitBuffer();

        var arrayLength = (int)Math.Ceiling((float) WriteCount * BitsPerWrite / 8) + 1;
        _resultArray = new byte[arrayLength];
    }

    [Benchmark(Baseline = true)]
    public byte BitWriter_Align_IntWrite()
    {
        _bitWriter.Clear();

        for (int i = 0; i < WriteCount; i++)
        {
            _bitWriter.Write(i);
        }
        
        _bitWriter.Flush();
        
        return _bitWriter.Array[0];
    }
    
    [Benchmark]
    public byte BitWriter_UnAlign_IntWrite()
    {
        _bitWriter.Clear();
        _bitWriter.WriteBits(1, 1);

        for (int i = 0; i < WriteCount; i++)
        {
            _bitWriter.Write(i);
        }
        
        _bitWriter.Flush();
        
        return _bitWriter.Array[0];
    }

    [Benchmark]
    public byte BitBuffer_Align_AddInt()
    {
        _bitBuffer.Clear();

        for (int i = 0; i < WriteCount; i++)
        {
            _bitBuffer.AddInt(i);
        }

        _bitBuffer.ToArray(_resultArray);
        
        return _resultArray[0];
    }
    
    [Benchmark]
    public byte BitBuffer_UnAlign_AddInt()
    {
        _bitBuffer.Clear();
        _bitBuffer.Add(1, 1);
        
        for (int i = 0; i < WriteCount; i++)
        {
            _bitBuffer.AddInt(i);
        }

        _bitBuffer.ToArray(_resultArray);
        
        return _resultArray[0];
    }
}