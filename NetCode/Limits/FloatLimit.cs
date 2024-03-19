namespace NetCode.Limits;

public struct FloatLimit
{
    public readonly float Min;

    public readonly float Max;

    public readonly float Precision;

    public readonly float Delta;

    public readonly uint MaxIntegerValue;

    public readonly int BitCount;

    public FloatLimit(float min, float max, float precision)
    {
        if (min >= max)
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