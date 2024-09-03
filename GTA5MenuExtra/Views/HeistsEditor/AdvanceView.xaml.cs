using GTA5Core.Features;

namespace GTA5MenuExtra.Views.HeistsEditor;

/// <summary>
/// AdvanceView.xaml 的交互逻辑
/// </summary>
public partial class AdvanceView : UserControl
{
    public static bool aloneMission = false;
    public AdvanceView()
    {
        InitializeComponent();
    }

    /// <summary>
    /// 即时完成 fm_mission_controller
    /// 公寓抢劫 | 末日豪劫 | 名钻赌场豪劫 | 改装铺合约 | 佩里科岛 | 德瑞 | anyMission
    /// </summary>
    public static void InstantFmMissionControllerANY()
    {
        if (Locals.LocalAddress("fm_mission_controller") != 0)
        {
            if (Locals.ReadLocalAddress<int>("fm_mission_controller", 3236) == 0)
                return;

            for (int i = 0; i <= 3; i++)
                Locals.WriteLocalAddress("fm_mission_controller", 19746 + 1232 + 1 + i, 264666);

            Locals.WriteLocalAddress("fm_mission_controller", 19746, 12);
            return;
        }

        if (Locals.LocalAddress("fm_mission_controller_2020") != 0)
        {
            if (Locals.ReadLocalAddress<int>("fm_mission_controller_2020", 19376) == 0)
                return;

            for (int i = 0; i <= 3; i++)
                Locals.WriteLocalAddress("fm_mission_controller_2020", 50150 + 1770 + 1 + i, 264666);

            Locals.WriteLocalAddress("fm_mission_controller_2020", 50150, 9);
            return;
        }
    }

    /// <summary>
    /// 单人启动任务（这应该允许你能完整的玩末日将至）
    /// https://www.unknowncheats.me/forum/4007046-post4761.html
    /// </summary>
    public static async Task AloneLaunchHeist()
    {
        await Task.Run(() =>
        {
            if (!aloneMission)
                aloneMission = true;
            else
                aloneMission = false;

            while (aloneMission)
            {
                if (Locals.LocalAddress("fmmc_launcher") != 0)
                {
                    if (Locals.ReadLocalAddress<int>("fmmc_launcher", 19709 + 34) != 9999)
                    {
                        if (Locals.ReadLocalAddress<int>("fmmc_launcher", 19709 + 15) > 1)
                        {
                            Locals.WriteLocalAddress("fmmc_launcher", 19709 + 15, 1);
                            Globals.Set_Global_Value(794744 + 4 + 1 + (Locals.ReadLocalAddress<int>("fmmc_launcher", 19709 + 34) * 89) + 69, 1);
                        }
                    }
                }

                Globals.Set_Global_Value(4718592 + 3523 + 1, 1);
                Globals.Set_Global_Value(4718592 + 3529 + 1, 1);
                Globals.Set_Global_Value(4718592 + 178821 + 1, 0);
                Globals.Set_Global_Value(4718592 + 3526, 1);
                Globals.Set_Global_Value(4718592 + 3526 + 1, 1);
            }
        });
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
    public static void GreenPlayersAllCrashGame()
    {
        var pedHandle = __PLAYER.PLAYER_PED_ID();
        Vector3 vec3 = Parse.Arg(__ENTITY.GET_ENTITY_COORDS(pedHandle, 0));
        vec3.Z = 500f;
        var vehicleHandle = vehicle.create_vehicle("ruiner2", vec3, __ENTITY.GET_ENTITY_HEADING(pedHandle));

        if (vehicleHandle != 0)
        {
            __PED.SET_PED_INTO_VEHICLE(pedHandle, vehicleHandle, -1);
            __VEHICLE.VEHICLE_SET_PARACHUTE_MODEL_OVERRIDE(vehicleHandle, RAGE.JOAAT("sum_prop_dufocore_01a"));
            __VEHICLE.VEHICLE_START_PARACHUTING(vehicleHandle, 1);
            // entity.delete_entity(vehicle);
        }
    }
    public static void GreenPlayersAllTse968269233()
    {
        for (var i = 0; i <= 24; i++)
        {
            script.trigger_script_event(1, 0x7fffffff, new int[] { 968269233, 0xff, 1, 4, i, 1, 1, 1 });
            script.trigger_script_event(1, 0x7fffffff, new int[] { 968269233, 0xff, 1, 8, -5, 1, 1, 1 });
        }
    }

    //////////////////////////////////////////////////////

    private void Button_InstantFmMissionControllerANY_Click(object sender, RoutedEventArgs e)
    {
        InstantFmMissionControllerANY();
    }

    private async void Button_AloneLaunchHeist_Click(object sender, RoutedEventArgs e)
    {
        await AloneLaunchHeist();
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

    private void Button_GreenPlayersAllCrashGame_Click(object sender, RoutedEventArgs e)
    {
        GreenPlayersAllCrashGame();
    }

    private void Button_GreenPlayersAllTse968269233_Click(object sender, RoutedEventArgs e)
    {
        GreenPlayersAllTse968269233();
    }
}
