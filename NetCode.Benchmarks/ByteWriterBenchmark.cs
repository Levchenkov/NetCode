using BenchmarkDotNet.Attributes;

namespace NetCode.Benchmarks;

/// <summary>
/// BenchmarkDotNet=v0.13.1, OS=macOS Big Sur 11.5.2 (20G95) [Darwin 20.6.0]
/// Intel Core i7-9750H CPU 2.60GHz, 1 CPU, 12 logical and 6 physical cores
/// .NET SDK=6.0.100
/// [Host]     : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT
/// DefaultJob : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT
/// 
///  |           Method |       Mean |    Error |   StdDev | Ratio | RatioSD |
/// |----------------- |-----------:|---------:|---------:|------:|--------:|
/// | BinaryPrimitives |   324.3 ns |  1.35 ns |  1.26 ns |  1.00 |    0.00 |
/// |       ByteWriter |   329.9 ns |  1.53 ns |  1.43 ns |  1.02 |    0.01 |
/// |        BitWriter |   337.2 ns |  1.52 ns |  1.43 ns |  1.04 |    0.00 |
/// |     BinaryWriter | 2,336.4 ns | 12.93 ns | 12.09 ns |  7.20 |    0.06 |
/// 
/// 
/// </summary>
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