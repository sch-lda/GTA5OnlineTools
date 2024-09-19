﻿using GTA5OnlineTools.Data;
using GTA5OnlineTools.Models;
using GTA5OnlineTools.Utils;
using GTA5OnlineTools.Views;
using GTA5OnlineTools.Windows;

using GTA5Shared.Helper;

using CommunityToolkit.Mvvm.Input;

namespace GTA5OnlineTools;

/// <summary>
/// MainWindow.xaml 的交互逻辑
/// </summary>
public partial class MainWindow
{
    /// <summary>
    /// 主窗口数据模型
    /// </summary>
    public MainModel MainModel { get; set; } = new();

    ///////////////////////////////////////////////////////////////

    /// <summary>
    /// 导航字典
    /// </summary>
    private readonly Dictionary<string, UserControl> NavDictionary = new();

    ///////////////////////////////////////////////////////////////

    /// <summary>
    /// 主程序是否在运行，用于结束线程内循环
    /// </summary>
    public static bool IsAppRunning = true;

    /// <summary>
    /// 主窗口关闭事件
    /// </summary>
    public static event Action WindowClosingEvent;

    /// <summary>
    /// 用于向外暴露主窗口实例
    /// </summary>
    public static Window MainWindowInstance { get; private set; }

    /// <summary>
    /// 存储软件开始运行的时间
    /// </summary>
    private DateTime OriginDateTime;

    ///////////////////////////////////////////////////////////////

    public MainWindow()
    {
        InitializeComponent();

        CreateView();
    }

    /// <summary>
    /// 窗口加载完成事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_Main_Loaded(object sender, RoutedEventArgs e)
    {
        // 向外暴露主窗口实例
        MainWindowInstance = this;

        // 首页导航
        Navigate(NavDictionary.First().Key);

        // 获取当前时间，存储到对于变量中
        OriginDateTime = DateTime.Now;

        ///////////////////////////////////////////////////////////////

        // 设置主窗口标题
        this.Title = CoreUtil.MainAppWindowName + CoreUtil.ClientVersion;

        MainModel.AppRunTime = "00:00:00";
        MainModel.AppVersion = CoreUtil.ClientVersion;

        // 更新主窗口UI线程
        new Thread(UpdateUiThread)
        {
            Name = "UpdateUiThread",
            IsBackground = true
        }.Start();

        // 检查GTA5是否运行线程
        new Thread(CheckGTA5IsRunThread)
        {
            Name = "CheckGTA5IsRunThread",
            IsBackground = true
        }.Start();

        // 检查更新线程
        /*
        new Thread(CheckUpdateThread)
        {
            Name = "CheckUpdateThread",
            IsBackground = true
        }.Start();
        */
    }

    /// <summary>
    /// 窗口关闭事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_Main_Closing(object sender, CancelEventArgs e)
    {
        // 终止线程内循环
        IsAppRunning = false;

        WindowClosingEvent();
        LoggerHelper.Info("调用主窗口关闭事件成功");

        GTA5View.ActionCloseAllGTA5Window();
        LoggerHelper.Info("关闭小助手功能窗口成功");

        ProcessHelper.CloseThirdProcess();
        LoggerHelper.Info("关闭第三方进程成功");

        Application.Current.Shutdown();
        LoggerHelper.Info("主程序关闭\n\n");
    }

    ///////////////////////////////////////////////////////////////

    /// <summary>
    /// 创建页面
    /// </summary>
    private void CreateView()
    {
        foreach (var item in ControlHelper.GetControls(StackPanel_NavMenu).Cast<RadioButton>())
        {
            var viewName = item.CommandParameter.ToString();

            if (NavDictionary.ContainsKey(viewName))
                continue;

            var viewType = Type.GetType($"GTA5OnlineTools.Views.{viewName}");
            if (viewType == null)
                continue;

            NavDictionary.Add(viewName, Activator.CreateInstance(viewType) as UserControl);
        }
    }

    /// <summary>
    /// View页面导航
    /// </summary>
    /// <param name="viewName"></param>
    [RelayCommand]
    private void Navigate(string viewName)
    {
        if (!NavDictionary.ContainsKey(viewName))
            return;

        if (ContentControl_NavRegion.Content == NavDictionary[viewName])
            return;

        ContentControl_NavRegion.Content = NavDictionary[viewName];
    }

    /// <summary>
    /// 更新主窗口UI线程
    /// </summary>
    private void UpdateUiThread()
    {
        while (IsAppRunning)
        {
            // 获取软件运行时间
            MainModel.AppRunTime = CoreUtil.ExecDateDiff(OriginDateTime, DateTime.Now);

            Thread.Sleep(1000);
        }
    }

    /// <summary>
    /// 检查GTA5是否运行线程
    /// </summary>
    private void CheckGTA5IsRunThread()
    {
        bool isExecute = true;

        while (IsAppRunning)
        {
            // 判断 GTA5 是否运行
            MainModel.IsGTA5Run = ProcessHelper.IsGTA5Run();

            if (MainModel.IsGTA5Run)
            {
                isExecute = false;
            }
            else
            {
                // 下列功能只会在GTA5停止运行时执行一次，直到下一次GTA5停止运行
                if (!isExecute)
                {
                    isExecute = true;
                    GTA5View.ActionCloseAllGTA5Window();
                }
            }

            Thread.Sleep(1000);
        }
    }

    /// <summary>
    /// 检查更新线程
    /// </summary>
    private async void CheckUpdateThread()
    {
        try
        {
            LoggerHelper.Info("正在检测版本更新...");
            this.Dispatcher.Invoke(() =>
            {
                NotifierHelper.Show(NotifierType.Notification, "正在检测版本更新...");
            });

            // 检测版本更新
            var config = await HttpHelper.DownloadString("https://api.crazyzhang.cn/update/config.json");
            if (string.IsNullOrEmpty(config))
            {
                LoggerHelper.Error("网络异常");
                this.Dispatcher.Invoke(() =>
                {
                    NotifierHelper.Show(NotifierType.Error, "网络异常，这并不影响小助手程序使用");
                });
                return;
            }

            // 解析web返回的数据
            CoreUtil.UpdateInfo = JsonHelper.JsonDese<UpdateInfo>(config);
            // 获取对应数据
            CoreUtil.ServerVersion = Version.Parse(CoreUtil.UpdateInfo.Version);

            // 如果线上版本号小于等于本地版本号，则不提示更新
            if (CoreUtil.ServerVersion <= CoreUtil.ClientVersion)
            {
                LoggerHelper.Info($"当前已是最新版本 {CoreUtil.ServerVersion}");
                this.Dispatcher.Invoke(() =>
                {
                    NotifierHelper.Show(NotifierType.Notification, $"当前已是最新版本 {CoreUtil.ServerVersion}");
                });
                return;
            }

            // 打开更新对话框
            this.Dispatcher.Invoke(() =>
            {
                var UpdateWindow = new UpdateWindow
                {
                    // 设置父窗口
                    Owner = this
                };
                // 以对话框形式显示更新窗口
                UpdateWindow.ShowDialog();
            });
        }
        catch (Exception ex)
        {
            LoggerHelper.Error($"初始化异常", ex);
            this.Dispatcher.Invoke(() =>
            {
                NotifierHelper.Show(NotifierType.Error, $"初始化异常\n{ex.Message}");
            });
        }
    }
}
