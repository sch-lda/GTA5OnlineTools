using GTA5Core.Native;
using GTA5Core.Offsets;

namespace GTA5Core.Features;

public static class STATS
{
    private const int character = 1574926;

    private const int stat_structure = 2749550 + 305;
    private const int stat_set_int_case = 1668667 + 1138;

    private const int stat_set_int_hash = 1680145 + 1 + 3;
    private const int stat_set_int_value = 982384 + 5587;

    private static uint GET_STAT_HASH(string statName)
    {
        if (statName.StartsWith("MPx_"))
        {
            var index = Globals.GetPlayerIndex();
            statName = statName.Replace("MPx_", $"MP{index}_");
        }

        return JOATT(statName);
    }

    /// <summary>
    /// 字符串转HASH值
    /// </summary>
    /// <param name="statName"></param>
    /// <returns></returns>
    public static uint JOATT(string statName)
    {
        var hash = 0u;

        foreach (var c in statName.ToLower())
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

    /// <summary>
    /// 设置INT类型STAT值
    /// </summary>
    /// <param name="statName"></param>
    /// <returns></returns>
    public static async Task STAT_SET_INT(string statName, int value)
    {
        await Task.Run(() => { STAT_SET_INT1(statName, value); });
    }

    /// <summary>
    /// 设置INT类型STAT值
    /// </summary>
    /// <param name="statName"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    private static void STAT_SET_INT1(string statName, int value)
    {
        var hash = GET_STAT_HASH(statName);
        __STATS.STAT_SET_INT(hash, value, 1);
    }
    /// <summary>
    /// 设置INT类型STAT值（旧方法）
    /// </summary>
    /// <param name="statName"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    private static void STAT_SET_INT2(string statName, int value)
    {
        var hash = GET_STAT_HASH(statName);

        var oldSetIntHash = Globals.Get_Global_Value<uint>(stat_set_int_hash);
        var oldSetIntValue = Globals.Get_Global_Value<int>(stat_set_int_value);

        Globals.Set_Global_Value(stat_set_int_hash, hash);
        Globals.Set_Global_Value(stat_set_int_value, value);

        Globals.Set_Global_Value(stat_structure, 3);
        Globals.Set_Global_Value(stat_set_int_case, 3);

        do
        {
            // 应该主动调用速度会快很多...
        } while ((Globals.Get_Global_Value<int>(stat_set_int_case) == 3) & (Globals.Get_Global_Value<int>(stat_structure) == 3));

        Globals.Set_Global_Value(stat_set_int_hash, oldSetIntHash);
        Globals.Set_Global_Value(stat_set_int_value, oldSetIntValue);
    }

    /// <summary>
    /// 读取INT类型STAT值
    /// </summary>
    /// <param name="statName"></param>
    /// <returns></returns>
    unsafe private static int STAT_GET_INT(string statName)
    {
        var hash = GET_STAT_HASH(statName);

        int value = 0;
        if (!stats.get_int(hash, &value))
            return 0;

        return value;
    }

    /// <summary>
    /// 设置FLOAT类型STAT值
    /// </summary>
    /// <param name="statName"></param>
    /// <param name="value"></param>
    public static void STAT_SET_FLOAT(string statName, float value)
    {
        var hash = GET_STAT_HASH(statName);
        stats.set_float(hash, value);
    }

    /// <summary>
    /// 设置BOOL类型STAT值
    /// </summary>
    /// <param name="statName"></param>
    /// <param name="value"></param>
    public static void STAT_SET_BOOL(string statName, bool value)
    {
        var hash = GET_STAT_HASH(statName);
        stats.set_bool(hash, value);
    }
}
