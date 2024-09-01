﻿using GTA5Core.Native;
using GTA5Core.Offsets;
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
    private static long patch_owned_explosion = 0;
    public static void add_owned_explosion(int pedHandle, float x, float y, float z, int explosionType, float damageScale, int isAudible, int isInvisible, float cameraShake)
    {
        if (patch_owned_explosion == 0)
            patch_owned_explosion = Memory.FindPattern("40 0F 94 C7 E8 ?? ?? ?? ?? 84 C0 74 ?? 44 38 35") - 0x02;

        native_invoker.add_special_native(Parse.Arg("ADD_OWNED_EXPLOSION"), Parse.Arg("am_mp_orbital_cannon"));
        Memory.Write(patch_owned_explosion, (ushort)0xFF39);

        __FIRE.ADD_OWNED_EXPLOSION(pedHandle, x, y, z, explosionType, damageScale, isAudible, isInvisible, cameraShake);

        Memory.Write(patch_owned_explosion, (ushort)0xC739);
        native_invoker.remove_special_native(Parse.Arg("ADD_OWNED_EXPLOSION"));
    }
}