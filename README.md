# NetCode

### What is NetCode?

Light and Fast bit and byte serialization for .NET Standard 2.0, .NET Standard 2.1, .NET 6 and .NET 8 (Mono, .NET Core, .NET Framework, Unity)

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

##### Quantization Example:

```csharp
var bitWriter = new BitWriter();
bitWriter.Write(value: 1f, min: 0f, max: 10f, precision: 0.1f);
Console.WriteLine(bitWriter.BitsCount); // 7
        
bitWriter.Flush();
Console.WriteLine(bitWriter.BitsCount); // 8

var data = bitWriter.Array;
var bitReader = new BitReader(data);
var value = bitReader.ReadFloat(min: 0f, max: 10f, precision: 0.1f);

Console.WriteLine(Math.Abs(value - 1f) < 0.1f); // True
Console.WriteLine(value); // 1
```

##### Use alloc-free binary serialization and deserialization:
```csharp
var serializer = new TransformComponentSerializer();
var deserializer = new TransformComponentDeserializer();

var before = new TransformComponent { Position = new Vector3(10f, 5f, 10f), Pitch = 30f, Yaw = 60f };
var after = new TransformComponent { Position = new Vector3(10.5f, 5.5f, 10.5f), Pitch = 30f, Yaw = 60f };

var serializedComponent = serializer.Serialize(before, after);
Console.WriteLine(serializedComponent.Length); // 3

var updated = deserializer.Deserialize(before, serializedComponent.Array);

serializedComponent.Dispose();

Console.WriteLine(updated); // Position: <10.5, 5.5, 10.5>, Yaw: 60, Pitch: 30

public record struct TransformComponent (Vector3 Position, float Yaw, float Pitch );

public struct SerializedComponent
{
    private readonly ArrayPool<byte> _arrayPool;
    
    public byte[] Array { get; }
    
    public int Length { get; }

    public SerializedComponent(ArrayPool<byte> arrayPool, byte[] array, int length)
    {
        _arrayPool = arrayPool;
        Array = array;
        Length = length;
    }

    public void Dispose()
    {
        _arrayPool.Return(Array);
    }
}

public static class Limits
{
    public static readonly FloatLimit Rotation = new FloatLimit(0, 360, 0.1f);
        
    public static readonly Vector3Limit AbsolutePosition = new Vector3Limit(new FloatLimit(-100f, 100f, 0.1f), new FloatLimit(-10f, 10f, 0.1f), new FloatLimit(-100f, 100f, 0.1f));
        
    public static readonly Vector3Limit DiffPosition = new Vector3Limit(new FloatLimit(-1f, 1f, 0.1f), new FloatLimit(-1f, 1f, 0.1f), new FloatLimit(-1f, 1f, 0.1f));
}

public class TransformComponentSerializer
{
    private const int MTU = 1500;
    
    private readonly BitWriter _bitWriter = new BitWriter();
    private readonly ArrayPool<byte> _arrayPool = ArrayPool<byte>.Shared;

    public SerializedComponent Serialize(TransformComponent baseline, TransformComponent updated)
    {
        var array = _arrayPool.Rent(MTU);
        _bitWriter.SetArray(array);
        
        _bitWriter.WriteDiffIfChanged(baseline.Position.X, updated.Position.X, Limits.AbsolutePosition.X, Limits.DiffPosition.X);
        _bitWriter.WriteDiffIfChanged(baseline.Position.Y, updated.Position.Y, Limits.AbsolutePosition.Y, Limits.DiffPosition.Y);
        _bitWriter.WriteDiffIfChanged(baseline.Position.Z, updated.Position.Z, Limits.AbsolutePosition.Z, Limits.DiffPosition.Z);
        
        _bitWriter.WriteValueIfChanged(baseline.Yaw, updated.Yaw, Limits.Rotation);
        _bitWriter.WriteValueIfChanged(baseline.Pitch, updated.Pitch, Limits.Rotation);
        
        _bitWriter.Flush();

        return new SerializedComponent(_arrayPool, _bitWriter.Array, _bitWriter.BytesCount);
    }
}

public class TransformComponentDeserializer
{
    private readonly BitReader _bitReader = new BitReader();

    public TransformComponent Deserialize(TransformComponent before, byte[] array)
    {
        _bitReader.SetArray(array);

        TransformComponent result = default;

        result.Position = new Vector3(
            _bitReader.ReadFloat(before.Position.X, Limits.AbsolutePosition.X, Limits.DiffPosition.X),
            _bitReader.ReadFloat(before.Position.Y, Limits.AbsolutePosition.Y, Limits.DiffPosition.Y),
            _bitReader.ReadFloat(before.Position.Z, Limits.AbsolutePosition.Z, Limits.DiffPosition.Z));
        
        result.Yaw = _bitReader.ReadFloat(before.Yaw, Limits.Rotation);
        result.Pitch = _bitReader.ReadFloat(before.Pitch, Limits.Rotation);

        return result;
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
- Read \ Write bit values
- Read \ Write with delta compression
- Read \ Write quantized values
- Safe to use
  - Covered by [unit](https://github.com/Levchenkov/NetCode/tree/main/NetCode.UnitTests) tests
- Supports a lot of frameworks:
  - .NET Standard 2.0
  - .NET Standard 2.1
  - .NET 6
  - .NET 8

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