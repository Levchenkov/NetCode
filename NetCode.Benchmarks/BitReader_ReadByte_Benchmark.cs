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
/// |                       Method |     Mean |    Error |   StdDev | Ratio | RatioSD |
/// |----------------------------- |---------:|---------:|---------:|------:|--------:|
/// |   BitReader_Aligned_ReadByte | 455.3 ns |  1.14 ns |  1.01 ns |  1.00 |    0.00 |
/// | BitReader_UnAligned_ReadByte | 518.5 ns |  3.13 ns |  2.61 ns |  1.14 |    0.01 |
/// |   BitBuffer_Aligned_ReadByte | 867.8 ns |  1.55 ns |  1.45 ns |  1.91 |    0.01 |
/// | BitBuffer_UnAligned_ReadByte | 965.5 ns | 12.69 ns | 11.87 ns |  2.12 |    0.03 |
/// 
/// </summary>
public class BitReader_ReadByte_Benchmark
{
    private const int ReadCount = 255;
    private const int BitsPerRead = 8;
    
    private BitReader _bitReader;
    private byte[] _array;

    private BitBuffer _bitBuffer;

    [GlobalSetup]
    public void GlobalSetup()
    {
        var arrayLength = (int)Math.Ceiling((float) ReadCount * BitsPerRead / 8) + 1;
        
        _array = new byte[arrayLength]; // 256
        for (int i = 0; i < _array.Length; i++)
        {
            _array[i] = (byte)i;
        }

        _bitReader = new BitReader();
        _bitBuffer = new BitBuffer();
    }

    [Benchmark(Baseline = true)]
    public int BitReader_Aligned_ReadByte()
    {
        var s = 0;
        _bitReader.SetArray(_array);
    
        for (int i = 0; i < ReadCount; i++)
        {
            var value = _bitReader.ReadByte();
            s += value;
        }
        
        return s;
    }
    
    [Benchmark]
    public int BitReader_UnAligned_ReadByte()
    {
        var s = 0;
        _bitReader.SetArray(_array);
        
        _bitReader.ReadBits(1);

        for (int i = 0; i < ReadCount; i++)
        {
            var value = _bitReader.ReadByte();
            s += value;
        }
        
        return s;
    }

    [Benchmark]
    public int BitBuffer_Aligned_ReadByte()
    {
        var s = 0;
        _bitBuffer.FromArray(_array, _array.Length);
    
        for (int i = 0; i < ReadCount; i++)
        {
            var value = _bitBuffer.ReadByte();
            s += value;
        }
        
        return s;
    }
    
    [Benchmark]
    public int BitBuffer_UnAligned_ReadByte()
    {
        var s = 0;
        _bitBuffer.FromArray(_array, _array.Length);
        
        _bitBuffer.Read(1);

        for (int i = 0; i < ReadCount; i++)
        {
            var value = _bitBuffer.ReadByte();
            s += value;
        }
        
        return s;
    }
}