namespace NetCode.Limits;

public sealed class FloatLimit
{
    public float Min { get; }
    
    public float Max { get; }
    
    public float Precision { get; }
    
    public float Delta { get; }

    public uint MaxIntegerValue { get; }

    public int BitCount { get; }
    
    public FloatLimit(float min, float max, float precision)
    {
        if (min > max)
        {
            ThrowHelper.ThrowArgumentException();
        }
        
        Min = min;
        Max = max;
        Precision = precision;
        Delta = max - min;
        float values = Delta / precision;
        MaxIntegerValue = (uint)Math.Ceiling(values);
        BitCount = Mathi.BitsRequired(MaxIntegerValue);
    }
}