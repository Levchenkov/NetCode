namespace NetCode.Limits;

public sealed class IntLimit
{
    public readonly int Min;

    public readonly int Max;

    public readonly int BitCount;

    public IntLimit(int min, int max)
    {
        if (min > max)
        {
            ThrowHelper.ThrowArgumentException();
        }
        
        var range = max - min;
        BitCount = Mathi.BitsRequired((uint)range);
        Min = min;
        Max = max;
    }
}