namespace NetCode.Limits;

public struct ShortLimit
{
    public readonly short Min;

    public readonly short Max;

    public readonly int BitCount;

    public ShortLimit(short min, short max)
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