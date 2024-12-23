using GTA5Core.Native;
using GTA5Core.Offsets;

namespace GTA5Core.Features;

public static class Online
{
    /// <summary>
    /// 线上战局切换
    /// -1, 离开线上模式
    ///  0, 公共战局
    ///  1, 创建公共战局
    ///  2, 私人帮会战局
    ///  3, 帮会战局
    ///  6, 私人好友战局
    ///  9, 加入好友
    /// 10, 单人战局
    /// 11, 仅限邀请战局
    /// 12, 加入帮会伙伴
    /// </summary>
    /// <param name="sessionID">战局ID</param>
    public static void LoadSession(int sessionID)
    {
        Task.Run(async () =>
        {
            Memory.SetForegroundWindow();

            if (sessionID == -1)
            {
                // 离开线上模式需要特殊处理
                Globals.Set_Global_Value(Base.SessionSwitchCache, -1);
            }
            else
            {
                // 正常切换战局，修改战局类型
                Globals.Set_Global_Value(Base.SessionSwitchType, sessionID);
            }

            // 切换战局状态
            Globals.Set_Global_Value(Base.SessionSwitchState, 1);
            await Task.Delay(200);
            Globals.Set_Global_Value(Base.SessionSwitchState, 0);
        });
    }

    /// <summary>
    /// 空战局
    /// </summary>
    public static void EmptySession()
    {
        Task.Run(async () =>
        {
            ProcessMgr.SuspendProcess();
            await Task.Delay(10000);
            ProcessMgr.ResumeProcess();
        });
    }

