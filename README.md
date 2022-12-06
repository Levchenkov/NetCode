# NetCode

### What is NetCode?

Light and Fast bit and byte serialization for .NET Standard 2.0, .NET Standard 2.1, .NET 5 and .NET 6 (Mono, .NET Core, .NET Framework)

### How do I get started?

##### Write and read bits:

```csharp
var bitWriter = new BitWriter();
        
bitWriter.WriteBits(3, 0b_101);        
bitWriter.Flush();

byte[] data = bitWriter.Array;

var bitReader = new BitReader(data);
uint value = bitReader.ReadBits(3);

Console.WriteLine(value); // output: 5
Console.WriteLine(Convert.ToString(value, 2)); // output: 101
```

##### Use alloc-free binary serialization and deserialization:
```csharp
record Data(byte Byte, short Short, int Int, long Long);

class DataSerializer
{
    private readonly ByteWriter _byteWriter;

    public DataSerializer()
    {
        // Array copying is slow so ByteWriter doesn't use it. It operates fixed size arrays.
        // So be ensure that capacity will be enough otherwise it will be throw exception.
        _byteWriter = new ByteWriter(capacity: 1500); 
    }

    public (byte[] Array, int Length) Serialize(Data data)
    {
        _byteWriter.Clear();
        
        _byteWriter.Write(data.Byte);
        _byteWriter.Write(data.Short);
        _byteWriter.Write(data.Int);
        _byteWriter.Write(data.Long);

        return (_byteWriter.Array, _byteWriter.Count);
    }
}

class DataDeserializer
{
    private readonly ByteReader _byteReader;

    public DataDeserializer()
    {
        _byteReader = new ByteReader();
    }

    public Data Deserialize(byte[] array)
    {
        _byteReader.SetArray(array);

        var data = new Data(
            _byteReader.ReadByte(),
            _byteReader.ReadShort(),
            _byteReader.ReadInt(),
            _byteReader.ReadLong());

        return data;
    }
}

```

### Where can I get it?

```
PM> Install-Package NetCode
```

or

```
dotnet add package NetCode
```

### Features

- Fast
  - Performance focused
    - Uses [high-perf](https://docs.microsoft.com/en-us/dotnet/api/system.buffers.binary.binaryprimitives) memory accessors
    - No array copying
  - Zero memory allocations
    - No array creation
    - Reusable class instances
  - Has a lot of [benchmarks](https://github.com/Levchenkov/NetCode/tree/main/NetCode.Benchmarks)
- Light
  - 4 classes each about 150 code lines 
- Safe to use
  - Covered by [unit](https://github.com/Levchenkov/NetCode/tree/main/NetCode.UnitTests) tests
- Supports a lot of frameworks:
  - .NET Standard 2.0
  - .NET Standard 2.1
  - .NET 5
  - .NET 6

### Why NetCode?

##### 1. High Performance and Alloc-free for read and write operations:

Read benchmarks you can find [here](https://github.com/Levchenkov/NetCode/blob/main/NetCode.Benchmarks/ByteReaderBenchmark.cs).

```
BenchmarkDotNet=v0.13.1, OS=macOS Big Sur 11.5.2 (20G95) [Darwin 20.6.0]
Intel Core i7-9750H CPU 2.60GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.100
  [Host]     : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT
  DefaultJob : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT

|           Method |       Mean |   Error |  StdDev | Ratio | RatioSD | Allocated |
|----------------- |-----------:|--------:|--------:|------:|--------:|----------:|
| BinaryPrimitives |   328.4 ns | 1.24 ns | 1.10 ns |  1.00 |    0.00 |         - |
|       ByteReader |   329.4 ns | 1.71 ns | 1.60 ns |  1.00 |    0.01 |         - |
|        BitReader |   457.8 ns | 1.09 ns | 0.91 ns |  1.39 |    0.01 |         - |
|     BinaryReader | 1,205.8 ns | 6.75 ns | 6.31 ns |  3.67 |    0.02 |         - |
```

Write benchmarks you can find [here](https://github.com/Levchenkov/NetCode/blob/main/NetCode.Benchmarks/ByteWriterBenchmark.cs).

```
BenchmarkDotNet=v0.13.1, OS=macOS Big Sur 11.5.2 (20G95) [Darwin 20.6.0]
Intel Core i7-9750H CPU 2.60GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.100
  [Host]     : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT
  DefaultJob : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT


|           Method |       Mean |    Error |   StdDev | Ratio | RatioSD | Allocated |
|----------------- |-----------:|---------:|---------:|------:|--------:|----------:|
| BinaryPrimitives |   325.7 ns |  1.39 ns |  1.30 ns |  1.00 |    0.00 |         - |
|       ByteWriter |   330.3 ns |  1.67 ns |  1.48 ns |  1.01 |    0.01 |         - |
|        BitWriter |   338.0 ns |  1.69 ns |  1.58 ns |  1.04 |    0.00 |         - |
|     BinaryWriter | 2,344.7 ns | 12.08 ns | 10.71 ns |  7.20 |    0.03 |         - |
```

##### 2. Supports aligned data [writing](https://github.com/Levchenkov/NetCode/blob/main/NetCode.Benchmarks/BitWriter_WriteByte_Benchmark.cs) and [reading](https://github.com/Levchenkov/NetCode/blob/main/NetCode.Benchmarks/BitReader_ReadByte_Benchmark.cs).

##### 3. Faster than other bit serialization libraries (see [this](https://github.com/Levchenkov/NetCode/blob/main/NetCode.Benchmarks/BitReader_ReadBits_Benchmark.cs) and [this](https://github.com/Levchenkov/NetCode/blob/main/NetCode.Benchmarks/BitWriter_WriteBits_Benchmark.cs)).