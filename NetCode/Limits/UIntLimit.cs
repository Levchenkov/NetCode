namespace NetCode.Limits;

public sealed class UIntLimit
{
    public readonly uint Min;

    public readonly uint Max;

    public readonly int BitCount;

    public UIntLimit(uint min, uint max)
    {
        if (min > max)
        {
            ThrowHelper.ThrowArgumentException();
        }
        
        var range = max - min;
        BitCount = Mathi.BitsRequired(range);
        Min = min;
        Max = max;
    }
}