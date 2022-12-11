namespace NetCode;

public static class ThrowHelper
{
    public static void ThrowIndexOutOfRangeException() => throw new IndexOutOfRangeException();
    
    public static void ThrowArgumentException() => throw new ArgumentException();
    
    public static void ThrowArgumentOutOfRangeException() => throw new ArgumentOutOfRangeException();
}