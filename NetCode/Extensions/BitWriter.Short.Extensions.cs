namespace NetCode;

public static class BitWriterShortExtensions
{
    public static void WriteValueIfChanged(this BitWriter writer, short baseline, short updated)
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
    
    public static void WriteValueIfChanged(this BitWriter writer, ushort baseline, ushort updated)
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
}