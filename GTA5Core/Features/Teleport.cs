using GTA5Core.Native;
using GTA5Core.Offsets;
using System.Security.Policy;
namespace GTA5Core.Features;

public static class Teleport
{

    /// <summary>
    /// 获取玩家当前坐标
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetPlayerPosition()
    {
        var pCPed = Game.GetCPed();
        return Memory.Read<Vector3>(pCPed + CPed.VisualX);
    }

    /// <summary>
    /// 获取当前载具坐标
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetCurrentVehiclePosition()
    {
        var pCPed = Game.GetCPed();
        var pCVehicle = Memory.Read<long>(pCPed + CPed.CVehicle);
        return Memory.Read<Vector3>(pCVehicle + CVehicle.VisualX);
    }

    /// <summary>
    /// 获取准星当前坐标
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetCrossHairPosition()
    {
        var pCPlayerInfo = Game.GetCPlayerInfo();
        return Memory.Read<Vector3>(pCPlayerInfo + CPlayerInfo.CrossHairX);
    }

    /// <summary>
    /// 传送到准星坐标
    /// </summary>
    public static void ToCrossHair()
    {
        SetTeleportPosition(GetCrossHairPosition());
    }

    /// <summary>
    /// 传送到导航点
    /// </summary>
    public static void ToWaypoint()
    {
        var wayPos = GetWaypointPosition();

        // 避免误传送
        if (wayPos == Vector3.Zero)
            return;

        //SetTeleportPosition(wayPos);
        TowaypointForceGroundZ(wayPos);         // 更稳定的native传送
    }

    /// <summary>
    /// 传送到目标点
    /// </summary>
    public static void ToObjective()
    {
        var objPos = GetObjectivePosition();

        // 避免误传送
        if (objPos == Vector3.Zero)
            return;

        SetTeleportPosition(objPos);
    }

    /// <summary>
    /// 传送到导航点（Native）
    /// </summary>
    public static void TowaypointForceGroundZ(Vector3 blipV3)
    {
        if (blipV3 == Vector3.Zero)
            return;

        Vector3 vec3;
        vec3.X = blipV3.X;
        vec3.Y = blipV3.Y;
        vec3.Z = -255f;

        SetTeleportPosition(vec3);

        ulong outValue = native_invoker.alloc_mem(1024); // 分配一个1024字节的内存

        if (__WATER.GET_WATER_HEIGHT(vec3.X, vec3.Y, 800f, outValue) == 1)
        {
            vec3.Z = Memory.Read<float>((long)outValue);
            SetTeleportPosition(vec3);
        }
        else
        {
            if (__MISC.GET_GROUND_Z_FOR_3D_COORD(vec3.X, vec3.Y, 800f, outValue, 1, 0) == 0)
                for (var i = 0; i <= 8; i++)
                {
                    vec3.Z = 850f - (i * 100f);
                    SetTeleportPosition(vec3);
                    if (__MISC.GET_GROUND_Z_FOR_3D_COORD(vec3.X, vec3.Y, 800f, outValue, 1, 0) == 1)
                        break;
                }

            vec3.Z = Memory.Read<float>((long)outValue);
            SetTeleportPosition(vec3);
        }

        native_invoker.free_mem(outValue); // 释放一个内存只能是已分配的

        //var a = entity.handle_to_ptr(__PLAYER.PLAYER_PED_ID());
        //var b = entity.ptr_to_handle(a);

        //fire.add_owned_explosion(__PLAYER.PLAYER_PED_ID(), 0, 0, 0, 0, 0f, 1, 0, 0f);

        /*unsafe
        {
            Vector3 VEC3 = Parse.Arg(__ENTITY.GET_ENTITY_COORDS(__PLAYER.PLAYER_PED_ID(), 0));
        }*/

        /*unsafe
        {
             int value = 0;
             if (!stats.get_int(RAGE.JOAAT("mp0_cas_heist_flow"), &value))
                 return;
        }*/

        /*unsafe
        {
            var vehicles = 0;
            Vector3 v3 = Parse.Arg(__ENTITY.GET_ENTITY_COORDS(__PLAYER.PLAYER_PED_ID(), 0));
            var hashes = RAGE.JOAAT("kuruma2");

            if (streaming.request_model(hashes))
            {
                var mem = native_invoker.alloc_mem(1024);

                native_invoker.add_special_native(Parse.Arg("CREATE_VEHICLE"), Parse.Arg("activity_creator_prototype_launcher"));
                vehicles = __VEHICLE.CREATE_VEHICLE(hashes, v3.X, v3.Y, v3.Z, 0, 1, 0, 1);
                native_invoker.remove_special_native(Parse.Arg("CREATE_VEHICLE"));

                var scr = Parse.Arg(__ENTITY.GET_ENTITY_SCRIPT(vehicles, mem));
                native_invoker.free_mem(mem);
                __STREAMING.SET_MODEL_AS_NO_LONGER_NEEDED(hashes);
            }
        }*/
    }

