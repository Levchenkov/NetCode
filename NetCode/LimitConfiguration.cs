using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;

namespace NetCode;

public abstract class MinMaxConfiguration<T>
    where T : unmanaged
{
    internal T Min { get; set; }
    
    internal T Max { get; set; }
}
    

public abstract class MinMaxPrecisionConfiguration<T> : MinMaxConfiguration<T>
    where T : unmanaged
{
    internal T Precision { get; set; }
}
    
public sealed class IntFieldConfiguration : MinMaxConfiguration<int>
{
    public void Limit(int min, int max)
    {
        Min = min;
        Max = max;
    }
}
    
public sealed class FloatFieldConfiguration : MinMaxPrecisionConfiguration<float>
{
    public void Limit(float min, float max, float precision)
    {
        Min = min;
        Max = max;
        Precision = precision;
    }
}

public class LimitProfile<T>
{
    public LimitProfile<T> Configure(Expression<Func<T, int>> fieldAccessor)
    {
        return this;
    }
    
    public LimitProfile<T> Configure(Expression<Func<T, int>> fieldAccessor, Action<IntFieldConfiguration> config)
    {
        return this;
    }

    public LimitProfile<T> Configure(Expression<Func<T, float>> fieldAccessor, Action<FloatFieldConfiguration> config)
    {
        return this;
    }
}

public class QuantizationBitReader : BitReader
{
    public QuantizationBitReader()
    {
    }

    public QuantizationBitReader(byte[] data) : base(data)
    {
    }

    public QuantizationBitReader(ByteReader byteReader) : base(byteReader)
    {
    }

    public int ReadInt(int min, int max)
    {
        if (min > max)
        {
            ThrowHelper.ThrowArgumentException();
        }
        
        var range = max - min;
        var bitsRequired = Mathi.BitsRequired((uint)range);
        var value = (int)ReadBits(bitsRequired);
        return value + min;
    }

    public float ReadFloat(float min, float max, float precision)
    {
        if (min > max)
        {
            ThrowHelper.ThrowArgumentException();
        }
        
        var result = ReadFloat(new FloatLimit(min, max, precision));

        return result;
    }

    public float ReadFloat(FloatLimit limit)
    {
        uint integerValue = ReadBits(limit.NumberOfBits);
        float normalizedValue = integerValue / (float)limit.MaxIntegerValue;

        return normalizedValue * limit.Delta + limit.Min;
    }
}


public class QuantizationBitWriter : BitWriter
{
    public QuantizationBitWriter(int capacity = 1500) : base(capacity)
    {
    }

    public QuantizationBitWriter(byte[] data) : base(data)
    {
    }

    public QuantizationBitWriter(ByteWriter byteWriter) : base(byteWriter)
    {
    }

    public void Write(int value, int min, int max)
    {
        if (min > max)
        {
            ThrowHelper.ThrowArgumentException();
        }

        if (value < min || value > max)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException();
        }
        
        var range = max - min;
        var bitsRequired = Mathi.BitsRequired((uint)range);
        WriteBits(bitsRequired, (uint)(value - min));
    }

    public void Write(uint value, uint min, uint max)
    {
        if (min > max)
        {
            ThrowHelper.ThrowArgumentException();
        }

        if (value < min || value > max)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException();
        }
        
        var range = max - min;
        var bitsRequired = Mathi.BitsRequired(range);
        WriteBits(bitsRequired, value - min);
    }

    public void Write(float value, float min, float max, float precision)
    {
        if (min > max)
        {
            ThrowHelper.ThrowArgumentException();
        }

        if (value < min || value > max)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException();
        }
        
        Write(value, new FloatLimit(min, max, precision));
    }
    
    public void Write(float value, FloatLimit limit)
    {
        float normalizedValue = Math.Clamp((value - limit.Min) / limit.Delta, 0, 1);
        uint integerValue = (uint)Math.Floor(normalizedValue * limit.MaxIntegerValue + 0.5f);

        WriteBits(limit.NumberOfBits, integerValue);
    }
}

public readonly struct FloatLimit
{
    public readonly float Min;
    
    public readonly float Delta;

    public readonly uint MaxIntegerValue;

    public readonly int NumberOfBits;
    
    public FloatLimit(float min, float max, float precision)
    {
        Min = min;
        Delta = max - min;
        float values = Delta / precision;
        MaxIntegerValue = (uint)Math.Ceiling(values);
        NumberOfBits = Mathi.BitsRequired(MaxIntegerValue);
    }
} 

public static class C
{
    public static Action<T, TProperty> GetSetter<T, TProperty>(Expression<Func<T, TProperty>> expression)
    {
        var memberExpression = (MemberExpression)expression.Body;
        var property = (PropertyInfo)memberExpression.Member;
        var setMethod = property.GetSetMethod();

        var parameterT = Expression.Parameter(typeof(T), "x");
        var parameterTProperty = Expression.Parameter(typeof(TProperty), "y");

        var newExpression =
            Expression.Lambda<Action<T, TProperty>>(
                Expression.Call(parameterT, setMethod, parameterTProperty),
                parameterT,
                parameterTProperty
            );

        return newExpression.Compile();
    }
    
    public static void M()
    {
        var randomProfile = new LimitProfile<RandomComponent>()
            .Configure(x => x.Seed);
        
        var healthProfile = new LimitProfile<HealthComponent>()
            .Configure(x => x.Health, c => c.Limit(0, 100))
            .Configure(x => x.MaxHealth, c => c.Limit(0, 100));

        var transformProfile = new LimitProfile<TransformComponent>()
            .Configure(x => x.Position.X, c => c.Limit(-100, 100, 0.01f))
            .Configure(x => x.Position.Y, c => c.Limit(-3, 13, 0.01f))
            .Configure(x => x.Position.Z, c => c.Limit(-100, 100, 0.01f))
            .Configure(x => x.Yaw, c => c.Limit(0, 360, 0.1f))
            .Configure(x => x.Pitch, c => c.Limit(0, 360, 0.1f));
    }
}

public struct RandomComponent
{
    public int Seed;
}

public struct HealthComponent
{
    public int Health;

    public int MaxHealth;
}

public struct TransformComponent
{
    public Vector3 Position;

    public float Yaw;

    public float Pitch;
}

