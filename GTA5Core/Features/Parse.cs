using GTA5Core.Features;
using GTA5Core.Native;
using GTA5Core.Offsets;
using System;
using System.Numerics;
using System.Reflection.Metadata;
unsafe public static class Parse
{
    public static sbyte* Arg(string arg) // string to sbyte*
    {
        return (sbyte*)Marshal.StringToHGlobalAnsi(arg);
    }
    public static string Arg(sbyte* arg) // sbyte* to string
    {
        return Marshal.PtrToStringAnsi((IntPtr)arg);
    }
    public static Vector3 Arg(ulong arg) // ulong to Vector3
    {
        Vector3 vec3;
        vec3.X = 0;
        vec3.Y = 0;
        vec3.Z = 0;

        if (arg == 0)
            return vec3;

        vec3.X = Memory.Read<float>((long)arg);
        vec3.Y = Memory.Read<float>((long)arg + 0x08);
        vec3.Z = Memory.Read<float>((long)arg + 0x10);

        native_invoker.free_mem(arg);

        return vec3;
    }
}

unsafe public static class script
{
    public static void trigger_script_event(int eventgroup, int playerbits, int[] args)
    {
        var mem_size = 0;
        var ptr = native_invoker.alloc_mem(1024);
        args[1] = __PLAYER.PLAYER_ID();

        for (var i = 0; i < args.Length; i++)
        {
            Memory.Write((long)ptr + (mem_size * 0x08), args[i]);
            mem_size++;
        }

        __SCRIPT.TRIGGER_SCRIPT_EVENT(eventgroup, ptr, sizeof(ulong), playerbits);
        native_invoker.free_mem(ptr);
    }
}

unsafe public static class entity
{
    private static long ASM_handle_to_ptr = 0;
    private static long ASM_ptr_to_handle = 0;

    public static ulong handle_to_ptr(int handle)
    {
        if (ASM_handle_to_ptr == 0)
            ASM_handle_to_ptr = Memory.FindPattern("83 F9 FF 74 31 4C 8B 0D");

        return native_invoker.call_asm((ulong)ASM_handle_to_ptr, (ulong)handle, 2000);
    }
    public static int ptr_to_handle(ulong ptr)
    {
        if (ASM_ptr_to_handle == 0)
            ASM_ptr_to_handle = Memory.FindPattern("48 8B F9 48 83 C1 10 33 DB") - 0x15;

        if (Memory.Read<ulong>((long)ptr) == 0)
            return 0;

        return (int)native_invoker.call_asm((ulong)ASM_ptr_to_handle, ptr, 2000);
    }
    public static void delete_entity(int handle)
    {
        if (__ENTITY.DOES_ENTITY_EXIST(handle) == 1)
        {
            var ptr = native_invoker.alloc_mem(1024);

            var entity_scrInfo = Parse.Arg(__ENTITY.GET_ENTITY_SCRIPT(handle, ptr));
            Memory.Write((long)ptr, handle);

            native_invoker.add_special_native(Parse.Arg("DELETE_ENTITY"), Parse.Arg(entity_scrInfo));

            __ENTITY.SET_ENTITY_AS_MISSION_ENTITY(handle, 1, 1);
            __ENTITY.DELETE_ENTITY(ptr);

            native_invoker.remove_special_native(Parse.Arg("DELETE_ENTITY"));
            native_invoker.free_mem(ptr);
        }
    }
}

unsafe public static class fire
{
    public static void add_owned_explosion(int pedHandle, float x, float y, float z, int explosionType, float damageScale, int isAudible, int isInvisible, float cameraShake)
    {
        native_invoker.add_special_native(Parse.Arg("ADD_OWNED_EXPLOSION"), Parse.Arg("am_mp_orbital_cannon"));
        __FIRE.ADD_OWNED_EXPLOSION(pedHandle, x, y, z, explosionType, damageScale, isAudible, isInvisible, cameraShake);
        native_invoker.remove_special_native(Parse.Arg("ADD_OWNED_EXPLOSION"));
    }
}

unsafe public static class streaming
{
    public static bool request_model(uint hash)
    {
        if (__STREAMING.IS_MODEL_VALID(hash) == 1)
        {
            bool model_loaded = (__STREAMING.HAS_MODEL_LOADED(hash) != 0);
            if (model_loaded)
                return true;

            do
            {
                model_loaded = (__STREAMING.HAS_MODEL_LOADED(hash) != 0);
                if (!model_loaded)
                    __STREAMING.REQUEST_MODEL(hash);
                else
                    return true;

            } while (!model_loaded);
        }

        return false;
    }
    public static bool request_model(string hash)
    {
        return request_model(RAGE.JOAAT(hash));
    }
}

unsafe public static class objects
{
    public static int create_object(uint hash, Vector3 vec3, bool dynamic)
    {
        var is_dynamic = 0;

        if (dynamic)
            is_dynamic = 1;

        var _object = 0;

        if (streaming.request_model(hash))
        {
            _object = __OBJECT.CREATE_OBJECT(hash, vec3.X, vec3.Y, vec3.Z, 1, 0, is_dynamic);
            __STREAMING.SET_MODEL_AS_NO_LONGER_NEEDED(hash);
        }

        return _object;
    }
    public static int create_object(string hash, Vector3 vec3, bool dynamic)
    {
        return create_object(RAGE.JOAAT(hash),vec3,dynamic);
    }
}

