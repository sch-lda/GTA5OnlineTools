using GTA5Core.Native;
using GTA5Core.Offsets;
using System.Reflection.Metadata;
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