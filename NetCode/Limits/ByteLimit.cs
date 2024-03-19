namespace NetCode.Limits;

public struct ByteLimit
{
    public readonly byte Min;

    public readonly byte Max;

    public readonly int BitCount;

    public ByteLimit(byte min, byte max)
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