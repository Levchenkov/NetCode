using BenchmarkDotNet.Attributes;

namespace NetCode.Benchmarks;

/// <summary>
/// BenchmarkDotNet=v0.13.1, OS=macOS Big Sur 11.5.2 (20G95) [Darwin 20.6.0]
/// Intel Core i7-9750H CPU 2.60GHz, 1 CPU, 12 logical and 6 physical cores
/// .NET SDK=6.0.100
/// [Host]     : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT
/// DefaultJob : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT
/// 
/// |           Method |       Mean |   Error |  StdDev | Ratio | RatioSD |
/// |----------------- |-----------:|--------:|--------:|------:|--------:|
/// | BinaryPrimitives |   324.5 ns | 1.26 ns | 1.18 ns |  1.00 |    0.00 |
/// |       ByteReader |   326.4 ns | 1.32 ns | 1.24 ns |  1.01 |    0.01 |
/// |        BitReader |   453.1 ns | 1.35 ns | 1.13 ns |  1.40 |    0.01 |
/// |     BinaryReader | 1,162.4 ns | 5.82 ns | 5.45 ns |  3.58 |    0.02 |
/// 
/// 
/// </summary>
public class ByteReaderBenchmark
{
    private ByteReader _byteReader;
    private BitReader _bitReader;
    private byte[] _array;

    private BinaryReader _binaryReader;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _array = new byte[2000];
        for (int i = 0; i < _array.Length; i++)
        {
            _array[i] = (byte)i;
        }
        
        _byteReader = new ByteReader(_array);
        _bitReader = new BitReader(_array);

        _binaryReader = new BinaryReader(new MemoryStream(_array));
    }
    
    [Benchmark(Baseline = true)]
    public int BinaryPrimitives()
    {
        var s = 0;
        var count = 0;
        Span<byte> span = _array.AsSpan();

        for (int i = 0; i < 255; i++)
        {
            var value = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span);
            s += value;
            
            span = span.Slice(4);
            count += 4;
        }
        
        return s;
    }

    [Benchmark]
    public int ByteReader()
    {
        var s = 0;
        _byteReader.Reset();

        for (int i = 0; i < 255; i++)
        {
            var value = _byteReader.ReadInt();
            s += value;
        }
        
        return s;
    }
    
    [Benchmark]
    public int BitReader()
    {
        var s = 0;
        _bitReader.Reset();

        for (int i = 0; i < 255; i++)
        {
            var value = _bitReader.ReadInt();
            s += value;
        }
        
        return s;
    }

    [Benchmark]
    public int BinaryReader()
    {
        var s = 0;
        _binaryReader.BaseStream.Seek(0, SeekOrigin.Begin);

        for (int i = 0; i < 255; i++)
        {
            var value = _binaryReader.ReadInt32();
            s += value;
        }

        return s;
    }
}