namespace NetCode.Limits;

public sealed class Vector3Limit
{
    public readonly float Precision;

    public readonly float PrecisionSquare;

    public readonly FloatLimit X;

    public readonly FloatLimit Y;

    public readonly FloatLimit Z;

    public Vector3Limit(FloatLimit x, FloatLimit y, FloatLimit z)
    {
        X = x;
        Y = y;
        Z = z;

        Precision = Math.Max(Math.Max(X.Precision, Y.Precision), Z.Precision);
        PrecisionSquare = Precision * Precision;
    }
}