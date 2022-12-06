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
/// |                      Method |     Mean |   Error |  StdDev | Ratio |
/// |---------------------------- |---------:|--------:|--------:|------:|
/// | NetCode_BitWriter_WriteBits | 572.1 ns | 2.80 ns | 2.48 ns |  1.00 |
/// |      NetStack_BitBuffer_Add | 972.0 ns | 7.91 ns | 7.40 ns |  1.70 |
/// 
/// </summary>
public class BitWriter_WriteBits_Benchmark
{
    private const int WriteCount = 255;
    private const int BitsPerWrite = 7;
    
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
    public byte NetCode_BitWriter_WriteBits()
    {
        _bitWriter.Clear();

        for (byte i = 0; i < WriteCount; i++)
        {
            _bitWriter.WriteBits(BitsPerWrite, i);
        }
        
        _bitWriter.Flush();
        
        return _bitWriter.Array[0];
    }

    [Benchmark]
    public byte NetStack_BitBuffer_Add()
    {
        _bitBuffer.Clear();
    
        for (byte i = 0; i < WriteCount; i++)
        {
            _bitBuffer.Add(BitsPerWrite, i);
        }
    
        _bitBuffer.ToArray(_resultArray);
        
        return _resultArray[0];
    }
}