    /// <summary>
    /// 传送到Blips
    /// </summary>
    public static void ToBlips(int blipId, byte blipColor = 0)
    {
        Vector3 vector3;

        if (blipColor == 0)
            vector3 = GetBlipPosition(new int[] { blipId });
        else
            vector3 = GetBlipPosition(new int[] { blipId }, new byte[] { blipColor });

        SetTeleportPosition(vector3);
    }

    /// <summary>
    /// 坐标传送功能
    /// </summary>
    public static void SetTeleportPosition(Vector3 vector3)
    {
        if (vector3 == Vector3.Zero)
            return;

        // 禁用越界死亡
        // Globals.Set_Global_Value(2738934 + 6958, 1);     // freemode - "TRI_WARP"

        var pCPed = Game.GetCPed();
        var pedHandle = __PLAYER.PLAYER_PED_ID();

        if (!Vehicle.IsInVehicle(pCPed))
            __ENTITY.SET_ENTITY_COORDS(pedHandle, vector3.X, vector3.Y, vector3.Z, 1, 1, 1, 1);
        else
            __ENTITY.SET_ENTITY_COORDS(__PED.GET_VEHICLE_PED_IS_IN(pedHandle, 0), vector3.X, vector3.Y, vector3.Z, 1, 1, 1, 1);

        /*
        var pCPed = Game.GetCPed();

        if (Vehicle.IsInVehicle(pCPed))
        {
            // 玩家在载具
            var pCVehicle = Memory.Read<long>(pCPed + CPed.CVehicle);
            Memory.Write(pCVehicle + CVehicle.VisualX, vector3);

            var pCNavigation = Memory.Read<long>(pCVehicle + CVehicle.CNavigation);
            Memory.Write(pCNavigation + CNavigation.PositionX, vector3);
        }
        else
        {
            // 玩家不在载具
            var pCNavigation = Memory.Read<long>(pCPed + CPed.CNavigation);

            Memory.Write(pCPed + CPed.VisualX, vector3);
            Memory.Write(pCNavigation + CNavigation.PositionX, vector3);
        }
        */
    }

    /// <summary>
    /// 获取Blip坐标
    /// </summary>
    public static Vector3 GetBlipPosition(int[] blipIds, byte[] blipColors = null)
    {
        if (blipIds is null || blipIds.Length == 0)
            return Vector3.Zero;

        var isIgnoreColor = false;
        if (blipColors is null || blipColors.Length == 0)
            isIgnoreColor = true;

        for (var i = 1; i <= 2000; i++)
        {
            var pBlip = Memory.Read<long>(Pointers.BlipPTR + i * 0x08);
            if (!Memory.IsValid(pBlip))
                continue;

            var dwIcon = Memory.Read<int>(pBlip + 0x40);
            var dwColor = Memory.Read<byte>(pBlip + 0x48);

            if (isIgnoreColor)
            {
                if (!blipIds.Contains(dwIcon))
                    continue;
            }
            else
            {
                if (!blipIds.Contains(dwIcon) ||
                    !blipColors.Contains(dwColor))
                    continue;
            }

            var vector3 = Memory.Read<Vector3>(pBlip + 0x10);
            vector3.Z = vector3.Z == 20.0f ? -225.0f : vector3.Z + 1.0f;

            return vector3;
        }

        return Vector3.Zero;
    }

