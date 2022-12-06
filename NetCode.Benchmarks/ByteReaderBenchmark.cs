using BenchmarkDotNet.Attributes;

namespace NetCode.Benchmarks;

/// <summary>
/// BenchmarkDotNet=v0.13.1, OS=macOS Big Sur 11.5.2 (20G95) [Darwin 20.6.0]
/// Intel Core i7-9750H CPU 2.60GHz, 1 CPU, 12 logical and 6 physical cores
/// .NET SDK=6.0.100
/// [Host]     : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT
/// DefaultJob : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT
/// 
/// |           Method |       Mean |   Error |  StdDev | Ratio | RatioSD | Allocated |
/// |----------------- |-----------:|--------:|--------:|------:|--------:|----------:|
/// | BinaryPrimitives |   328.4 ns | 1.24 ns | 1.10 ns |  1.00 |    0.00 |         - |
/// |       ByteReader |   329.4 ns | 1.71 ns | 1.60 ns |  1.00 |    0.01 |         - |
/// |        BitReader |   457.8 ns | 1.09 ns | 0.91 ns |  1.39 |    0.01 |         - |
/// |     BinaryReader | 1,205.8 ns | 6.75 ns | 6.31 ns |  3.67 |    0.02 |         - |
/// 
/// </summary>
[MemoryDiagnoser]
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
    public int Shift()
    {
        var s = 0;
        int index = 0;

        for (int i = 0; i < 255; i++)
        {
            int value = _array[index++];
            value |= _array[index++] << 8;
            value |= _array[index++] << 16;
            value |= _array[index++] << 24;
            
            s += value;
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