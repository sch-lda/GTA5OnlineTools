﻿using CommunityToolkit.Mvvm.ComponentModel;

namespace GTA5OnlineTools.Models;

public partial class MainModel : ObservableObject
{
    /// <summary>
    /// GTA5是否运行
    /// </summary>
    [ObservableProperty]
    private bool isGTA5Run;

    /// <summary>
    /// 程序版本号
    /// </summary>
    [ObservableProperty]
    private Version appVersion;

    /// <summary>
    /// 程序版本号
    /// </summary>
    [ObservableProperty]
    private string status;

    /// <summary>
    /// 程序运行时间
    /// </summary>
    [ObservableProperty]
    private string appRunTime;
}