    /// <summary>
    /// 获取导航点坐标
    /// </summary>
    public static Vector3 GetWaypointPosition()
    {
        return GetBlipPosition(new int[] { 8 }, new byte[] { 84 });
    }

    /// <summary>
    /// 获取目标点坐标
    /// </summary>
    public static Vector3 GetObjectivePosition()
    {
        Vector3 vector3;

        vector3 = GetBlipPosition(new int[] { 1 }, new byte[] { 5, 60, 66 });
        if (vector3 != Vector3.Zero)
            return vector3;

        vector3 = GetBlipPosition(new int[] { 1, 225, 427, 478, 501, 523, 556 }, new byte[] { 1, 2, 3, 54, 78 });
        if (vector3 != Vector3.Zero)
            return vector3;

        vector3 = GetBlipPosition(new int[] { 432, 443 }, new byte[] { 59 });
        if (vector3 != Vector3.Zero)
            return vector3;

        return vector3;
    }

    /// <summary>
    /// 坐标向前微调
    /// </summary>
    public static void MoveFoward(float distance)
    {
        var pCPed = Game.GetCPed();
        var pCNavigation = Memory.Read<long>(pCPed + CPed.CNavigation);

        var head = Memory.Read<float>(pCNavigation + CNavigation.RightX);
        var head2 = Memory.Read<float>(pCNavigation + CNavigation.RightY);

        var vector3 = Memory.Read<Vector3>(pCPed + CPed.VisualX);

        vector3.X -= head2 * distance;
        vector3.Y += head * distance;

        SetTeleportPosition(vector3);
    }

    /// <summary>
    /// 坐标向后微调
    /// </summary>
    /// <param name="distance">微调距离</param>
    public static void MoveBack(float distance)
    {
        var pCPed = Game.GetCPed();
        var pCNavigation = Memory.Read<long>(pCPed + CPed.CNavigation);

        var head = Memory.Read<float>(pCNavigation + CNavigation.RightX);
        var head2 = Memory.Read<float>(pCNavigation + CNavigation.RightY);

        var vector3 = Memory.Read<Vector3>(pCPed + CPed.VisualX);

        vector3.X += head2 * distance;
        vector3.Y -= head * distance;

        SetTeleportPosition(vector3);
    }

    /// <summary>
    /// 坐标向左微调
    /// </summary>
    /// <param name="distance">微调距离</param>
    public static void MoveLeft(float distance)
    {
        var pCPed = Game.GetCPed();
        var pCNavigation = Memory.Read<long>(pCPed + CPed.CNavigation);

        var head2 = Memory.Read<float>(pCNavigation + CNavigation.RightY);

        var vector3 = Memory.Read<Vector3>(pCPed + CPed.VisualX);

        vector3.X += distance;
        vector3.Y -= head2 * distance;

        SetTeleportPosition(vector3);
    }

    /// <summary>
    /// 坐标向右微调
    /// </summary>
    /// <param name="distance">微调距离</param>
    public static void MoveRight(float distance)
    {
        var pCPed = Game.GetCPed();
        var pCNavigation = Memory.Read<long>(pCPed + CPed.CNavigation);

        var head2 = Memory.Read<float>(pCNavigation + CNavigation.RightY);

        var vector3 = Memory.Read<Vector3>(pCPed + CPed.VisualX);

        vector3.X -= distance;
        vector3.Y += head2 * distance;

        SetTeleportPosition(vector3);
    }

    /// <summary>
    /// 坐标向上微调
    /// </summary>
    /// <param name="distance">微调距离</param>
    public static void MoveUp(float distance)
    {
        var vector3 = GetPlayerPosition();
        vector3.Z += distance;

        SetTeleportPosition(vector3);
    }

    /// <summary>
    /// 坐标向下微调
    /// </summary>
    /// <param name="distance">微调距离</param>
    public static void MoveDown(float distance)
    {
        var vector3 = GetPlayerPosition();
        vector3.Z -= distance;

        SetTeleportPosition(vector3);
    }
}
