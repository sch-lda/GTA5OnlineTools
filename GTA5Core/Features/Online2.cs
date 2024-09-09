using GTA5Core.Offsets;
using System.Windows.Markup;
using System;
using GTA5Core.Native;

namespace GTA5Core.Features;

/// <summary>
/// 2023/06/17
/// 这里的功能可能会有风险， 暂时遗弃
/// </summary>
public static class Online2
{
    /// <summary>
    /// 设置CEO板条箱每箱出售单价为2W
    /// </summary>
    /// <param name="isEnable"></param>
    public static void CEOPricePerCrateAtCrates(bool isEnable)
    {
        // -1445480509 joaat("EXEC_CONTRABAND_SALE_VALUE_THRESHOLD1")
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("EXEC_CONTRABAND_SALE_VALUE_THRESHOLD1")), isEnable ? 20000 : 10000);            // 1
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("EXEC_CONTRABAND_SALE_VALUE_THRESHOLD2")), isEnable ? 20000 : 11000);        // 2
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("EXEC_CONTRABAND_SALE_VALUE_THRESHOLD3")), isEnable ? 20000 : 12000);        // 3
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("EXEC_CONTRABAND_SALE_VALUE_THRESHOLD4")), isEnable ? 20000 : 13000);        // 4-5
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("EXEC_CONTRABAND_SALE_VALUE_THRESHOLD5")), isEnable ? 20000 : 13500);        // 6-7
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("EXEC_CONTRABAND_SALE_VALUE_THRESHOLD6")), isEnable ? 20000 : 14000);        // 8-9
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("EXEC_CONTRABAND_SALE_VALUE_THRESHOLD7")), isEnable ? 20000 : 14500);        // 10-14
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("EXEC_CONTRABAND_SALE_VALUE_THRESHOLD8")), isEnable ? 20000 : 15000);        // 15-19
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("EXEC_CONTRABAND_SALE_VALUE_THRESHOLD9")), isEnable ? 20000 : 15500);        // 20-24
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("EXEC_CONTRABAND_SALE_VALUE_THRESHOLD10")), isEnable ? 20000 : 16000);        // 25-29
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("EXEC_CONTRABAND_SALE_VALUE_THRESHOLD11")), isEnable ? 20000 : 16500);       // 30-34
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("EXEC_CONTRABAND_SALE_VALUE_THRESHOLD12")), isEnable ? 20000 : 17000);       // 35-39
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("EXEC_CONTRABAND_SALE_VALUE_THRESHOLD13")), isEnable ? 20000 : 17500);       // 40-44
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("EXEC_CONTRABAND_SALE_VALUE_THRESHOLD14")), isEnable ? 20000 : 17750);       // 45-49
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("EXEC_CONTRABAND_SALE_VALUE_THRESHOLD15")), isEnable ? 20000 : 18000);       // 50-59
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("EXEC_CONTRABAND_SALE_VALUE_THRESHOLD16")), isEnable ? 20000 : 18250);       // 60-69
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("EXEC_CONTRABAND_SALE_VALUE_THRESHOLD17")), isEnable ? 20000 : 18500);       // 70-79
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("EXEC_CONTRABAND_SALE_VALUE_THRESHOLD18")), isEnable ? 20000 : 18750);       // 80-89
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("EXEC_CONTRABAND_SALE_VALUE_THRESHOLD19")), isEnable ? 20000 : 19000);       // 90-990
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("EXEC_CONTRABAND_SALE_VALUE_THRESHOLD20")), isEnable ? 20000 : 19500);       // 100-11
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("EXEC_CONTRABAND_SALE_VALUE_THRESHOLD21")), isEnable ? 20000 : 20000);       // 111
    }


    /// <summary>
    /// 设置地堡生产和研究时间为指定时间，单位秒
    /// </summary>
    /// <param name="isEnable"></param>
    /// <param name="produce_time"></param>
    public static void SetBunkerProduceResearchTime(bool isEnable, int produce_time = 1)
    {
        // Base Time to Produce                                                         // tuneables_processing.c
        Globals.Set_Global_Value(Tunables.Index(215868155), isEnable ? produce_time : 600000);        // Product                  215868155 
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("GR_RESEARCH_PRODUCTION_TIME")), isEnable ? produce_time : 300000);        // Research                 -676414773 joaat("GR_RESEARCH_PRODUCTION_TIME")

        // Time to Produce Reductions
        Globals.Set_Global_Value(Tunables.Index(631477612), isEnable ? produce_time : 90000);         // Production Equipment     631477612
        Globals.Set_Global_Value(Tunables.Index(818645907), isEnable ? produce_time : 90000);         // Production Staff         818645907
        Globals.Set_Global_Value(Tunables.Index(-1148432846), isEnable ? produce_time : 45000);         // Research Equipment       -1148432846
        Globals.Set_Global_Value(Tunables.Index(510883248), isEnable ? produce_time : 45000);         // Research Staff           510883248
    }

    /// <summary>
    /// 设置地堡进货单价为200元
    /// </summary>
    /// <param name="isEnable"></param>
    public static void SetBunkerResupplyCosts(bool isEnable)
    {
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("GR_PURCHASE_SUPPLIES_COST_PER_SEGMENT")), isEnable ? 200 : 15000);          // 2024/07/01 : update by Alice2333.
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("GR_PURCHASE_SUPPLIES_COST_PER_SEGMENT_BASE")), isEnable ? 200 : 15000);
    }

    /// <summary>
    /// 设置地堡远近出货倍数
    /// </summary>
    /// <param name="isEnable"></param>
    public static void SetBunkerSaleMultipliers(bool isEnable)
    {
        // Sale Multipliers                                             // tuneables_processing.c
        Globals.Set_Global_Value(Tunables.Index(1865029244), isEnable ? 2.0f : 1.0f);          // Near         1865029244
        Globals.Set_Global_Value(Tunables.Index(1021567941), isEnable ? 3.0f : 1.5f);          // Far          1021567941
    }

    /// <summary>
    /// 设置摩托帮远近出货倍数
    /// </summary>
    /// <param name="isEnable"></param>
    public static void SetMCSaleMultipliers(bool isEnable)
    {
        // Sale Multipliers                                             // tuneables_processing.c
        Globals.Set_Global_Value(Tunables.Index(-823848572), isEnable ? 2.0f : 1.0f);          // Near         -823848572
        Globals.Set_Global_Value(Tunables.Index(1763638426), isEnable ? 3.0f : 1.5f);          // Far          1763638426
    }

    /// <summary>
    /// 设置地堡原材料消耗量
    /// </summary>
    /// <param name="isEnable"></param>
    public static void SetBunkerSuppliesPerUnitProduced(bool isEnable)
    {
        // Supplies Per Unit Produced                                   // tuneables_processing.c
        Globals.Set_Global_Value(Tunables.Index(-1652502760), isEnable ? 1 : 10);               // Product Base              -1652502760
        Globals.Set_Global_Value(Tunables.Index(1647327744), isEnable ? 1 : 5);                // Product Upgraded          1647327744
        Globals.Set_Global_Value(Tunables.Index(1485279815), isEnable ? 1 : 2);                // Research Base             1485279815
        Globals.Set_Global_Value(Tunables.Index(2041812011), isEnable ? 1 : 1);                // Research Upgraded         2041812011
    }

    /// <summary>
    /// 设置摩托帮原材料消耗量
    /// </summary>
    /// <param name="isEnable"></param>
    public static void SetMCSuppliesPerUnitProduced(bool isEnable)
    {
        // Supplies Per Unit Produced                                   // tuneables_processing.c
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("BIKER_FAKEIDS_MATERIAL_PRODUCT_COST")), isEnable ? 1 : 4);                // Documents Base            -1839004359
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("BIKER_COUNTERCASH_MATERIAL_PRODUCT_COST")), isEnable ? 1 : 10);               // Cash Base
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("BIKER_CRACK_MATERIAL_PRODUCT_COST")), isEnable ? 1 : 50);               // Cocaine Base
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("BIKER_METH_MATERIAL_PRODUCT_COST")), isEnable ? 1 : 24);               // Meth Base
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("BIKER_WEED_MATERIAL_PRODUCT_COST")), isEnable ? 1 : 4);                // Weed Base
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("BIKER_FAKEIDS_MATERIAL_PRODUCT_COST_UPGRADE_REDUCTION")), isEnable ? 1 : 2);                // Documents Upgraded
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("BIKER_COUNTERCASH_MATERIAL_PRODUCT_COST_UPGRADE_REDUCTION")), isEnable ? 1 : 5);                // Cash Upgraded
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("BIKER_CRACK_MATERIAL_PRODUCT_COST_UPGRADE_REDUCTION")), isEnable ? 1 : 25);               // Cocaine Upgraded
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("BIKER_METH_MATERIAL_PRODUCT_COST_UPGRADE_REDUCTION")), isEnable ? 1 : 12);               // Meth Upgraded
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("BIKER_WEED_MATERIAL_PRODUCT_COST_UPGRADE_REDUCTION")), isEnable ? 1 : 2);                // Weed Upgraded
    }

    /// <summary>
    /// 设置夜总会生产时间为指定时间，单位秒
    /// </summary>
    /// <param name="isEnable"></param>
    /// <param name="produce_time"></param>
    public static void SetNightclubProduceTime(bool isEnable, int produce_time)
    {
        // Time to Produce                                                      // tuneables_processing.c
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("BB_BUSINESS_DEFAULT_ACCRUE_TIME_WEAPONS")), isEnable ? produce_time : 4800000);       // Sporting Goods               -147565853
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("BB_BUSINESS_DEFAULT_ACCRUE_TIME_COKE")), isEnable ? produce_time : 14400000);      // South American Imports
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("BB_BUSINESS_DEFAULT_ACCRUE_TIME_METH")), isEnable ? produce_time : 7200000);       // Pharmaceutical Research
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("BB_BUSINESS_DEFAULT_ACCRUE_TIME_WEED")), isEnable ? produce_time : 2400000);       // Organic Produce
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("BB_BUSINESS_DEFAULT_ACCRUE_TIME_FORGED_DOCUMENTS")), isEnable ? produce_time : 1800000);       // Printing and Copying
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("BB_BUSINESS_DEFAULT_ACCRUE_TIME_COUNTERFEIT_CASH")), isEnable ? produce_time : 3600000);       // Cash Creation
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("BB_BUSINESS_DEFAULT_ACCRUE_TIME_CARGO")), isEnable ? produce_time : 8400000);       // Cargo and Shipments          1607981264
    }

    /// <summary>
    /// 设置摩托帮生产时间为指定时间，单位秒
    /// </summary>
    /// <param name="isEnable"></param>
    /// <param name="produce_time"></param>
    public static void SetMCProduceTime(bool isEnable, int produce_time)
    {
        // Base Time to Produce                                                 // tuneables_processing.c
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("BIKER_WEED_PRODUCTION_TIME")), isEnable ? produce_time : 360000);        // Weed                     -635596193
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("BIKER_METH_PRODUCTION_TIME")), isEnable ? produce_time : 1800000);       // Meth
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("BIKER_CRACK_PRODUCTION_TIME")), isEnable ? produce_time : 3000000);       // Cocaine
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("BIKER_FAKEIDS_PRODUCTION_TIME")), isEnable ? produce_time : 300000);        // Documents
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("BIKER_COUNTERCASH_PRODUCTION_TIME")), isEnable ? produce_time : 720000);        // Cash                     1310272402

        // Time to Produce Reductions
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("BIKER_FAKEIDS_UPGRADE_EQUIPMENT_REDUCTION_TIME")), isEnable ? 1 : 60000);                    // Documents Equipment      1672482518
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("BIKER_COUNTERCASH_UPGRADE_EQUIPMENT_REDUCTION_TIME")), isEnable ? 1 : 120000);                   // Cash Equipment
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("BIKER_CRACK_UPGRADE_EQUIPMENT_REDUCTION_TIME")), isEnable ? 1 : 600000);                   // Cocaine Equipment
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("BIKER_METH_UPGRADE_EQUIPMENT_REDUCTION_TIME")), isEnable ? 1 : 360000);                   // Meth Equipment
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("BIKER_WEED_UPGRADE_EQUIPMENT_REDUCTION_TIME")), isEnable ? 1 : 60000);                    // Weed Equipment
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("BIKER_FAKEIDS_UPGRADE_STAFF_REDUCTION_TIME")), isEnable ? 1 : 60000);                    // Documents Staff
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("BIKER_COUNTERCASH_UPGRADE_STAFF_REDUCTION_TIME")), isEnable ? 1 : 120000);                   // Cash Staff
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("BIKER_CRACK_UPGRADE_STAFF_REDUCTION_TIME")), isEnable ? 1 : 600000);                   // Cocaine Staff
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("BIKER_METH_UPGRADE_STAFF_REDUCTION_TIME")), isEnable ? 1 : 360000);                   // Meth Staff
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("BIKER_WEED_UPGRADE_STAFF_REDUCTION_TIME")), isEnable ? 1 : 60000);                    // Weed Staff               1575359233
    }

    /// <summary>
    /// 设置摩托帮进货单价为200元
    /// </summary>
    /// <param name="isEnable"></param>
    public static void SetMCResupplyCosts(bool isEnable)
    {
        Globals.Set_Global_Value(Tunables.Index(RAGE.JOAAT("BIKER_PURCHASE_SUPPLIES_COST_PER_SEGMENT")), isEnable ? 200 : 15000);      // Discounted Resupply Cost, BIKER_PURCHASE_SUPPLIES_COST_PER_SEGMENT
    }

    /// <summary>
    /// 夜总会托尼洗钱费用
    /// </summary>
    /// <param name="isEnable"></param>
    public static void NightclubNoTonyLaunderingMoney(bool isEnable)
    {
        Globals.Set_Global_Value(Tunables.Index(-1002770353), isEnable ? 0.000001f : 0.1f);        // -1002770353  tuneables_processing.c
    }

    public static async Task Begin_transaction(uint category, uint action, int flags, uint[,] item = null)
    {
        await Task.Run(() =>
        {
            for (var i = 0; i < item.Length / 0x02; i++)
                if (__PED.GET_PED_TYPE(__PLAYER.PLAYER_PED_ID()) == 2)
                {
                    unsafe
                    {
                        var ptr = native_invoker.alloc_mem(1024);
                        native_invoker.add_special_native(Parse.Arg("NET_GAMESERVER_CHECKOUT_START"), Parse.Arg("shop_controller"));

                        if (__NETSHOPPING.NET_GAMESERVER_BASKET_IS_ACTIVE() == 1)
                            __NETSHOPPING.NET_GAMESERVER_BASKET_END();

                        if (ptr != 0)
                            if (__NETSHOPPING.NET_GAMESERVER_BEGIN_SERVICE(ptr, category, item[i, 0], action, (int)item[i, 1], flags) == 1)
                                __NETSHOPPING.NET_GAMESERVER_CHECKOUT_START(Memory.Read<int>((long)ptr));

                        native_invoker.remove_special_native(Parse.Arg("NET_GAMESERVER_CHECKOUT_START"));
                        native_invoker.free_mem(ptr);

                        while (__NETSHOPPING.NET_GAMESERVER_TRANSACTION_IN_PROGRESS() == 1) ;
                    }
                }
        });
    }
}
