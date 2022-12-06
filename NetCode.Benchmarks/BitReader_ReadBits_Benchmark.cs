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
/// |                     Method |     Mean |   Error |  StdDev | Ratio |
/// |--------------------------- |---------:|--------:|--------:|------:|
/// | NetCode_BitReader_ReadBits | 560.8 ns | 3.34 ns | 3.12 ns |  1.00 |
/// |    NetStack_BitBuffer_Read | 939.4 ns | 1.76 ns | 1.47 ns |  1.68 |
/// 
/// </summary>
public class BitReader_ReadBits_Benchmark
{
    private const int ReadCount = 255;
    private const int BitsPerRead = 11;
    
    private BitReader _bitReader;
    private byte[] _array;

    private BitBuffer _bitBuffer;

    [GlobalSetup]
    public void GlobalSetup()
    {
        var arrayLength = (int)Math.Ceiling((float) ReadCount * BitsPerRead / 8);
        
        _array = new byte[arrayLength]; // 351
        for (int i = 0; i < _array.Length; i++)
        {
            _array[i] = (byte)i;
        }

        _bitReader = new BitReader();
        _bitBuffer = new BitBuffer();
    }

    [Benchmark(Baseline = true)]
    public uint NetCode_BitReader_ReadBits()
    {
        uint s = 0;
        
        _bitReader.SetArray(_array);

        for (int i = 0; i < ReadCount; i++)
        {
            var value = _bitReader.ReadBits(BitsPerRead);
            s += value;
        }

        return s;
    }

    [Benchmark]
    public uint NetStack_BitBuffer_Read()
    {
        uint s = 0;
        
        _bitBuffer.FromArray(_array, _array.Length);
    
        for (int i = 0; i < ReadCount; i++)
        {
            var value = _bitBuffer.Read(BitsPerRead);
            s += value;
        }
        
        return s;
    }
}