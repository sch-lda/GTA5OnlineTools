using GTA5Core.Features;

namespace GTA5MenuExtra.Views.HeistsEditor;

/// <summary>
/// AdvanceView.xaml 的交互逻辑
/// </summary>
public partial class AdvanceView : UserControl
{
    public AdvanceView()
    {
        InitializeComponent();
    }

    /// <summary>
    /// 即时完成 fm_mission_controller
    /// 公寓抢劫 | 末日豪劫 | 名钻赌场豪劫
    /// </summary>
    public static void InstantFmMissionController()
    {
        if (Locals.LocalAddress("fm_mission_controller") == 0)
            return;

        if (Locals.ReadLocalAddress<int>("fm_mission_controller", 3236) == 0)
            return;

        for (int i = 0; i <= 3; i++)
        {
            Locals.WriteLocalAddress("fm_mission_controller", 19746 + 1232 + 1 + i, 264666);
        }
        Locals.WriteLocalAddress("fm_mission_controller", 19746, 12);
    }

    /// <summary>
    /// 即时完成 fm_mission_controller_2020
    /// 改装铺合约 | 佩里科岛 | 德瑞
    /// </summary>
    public static void InstantFmMissionController2020()
    {
        if (Locals.LocalAddress("fm_mission_controller_2020") == 0)
            return;

        if (Locals.ReadLocalAddress<int>("fm_mission_controller_2020", 19376) == 0)
            return;

        for (int i = 0; i <= 3; i++)
        {
            Locals.WriteLocalAddress("fm_mission_controller_2020", 50150 + 1770 + 1 + i, 264666);
        }
        Locals.WriteLocalAddress("fm_mission_controller_2020", 50150, 9);
    }

    /// <summary>
    /// 单人启动任务（这应该允许你能完整的玩末日将至）
    /// https://www.unknowncheats.me/forum/4007046-post4761.html
    /// </summary>
    public static void AloneLaunchHeist()
    {
        if (Locals.LocalAddress("fmmc_launcher") == 0)
            return;

        if (Locals.ReadLocalAddress<int>("fmmc_launcher", 19709 + 34) == 0)
            return;

        if (Locals.ReadLocalAddress<int>("fmmc_launcher", 19709 + 15) > 1)
        {
            Locals.WriteLocalAddress("fmmc_launcher", 19709 + 15, 1);
            Globals.Set_Global_Value(794744 + 4 + 1 + (Locals.ReadLocalAddress<int>("fmmc_launcher", 19709 + 34) * 89) + 69, 1);
        }

        Globals.Set_Global_Value(4718592 + 3523 + 1, 1);
        Globals.Set_Global_Value(4718592 + 3529 + 1, 1);
        Globals.Set_Global_Value(4718592 + 178821 + 1, 0);
        Globals.Set_Global_Value(4718592 + 3526, 1);
        Globals.Set_Global_Value(4718592 + 3526 + 1, 1);
    }

    public static void BeginTransactionCasinoHeist()
    {
        // 交易到银行
        Online2.Begin_transaction(RAGE.JOAAT("CATEGORY_SERVICE_WITH_THRESHOLD"), RAGE.JOAAT("NET_SHOP_ACTION_EARN"), 4, new uint[,]{
            { RAGE.JOAAT("SERVICE_EARN_CASINO_HEIST_FINALE"), 3619000 }, // 名钻赌场豪劫
        }); // 弗利萨同款马桶刷钱方法
    }

    public static void BeginTransactionCayoHeist()
    {
        // 交易到银行
        Online2.Begin_transaction(RAGE.JOAAT("CATEGORY_SERVICE_WITH_THRESHOLD"), RAGE.JOAAT("NET_SHOP_ACTION_EARN"), 4, new uint[,]{
            { RAGE.JOAAT("SERVICE_EARN_ISLAND_HEIST_FINALE"), 2550000 }, // 佩里科岛
        }); // 弗利萨同款马桶刷钱方法
    }

    public static void BeginTransactionDoomsdayHeist()
    {
        // 交易到银行
        Online2.Begin_transaction(RAGE.JOAAT("CATEGORY_SERVICE_WITH_THRESHOLD"), RAGE.JOAAT("NET_SHOP_ACTION_EARN"), 4, new uint[,]{
            { RAGE.JOAAT("SERVICE_EARN_GANGOPS_FINALE"), 2550000 }, // 末日豪劫
        }); // 弗利萨同款马桶刷钱方法
    }
    //////////////////////////////////////////////////////

    private void Button_InstantFmMissionController_Click(object sender, RoutedEventArgs e)
    {
        InstantFmMissionController();
    }

    private void Button_InstantFmMissionController2020_Click(object sender, RoutedEventArgs e)
    {
        InstantFmMissionController2020();
    }

    private void Button_AloneLaunchHeist_Click(object sender, RoutedEventArgs e)
    {
        AloneLaunchHeist();
    }

    private void Button_BeginTransactionCasinoHeist_Click(object sender, RoutedEventArgs e)
    {
        BeginTransactionCasinoHeist();
    }
    private void Button_BeginTransactionCayoHeist_Click(object sender, RoutedEventArgs e)
    {
        BeginTransactionCayoHeist();
    }
    private void Button_BeginTransactionDoomsdayHeist_Click(object sender, RoutedEventArgs e)
    {
        BeginTransactionDoomsdayHeist();
    }
}
