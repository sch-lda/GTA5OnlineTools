namespace GTA5Core.Features;

public static class RAGE
{
    public static uint JOAAT(string __hash)
    {
        var hash = 0u;

        foreach (var c in __hash.ToLower())
        {
            hash += c;
            hash += hash << 10;
            hash ^= hash >> 6;
        }

        hash += hash << 3;
        hash ^= hash >> 11;
        hash += hash << 15;

        return hash;
    }
}