unsafe static class ped
{
    public static int create_ped(int pedType, uint hash, Vector3 vec3, float heading)
    {
        var peds = 0;

        if (streaming.request_model(hash))
        {
            peds = __PED.CREATE_PED(pedType, hash, vec3.X, vec3.Y, vec3.Z, heading, 1, 0);
            __STREAMING.SET_MODEL_AS_NO_LONGER_NEEDED(hash);
        }

        return peds;
    }

    public static int create_ped(int pedType, string hash, Vector3 vec3, float heading)
    {
        return create_ped(pedType, RAGE.JOAAT(hash), vec3, heading);
    }
}

unsafe public static class vehicle
{
    public static int create_vehicle(uint hash, Vector3 vec3, float heading)
    {
        var vehicles = 0;

        if (streaming.request_model(hash))
        {
            vehicles = __VEHICLE.CREATE_VEHICLE(hash, vec3.X, vec3.Y, vec3.Z, heading, 1, 0, 1);
            __DECORATOR.DECOR_SET_INT(vehicles, Parse.Arg("MPBitset"), 0);

            var network_id = __NETWORK.VEH_TO_NET(vehicles);
            if (__NETWORK.NETWORK_GET_ENTITY_IS_NETWORKED(vehicles) == 1)
                __NETWORK.SET_NETWORK_ID_EXISTS_ON_ALL_MACHINES(network_id, 1);

            __VEHICLE.SET_VEHICLE_IS_STOLEN(vehicles, 0);
            __STREAMING.SET_MODEL_AS_NO_LONGER_NEEDED(hash);
        }

        return vehicles;
    }
    public static int create_vehicle(string hash, Vector3 vec3, float heading)
    {
        return create_vehicle(RAGE.JOAAT(hash), vec3, heading);
    }
}

unsafe public static class stats
{
    public static bool getter<T>(uint hash, T* outvalue, Func<uint, ulong, int, int> callback)
    {
        var ptr = native_invoker.alloc_mem(1024);
        if (callback(hash, ptr, -1) == 1)
        {
            var value = Memory.Read<ulong>((long)ptr);
            void* temp = &value;
            *outvalue = *(T*)(temp);

            native_invoker.free_mem(ptr);
            return true;
        }

        native_invoker.free_mem(ptr);
        return false;
    }
    public static bool getter<T>(uint hash, T* outvalue, int bitstart, int bitsize, Func<uint, ulong, int, int, int, int> callback)
    {
        var ptr = native_invoker.alloc_mem(1024);
        if (callback(hash, ptr, bitstart, bitsize, -1) == 1)
        {
            var value = Memory.Read<ulong>((long)ptr);
            void* temp = &value;
            *outvalue = *(T*)(temp);

            native_invoker.free_mem(ptr);
            return true;
        }

        native_invoker.free_mem(ptr);
        return false;
    }
    public static bool get_int(uint hash, int* outvalue)
    {
        return getter<int>(hash, outvalue, __STATS.STAT_GET_INT);
    }
    public static bool get_float(uint hash, float* outvalue)
    {
        return getter<float>(hash, outvalue, __STATS.STAT_GET_FLOAT);
    }
    public static bool get_bool(uint hash, bool* outvalue)
    {
        return getter<bool>(hash, outvalue, __STATS.STAT_GET_BOOL);
    }
    public static bool get_masked_int(uint hash, int* outvalue, int bitstart, int bitsize)
    {
        return getter<int>(hash, outvalue, bitstart, bitsize, __STATS.STAT_GET_MASKED_INT);
    }
    public static bool get_bool_masked(uint hash, bool* outvalue, int bitindex)
    {
        return getter<bool>(hash, outvalue, bitindex, 1, __STATS.STAT_GET_MASKED_INT);
    }
    public static bool set_int(uint hash, int value)
    {
        if (__STATS.STAT_SET_INT(hash, value, 1) == 1)
            return true;

        return false;
    }
    public static bool set_float(uint hash, float value)
    {
        if (__STATS.STAT_SET_FLOAT(hash, value, 1) == 1)
            return true;

        return false;
    }
    public static bool set_bool(uint hash, bool value)
    {
        var int_value = 0;

        if (value)
            int_value = 1;

        if (__STATS.STAT_SET_BOOL(hash, int_value, 1) == 1)
            return true;

        return false;
    }
    public static bool set_masked_int(uint hash, int value, int bitstart, int bitsize)
    {
        if (__STATS.STAT_SET_MASKED_INT(hash, value, bitstart, bitsize, 1) == 1)
            return true;

        return false;
    }
    public static bool set_bool_masked(uint hash, bool value, int bitindex)
    {
        var int_value = 0;

        if (value)
            int_value = 1;

        if (set_masked_int(hash, int_value, bitindex, 1))
            return true;

        return false;
    }
    public static int get_mp_character_index()
    {
        int value = 0;
        if (!get_int(RAGE.JOAAT("mpply_last_mp_char"), &value))
            return -1;

        return value;
    }
    public static uint auto_character_slot(string hash)
    {
        string XPX = "";
        var pedType = __PED.GET_PED_TYPE(__PLAYER.PLAYER_PED_ID());

        switch(pedType)
        {
            case 0:
                XPX = "SP0_";
                break;
            case 1:
                XPX = "SP1_";
                break;
            case 2:
                XPX = "SP2_";
                break;
            case 3:
                XPX = "MP0_";
                break;
            case 4:
                XPX = "MP1_";
                break;
            default:
                XPX = "SP0_";
                break;
        }
        return RAGE.JOAAT(XPX + hash);
    }
}