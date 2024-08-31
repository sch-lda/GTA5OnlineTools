using GTA5Core.Native;
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