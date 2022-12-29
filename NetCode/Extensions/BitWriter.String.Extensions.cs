namespace NetCode;

public static class BitWriterStringExtensions
{
    public static void Write(this BitWriter writer, string value) => writer.WriteUtf8String(value);

    public static void WriteValueIfChanged(this BitWriter writer, string baseline, string updated)
    {
        if (baseline == updated)
        {
            writer.Write(false);
        }
        else
        {
            writer.Write(true);
            writer.Write(updated);
        }
    }
    
    public static void WriteUtf8String(this BitWriter writer, string value)
    {
        writer.WriteCompressed(value.Length);

        for (var i = 0; i < value.Length; i++)
        {
            var byteValue = Convert.ToByte(value[i]);
            writer.Write(byteValue); 
        }
    }
    
    public static void WriteUnicodeString(this BitWriter writer, string value)
    {
        writer.WriteCompressed(value.Length);

        for (var i = 0; i < value.Length; i++)
        {
            writer.Write(value[i]);
        }
    }
}