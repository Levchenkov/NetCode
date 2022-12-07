namespace NetCode.Limits;

public sealed class IntLimit
{
    public int Min { get; }
    
    public int Max { get; }
    
    public int BitCount { get; }

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