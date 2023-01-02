namespace NetCode.Limits;

public sealed class UShortLimit
{
    public readonly ushort Min;

    public readonly ushort Max;

    public readonly int BitCount;

    public UShortLimit(ushort min, ushort max)
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