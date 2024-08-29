unsafe public static class Parse
{
    public static sbyte* Arg(string arg)
    {
        return (sbyte*)Marshal.StringToHGlobalAnsi(arg);
    }

    public static string Arg(sbyte* arg)
    {
        return Marshal.PtrToStringAnsi((IntPtr)arg);
    }
}