    /// <summary>
    /// 挂机防踢
    /// </summary>
    /// <param name="isEnable"></param>
    public static void AntiAFK(bool isEnable)
    {
        // STATS::PLAYSTATS_IDLE_KICK
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("IDLEKICK_WARNING1")), isEnable ? 99999999 : 120000);        // 120000     joaat("IDLEKICK_WARNING1") 
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("IDLEKICK_WARNING2")), isEnable ? 99999999 : 300000);        // 300000     joaat("IDLEKICK_WARNING2")
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("IDLEKICK_WARNING3")), isEnable ? 99999999 : 600000);        // 600000     joaat("IDLEKICK_WARNING3")
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("IDLEKICK_KICK")), isEnable ? 99999999 : 900000);        // 900000     joaat("IDLEKICK_KICK")

        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("ConstrainedKick_Warning1")), isEnable ? 2000000000 : 30000);     // 30000      joaat("ConstrainedKick_Warning1")
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("ConstrainedKick_Warning2")), isEnable ? 2000000000 : 60000);     // 60000      joaat("ConstrainedKick_Warning2")
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("ConstrainedKick_Warning3")), isEnable ? 2000000000 : 90000);     // 90000      joaat("ConstrainedKick_Warning3")
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("ConstrainedKick_Kick")), isEnable ? 2000000000 : 120000);    // 120000     joaat("ConstrainedKick_Kick")
    }

    /// <summary>
    /// 免费更改角色外观
    /// </summary>
    /// <param name="isEnable"></param>
    public static void FreeChangeAppearance(bool isEnable)
    {
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("CHARACTER_APPEARANCE_CHARGE")), isEnable ? 0 : 100000);         // joaat("CHARACTER_APPEARANCE_CHARGE")
    }

    /// <summary>
    /// 移除更改角色外观冷却
    /// </summary>
    /// <param name="isEnable"></param>
    public static void ChangeAppearanceCooldown(bool isEnable)
    {
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("CHARACTER_APPEARANCE_COOLDOWN")), isEnable ? 0 : 2880000);         // joaat("CHARACTER_APPEARANCE_COOLDOWN")
    }

    /// <summary>
    /// 模型变更
    /// </summary>
    /// <param name="hash"></param>
    public static async void ModelChange(long hash)
    {
        // else if (!PED::HAS_PED_HEAD_BLEND_FINISHED(PLAYER::PLAYER_PED_ID())
        Globals.Set_Global_Value(Base.oVGETIn + 63, 1);                 // triggerModelChange
        Globals.Set_Global_Value(Base.oVGETIn + 50, hash);              // modelChangeHash
        await Task.Delay(10);
        Globals.Set_Global_Value(Base.oVGETIn + 63, 0);
    }

    /// <summary>
    /// 允许非公共战局运货
    /// </summary>
    /// <param name="isEnable"></param>
    public static void SellOnNonPublic(bool isEnable)
    {
        Globals.Set_Global_Value(2684504 + 746, isEnable ? 0 : 1);         // NETWORK::NETWORK_SESSION_GET_PRIVATE_SLOTS()
    }

    /// <summary>
    /// 移除被动模式CD
    /// </summary>
    /// <param name="isEnable"></param>
    public static void PassiveModeCooldown(bool isEnable)
    {
        Globals.Set_Global_Value(2738934 + 4512, isEnable ? 0 : 1);        // AUDIO::REQUEST_SCRIPT_AUDIO_BANK("DLC_HEI4/DLC_HEI4_Submarine" // _STOPWATCH_RESET(&(Global_2794162.f_4497), false, false);
        Globals.Set_Global_Value(1963748, isEnable ? 0 : 1);               // joaat("VEHICLE_WEAPON_SUB_MISSILE_HOMING")
    }

    /// <summary>
    /// 移除自杀CD
    /// </summary>
    /// <param name="isEnable"></param>
    public static void SuicideCooldown(bool isEnable)
    {
        if (isEnable)
            Globals.Set_Global_Value(Base.oVMYCar + 6975, 0);      // joaat("XPCATEGORY_ACTION_KILLS")

        Globals.Set_Global_Value(Tunables.Index(247954694), isEnable ? 1 : 300000);         // 247954694
        Globals.Set_Global_Value(Tunables.Index(-1771488297), isEnable ? 1 : 60000);          // -1771488297
    }

    /// <summary>
    /// 移除轨道炮CD
    /// </summary>
    /// <param name="isEnable"></param>
    public static void OrbitalCooldown(bool isEnable)
    {
        Globals.Set_Global_Value(Tunables.Index(-1707434973), isEnable ? 0 : 2880000);         // -1707434973
    }

    /// <summary>
    /// 进入线上个人载具
    /// </summary>
    public static void GetInOnlinePV()
    {
        Globals.Set_Global_Value(Base.oVGETIn + 8, 1);     // (PLAYER::PLAYER_ID()), 0f, 0f, 0f, Global_
    }

    /// <summary>
    /// 战局雪天
    /// </summary>
    /// <param name="isEnable"></param>
    public static void SessionSnow(bool isEnable)
    {
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("turn_snow_on_off")), isEnable ? 1 : 0);            // joaat("turn_snow_on_off")
    }

    /// <summary>
    /// 雷达影踪/人间蒸发
    /// </summary>
    /// <param name="isEnable"></param>
    public static void OffRadar(bool isEnable)
    {
        // Global_2657971[PLAYER::PLAYER_ID() /*465*/].f_211 = 0;
        // timeDifference = NETWORK::GET_TIME_DIFFERENCE(NETWORK::GET_NETWORK_TIME(), Global_2672524.f_57);
        Globals.Set_Global_Value(Base.oPlayerIDHelp + 1 + Globals.GetPlayerID() * 465 + 211, isEnable ? 1 : 0);
        if (isEnable)
            Globals.Set_Global_Value(Base.oNETTimeHelp + 58, Globals.GetNetworkTime() + 3600000);
        Globals.Set_Global_Value(Base.oVMYCar + 4682, isEnable ? 3 : 0);
    }

    /// <summary>
    /// 幽灵组织
    /// </summary>
    /// <param name="isEnable"></param>
    public static void GhostOrganization(bool isEnable)
    {
        Globals.Set_Global_Value(Base.oPlayerIDHelp + 1 + Globals.GetPlayerID() * 465 + 211, isEnable ? 1 : 0);
        if (isEnable)
            Globals.Set_Global_Value(Base.oNETTimeHelp + 58, Globals.GetNetworkTime() + 3600000);        // iVar0 = NETWORK::GET_TIME_DIFFERENCE(NETWORK::GET_NETWORK_TIME()
        Globals.Set_Global_Value(Base.oVMYCar + 4682, isEnable ? 4 : 0);
    }

    /// <summary>
    /// 警察无视犯罪
    /// </summary>
    /// <param name="isEnable"></param>
    public static void BribeOrBlindCops(bool isEnable)
    {
        Globals.Set_Global_Value(Base.oVMYCar + 4676 + 1, isEnable ? 1 : 0);           // _DISPLAY_HELP_TEXT("FM_LEST_NCPW"
        Globals.Set_Global_Value(Base.oVMYCar + 4676 + 3, isEnable ? Globals.GetNetworkTime() + 3600000 : 0);
        Globals.Set_Global_Value(Base.oVMYCar + 4676, isEnable ? 5 : 0);
    }

    /// <summary>
    /// 贿赂当局
    /// </summary>
    /// <param name="isEnable"></param>
    public static void BribeAuthorities(bool isEnable)
    {
        Globals.Set_Global_Value(Base.oVMYCar + 4676 + 1, isEnable ? 1 : 0);
        Globals.Set_Global_Value(Base.oVMYCar + 4676 + 3, isEnable ? Globals.GetNetworkTime() + 3600000 : 0);
        Globals.Set_Global_Value(Base.oVMYCar + 4676, isEnable ? 21 : 0);
    }

    /// <summary>
    /// 显示玩家
    /// </summary>
    /// <param name="isEnable"></param>
    public static void RevealPlayers(bool isEnable)
    {
        Globals.Set_Global_Value(Base.oPlayerIDHelp + 1 + Globals.GetPlayerID() * 465 + 214, isEnable ? 1 : 0);
        Globals.Set_Global_Value(Base.oNETTimeHelp + 59, isEnable ? Globals.GetNetworkTime() + 3600000 : 0);
    }

    /// <summary>
    /// 设置角色等级经验倍数
    /// </summary>
    /// <param name="multiplier"></param>
    public static void RPMultiplier(float multiplier)
    {
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("XP_MULTIPLIER")), multiplier);           // joaat("XP_MULTIPLIER")
    }

    /// <summary>
    /// 设置角色AP经验倍数
    /// </summary>
    /// <param name="multiplier"></param>
    public static void APMultiplier(float multiplier)
    {
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("AP_MULTIPLIER")), multiplier);      // joaat("AP_MULTIPLIER")
    }

    /// <summary>
    /// 设置车友会等级经验倍数
    /// </summary>
    /// <param name="multiplier"></param>
    public static void REPMultiplier(float multiplier)
    {
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("TUNER_STREET_RACE_PLACE_XP_MULTIPLIER")), multiplier);        // Street Race         街头比赛        -147149995  joaat("TUNER_STREET_RACE_PLACE_XP_MULTIPLIER")
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("TUNER_PURSUIT_RACE_PLACE_XP_MULTIPLIER")), multiplier);        // Pursuit Race        追逐赛
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("TUNER_CHECKPOINT_RACE_PLACE_XP_MULTIPLIER")), multiplier);        // Scramble            攀登
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("TUNER_HEADTOHEAD_RACE_PLACE_XP_MULTIPLIER")), multiplier);        // Head 2 Head         头对头          1434998920

        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("TUNER_CARCLUB_TIME_XP_MULTIPLIER")), multiplier);        // LS Car Meet         汽车见面会       1819417801  joaat("TUNER_CARCLUB_TIME_XP_MULTIPLIER")
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("TUNER_SANDBOX_TIME_XP_MULTIPLIER")), multiplier);        // LS Car Meet Track
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("TUNER_MERCH_PURCHASE_XP_MULTIPLIER")), multiplier);        // LS Car Meet Cloth Shop
    }

    /// <summary>
    /// 使用牛鲨睾酮
    /// </summary>
    /// <param name="isEnable"></param>
    public static void InstantBullShark(bool isEnable)
    {
        if (isEnable)
            Globals.Set_Global_Value(Base.oNETTimeHelp + 3733, 1);
        else
        {
            var temp = Globals.Get_Global_Value<int>(Base.oNETTimeHelp + 3733);
            if (temp != 0)
                Globals.Set_Global_Value(Base.oNETTimeHelp + 3733, 5);
        }
    }

    /// <summary>
    /// 呼叫支援直升机
    /// </summary>
    /// <param name="isEnable"></param>
    public static void CallBackupHeli(bool isEnable)
    {
        Globals.Set_Global_Value(Base.oVMYCar + 4506, isEnable ? 1 : 0);   // SCRIPT::GET_NUMBER_OF_THREADS_RUNNING_THE_SCRIPT_WITH_THIS_HASH(joaat("am_backup_heli")
    }

    /// <summary>
    /// 呼叫空袭
    /// </summary>
    /// <param name="isEnable"></param>
    public static void CallAirstrike(bool isEnable)
    {
        Globals.Set_Global_Value(Base.oVMYCar + 4507, isEnable ? 1 : 0);   // SCRIPT::GET_NUMBER_OF_THREADS_RUNNING_THE_SCRIPT_WITH_THIS_HASH(joaat("am_airstrike")
    }

    /// <summary>
    /// 启用CEO特殊货物
    /// </summary>
    /// <param name="isEnable"></param>
    public static void CEOSpecialCargo(bool isEnable)
    {
        Globals.Set_Global_Value(1943379, isEnable ? 1 : 0);           // MISC::GET_RANDOM_INT_IN_RANGE(1, 101);   // Global_1950703 = 1;
    }

    /// <summary>
    /// 设置CEO特殊货物类型
    /// </summary>
    /// <param name="cargoID"></param>
    public static void CEOCargoType(int cargoID)
    {
        Globals.Set_Global_Value(1943225, cargoID);                    // MISC::GET_RANDOM_INT_IN_RANGE(1, 101);   // Global_1950549 = num;
    }

    /// <summary>
    /// 移除购买CEO板条箱冷却
    /// </summary>
    /// <param name="isEnable"></param>
    public static void CEOBuyingCratesCooldown(bool isEnable)
    {
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("EXEC_BUY_COOLDOWN")), isEnable ? 0 : 300000);          // 153204142 joaat("EXEC_BUY_COOLDOWN")
    }

    /// <summary>
    /// 移除出售CEO板条箱冷却
    /// </summary>
    /// <param name="isEnable"></param>
    public static void CEOSellingCratesCooldown(bool isEnable)
    {
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("EXEC_SELL_COOLDOWN")), isEnable ? 0 : 1800000);         // 1291620941 joaat("EXEC_SELL_COOLDOWN")
    }

    /// <summary>
    /// 移除地堡进货延迟
    /// </summary>
    /// <param name="isEnable"></param>
    public static void BunkerSupplyDelay(bool isEnable)
    {
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("GR_PURCHASE_SUPPLIES_DELAY")), isEnable ? 0 : 600);            // -2094564985 joaat("GR_PURCHASE_SUPPLIES_DELAY")
    }

    /// <summary>
    /// 解锁地堡所有研究 (临时)
    /// </summary>
    /// <param name="isEnable"></param>
    public static void UnlockBunkerResearch(bool isEnable)
    {
        //TODO: 原方法早在1.61或更早版本就已经失效
    }

    /// <summary>
    /// 移除摩托帮进货延迟
    /// </summary>
    /// <param name="isEnable"></param>
    public static void MCSupplyDelay(bool isEnable)
    {
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("WEBSITE_PEGASSI_FAGGIO_SPORT")), isEnable ? 0 : 600);          // 728170457  joaat("WEBSITE_PEGASSI_FAGGIO_SPORT")
    }

    /// <summary>
    /// 设置梅利威瑟服务类型
    /// </summary>
    /// <param name="serverId"></param>
    public static void MerryWeatherServices(int serverId)
    {
        Globals.Set_Global_Value(Base.oVMYCar + serverId, 1);
    }

    /// <summary>
    /// 移除进出口大亨出货CD
    /// </summary>
    /// <param name="isEnable"></param>
    public static void ExportVehicleDelay(bool isEnable)
    {
        Globals.Set_Global_Value(Tunables.Index(1001423248), isEnable ? 0 : 1200000);        // 1001423248  tuneables_processing.c
        Globals.Set_Global_Value(Tunables.Index(240134765), isEnable ? 0 : 1680000);        // 240134765
        Globals.Set_Global_Value(Tunables.Index(1915379148), isEnable ? 0 : 2340000);        // 1915379148
        Globals.Set_Global_Value(Tunables.Index(-824005590), isEnable ? 0 : 2880000);        // -824005590
    }

    /// <summary>
    /// 断开战局连接
    /// </summary>
    public static async void Disconnect()
    {
        Globals.Set_Global_Value(33226, 1);              // NETWORK::NETWORK_BAIL(1, 0, 0)
        await Task.Delay(200);
        Globals.Set_Global_Value(33226, 0);              // return NETWORK::NETWORK_CAN_ACCESS_MULTIPLAYER
    }

    /// <summary>
    /// 结束过场动画
    /// </summary>
    public static async void StopCutscene()
    {
        Globals.Set_Global_Value(2710447 + 3, 1);        // CUTSCENE::REQUEST_CUTSCENE("HS4_SCP_KNCK", 8);
        Globals.Set_Global_Value(1575083, 1);            // NETWORK::NETWORK_TRANSITION_ADD_STAGE(hashKey, 1, num, etsParam0, 0);
        await Task.Delay(1000);
        Globals.Set_Global_Value(2710447 + 3, 0);
        Globals.Set_Global_Value(1575083, 0);
    }

    /// <summary>
    /// 移除机库进货CD
    /// </summary>
    /// <param name="isEnable"></param>
    public static void SmugglerRunInDelay(bool isEnable)
    {
        Globals.Set_Global_Value(Tunables.Index(1278611667), isEnable ? 0 : 120000);         // 1278611667  tuneables_processing.c
        Globals.Set_Global_Value(Tunables.Index(-1424847540), isEnable ? 0 : 180000);         // -1424847540
        Globals.Set_Global_Value(Tunables.Index(-1817541754), isEnable ? 0 : 240000);         // -1817541754
        Globals.Set_Global_Value(Tunables.Index(1722502526), isEnable ? 0 : 60000);          // 1722502526
    }

    /// <summary>
    /// 移除机库出货CD
    /// </summary>
    /// <param name="isEnable"></param>
    public static void SmugglerRunOutDelay(bool isEnable)
    {
        Globals.Set_Global_Value(Tunables.Index(-1525481945), isEnable ? 0 : 180000);         // -1525481945  tuneables_processing.c
    }

    /// <summary>
    /// 移除夜总会出货CD
    /// </summary>
    /// <param name="isEnable"></param>
    public static void NightclubOutDelay(bool isEnable)
    {
        Globals.Set_Global_Value(Tunables.Index(1763921019), isEnable ? 0 : 300000);         // 1763921019  tuneables_processing.c
        Globals.Set_Global_Value(Tunables.Index(-1004589438), isEnable ? 0 : 300000);         // -1004589438  Global_262145.f_24671 = 300000;
        Globals.Set_Global_Value(Tunables.Index(464940095), isEnable ? 0 : 300000);         // 464940095
    }

    /// <summary>
    /// 移除CEO工作冷却
    /// </summary>
    /// <param name="isEnable"></param>
    public static void CEOWorkCooldown(bool isEnable)
    {
        // CEO工作
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("GB_COOLDOWN_UNTIL_NEXT_BOSS_WORK")), isEnable ? 0 : 300000);       // -1404265088 joaat("GB_COOLDOWN_UNTIL_NEXT_BOSS_WORK")
        // 观光客
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("GB_SIGHTSEER_COOLDOWN")), isEnable ? 0 : 600000);       // -1911318106 joaat("GB_SIGHTSEER_COOLDOWN")
    }

    /// <summary>
    /// 移除恐霸客户差事冷却
    /// </summary>
    /// <param name="isEnable"></param>
    public static void ClientJobCooldown(bool isEnable)
    {
        Globals.Set_Global_Value(Tunables.Index(-926426916), isEnable ? 0 : 300000);       // Between Jobs           -926426916
        Globals.Set_Global_Value(Tunables.Index(1733390598), isEnable ? 0 : 1800000);      // Robbery in Progress    1733390598
        Globals.Set_Global_Value(Tunables.Index(724724668), isEnable ? 0 : 1800000);      // Data Sweep             724724668
        Globals.Set_Global_Value(Tunables.Index(846317886), isEnable ? 0 : 1800000);      // Targeted Data          846317886
        Globals.Set_Global_Value(Tunables.Index(443623246), isEnable ? 0 : 1800000);      // Diamond Shopping       443623246
    }

    /// <summary>
    /// 移除事务所安保合约冷却
    /// </summary>
    /// <param name="isEnable"></param>
    public static void SecurityHitCooldown(bool isEnable)
    {
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("FIXER_SECURITY_CONTRACT_COOLDOWN_TIME")), isEnable ? 0 : 300000);           // -1462622971 joaat("FIXER_SECURITY_CONTRACT_COOLDOWN_TIME")
    }

    /// <summary>
    /// 移除公共电话任务合约冷却
    /// </summary>
    /// <param name="isEnable"></param>
    public static void PayphoneHitCooldown(bool isEnable)
    {
        Globals.Set_Global_Value(Tunables.Index(1872071131), isEnable ? 0 : 1200000);          // 1872071131
    }

    /// <summary>
    /// 进入RC匪徒
    /// </summary>
    /// <param name="isEnable"></param>
    public static void TriggerRCBandito(bool isEnable)
    {
        Globals.Set_Global_Value(Base.oVMYCar + 6956, isEnable ? 1 : 0);                 // if (PED::IS_PED_IN_ANY_VEHICLE(PLAYER::PLAYER_PED_ID(), true) || PED::IS_PED_GETTING_INTO_A_VEHICLE(PLAYER::PLAYER_PED_ID()))
    }

    /// <summary>
    /// 进入迷你坦克
    /// </summary>
    /// <param name="isEnable"></param>
    public static void TriggerMiniTank(bool isEnable)
    {
        Globals.Set_Global_Value(Base.oVMYCar + 6957, isEnable ? 1 : 0);
    }

    /// <summary>
    /// 请求虎鲸
    /// </summary>
    /// <param name="isEnable"></param>
    public static async void RequestKosatka()
    {
        Globals.Set_Global_Value(Base.oNETTimeHelp + 63 + 10, 17);
        await Task.Delay(100);
        Globals.Set_Global_Value(Base.oVMYCar + 975, 1);
    }

    /// <summary>
    /// 虎鲸导弹冷却
    /// </summary>
    /// <param name="isEnable"></param>
    public static void KosatkaMissleCooldown(bool isEnable)
    {
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("IH_SUBMARINE_MISSILES_COOLDOWN")), isEnable ? 0 : 60000);           // joaat("IH_SUBMARINE_MISSILES_COOLDOWN")
    }
}
