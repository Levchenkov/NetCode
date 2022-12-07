namespace NetCode.Limits;

public sealed class UIntLimit
{
    public uint Min { get; }
    
    public uint Max { get; }
    
    public int BitCount { get; }

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