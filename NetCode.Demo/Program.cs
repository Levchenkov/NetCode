using System.Buffers;
using System.Numerics;
using NetCode;
using NetCode.Limits;

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