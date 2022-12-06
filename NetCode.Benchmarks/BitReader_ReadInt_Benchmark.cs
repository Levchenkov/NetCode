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
/// |                      Method |       Mean |    Error |   StdDev | Ratio | RatioSD |
/// |---------------------------- |-----------:|---------:|---------:|------:|--------:|
/// |   BitReader_Aligned_ReadInt |   518.2 ns |  2.62 ns |  2.45 ns |  1.00 |    0.00 |
/// | BitReader_UnAligned_ReadInt |   808.9 ns |  2.35 ns |  2.08 ns |  1.56 |    0.01 |
/// |   BitBuffer_Aligned_ReadInt | 2,403.0 ns | 14.81 ns | 13.85 ns |  4.64 |    0.04 |
/// | BitBuffer_UnAligned_ReadInt | 2,899.6 ns | 22.16 ns | 19.64 ns |  5.60 |    0.04 |
/// 
/// </summary>
public class BitReader_ReadInt_Benchmark
{
    private const int ReadCount = 255;
    private const int BitsPerRead = 32;
    
    private BitReader _bitReader;
    private byte[] _array;

    private BitBuffer _bitBuffer;

    [GlobalSetup]
    public void GlobalSetup()
    {
        var arrayLength = (int)Math.Ceiling((float) ReadCount * BitsPerRead / 8) + 1;
        
        _array = new byte[arrayLength]; // 1021
        for (int i = 0; i < _array.Length; i++)
        {
            _array[i] = (byte)i;
        }

        _bitReader = new BitReader();
        _bitBuffer = new BitBuffer();
        
    }

    [Benchmark(Baseline = true)]
    public int BitReader_Aligned_ReadInt()
    {
        var s = 0;
        _bitReader.SetArray(_array);
    
        for (int i = 0; i < ReadCount; i++)
        {
            var value = _bitReader.ReadInt();
            s += value;
        }
        
        return s;
    }
    
    [Benchmark]
    public int BitReader_UnAligned_ReadInt()
    {
        var s = 0;
        _bitReader.SetArray(_array);

        _bitReader.ReadBits(1);
    
        for (int i = 0; i < ReadCount; i++)
        {
            var value = _bitReader.ReadInt();
            s += value;
        }
        
        return s;
    }

    [Benchmark]
    public int BitBuffer_Aligned_ReadInt()
    {
        var s = 0;
        _bitBuffer.FromArray(_array, _array.Length);
    
        for (int i = 0; i < ReadCount; i++)
        {
            var value = _bitBuffer.ReadInt();
            s += value;
        }
        
        return s;
    }
    
    [Benchmark]
    public int BitBuffer_UnAligned_ReadInt()
    {
        var s = 0;
        _bitBuffer.FromArray(_array, _array.Length);

        _bitBuffer.Read(1);
    
        for (int i = 0; i < ReadCount; i++)
        {
            var value = _bitBuffer.ReadInt();
            s += value;
        }
        
        return s;
    }
}