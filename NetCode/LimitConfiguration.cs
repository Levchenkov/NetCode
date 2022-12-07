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

