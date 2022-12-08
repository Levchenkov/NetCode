namespace NetCode.Limits;

public sealed class Vector3Limit
{
    public float Precision { get; }
    
    public float PrecisionSquare { get; }

    public FloatLimit X { get; }
    
    public FloatLimit Y { get; }
    
    public FloatLimit Z { get; }

    public Vector3Limit(FloatLimit x, FloatLimit y, FloatLimit z)
    {
        X = x;
        Y = y;
        Z = z;

        Precision = Math.Max(Math.Max(X.Precision, Y.Precision), Z.Precision);
        PrecisionSquare = Precision * Precision;
    }
}