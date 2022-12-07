namespace NetCode.Limits;

public sealed class Vector3Limit
{
    public float Precision => Math.Max(Math.Max(X.Precision, Y.Precision), Z.Precision);
    
    public FloatLimit X { get; }
    public FloatLimit Y { get; }
    public FloatLimit Z { get; }

    public Vector3Limit(FloatLimit x, FloatLimit y, FloatLimit z)
    {
        X = x;
        Y = y;
        Z = z;
    }
}