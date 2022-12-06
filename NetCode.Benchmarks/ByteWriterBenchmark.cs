using BenchmarkDotNet.Attributes;

namespace NetCode.Benchmarks;

/// <summary>
/// BenchmarkDotNet=v0.13.1, OS=macOS Big Sur 11.5.2 (20G95) [Darwin 20.6.0]
/// Intel Core i7-9750H CPU 2.60GHz, 1 CPU, 12 logical and 6 physical cores
/// .NET SDK=6.0.100
/// [Host]     : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT
/// DefaultJob : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT
/// 
/// |           Method |       Mean |    Error |   StdDev | Ratio | RatioSD | Allocated |
/// |----------------- |-----------:|---------:|---------:|------:|--------:|----------:|
/// | BinaryPrimitives |   325.7 ns |  1.39 ns |  1.30 ns |  1.00 |    0.00 |         - |
/// |       ByteWriter |   330.3 ns |  1.67 ns |  1.48 ns |  1.01 |    0.01 |         - |
/// |        BitWriter |   338.0 ns |  1.69 ns |  1.58 ns |  1.04 |    0.00 |         - |
/// |     BinaryWriter | 2,344.7 ns | 12.08 ns | 10.71 ns |  7.20 |    0.03 |         - |
/// 
/// </summary>
[MemoryDiagnoser]
public class ByteWriterBenchmark
{
    private ByteWriter _byteWriter;
    private BitWriter _bitWriter;
    private byte[] _array;

    private BinaryWriter _binaryWriter;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _array = new byte[2000];
        _byteWriter = new ByteWriter(_array);
        _bitWriter = new BitWriter(_array);

        _binaryWriter = new BinaryWriter(new MemoryStream(_array));
    }
    
    [Benchmark(Baseline = true)]
    public int BinaryPrimitives()
    {
        var count = 0;
        Span<byte> span = _array.AsSpan();

        for (int i = 0; i < 255; i++)
        {
            System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(span, i);
            span = span.Slice(4);
            count += 4;
        }
        
        return count;
    }

    [Benchmark]
    public int ByteWriter()
    {
        _byteWriter.Clear();

        for (int i = 0; i < 255; i++)
        {
            _byteWriter.Write(i);
        }
        
        return _byteWriter.Count;
    }
    
    [Benchmark]
    public int BitWriter()
    {
        _bitWriter.Clear();

        for (int i = 0; i < 255; i++)
        {
            _bitWriter.Write(i);
        }
        
        return _bitWriter.BytesCount;
    }

    [Benchmark]
    public long BinaryWriter()
    {
        _binaryWriter.Seek(0, SeekOrigin.Begin);

        for (int i = 0; i < 255; i++)
        {
            _binaryWriter.Write(i);
        }
        
        _binaryWriter.Flush();
        
        return _binaryWriter.BaseStream.Position;
    }
}