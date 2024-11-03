using GTA5OnlineTools.Data;
using GTA5OnlineTools.Utils;

using GTA5Shared.Helper;
using Microsoft.Win32;
using System;
using Downloader;

namespace GTA5OnlineTools.Windows;

/// <summary>
/// FSLWindow.xaml 的交互逻辑
/// </summary>
public partial class FSLWindow
{
    private DownloadService _downloader;

    private string tempPath = string.Empty;

    private const string fsl_url = "https://sstaticstp.cc2077.site/version.dll";
    private string GTA5_InstallPath_Steam = string.Empty;
    private string GTA5_InstallPath_Epic = string.Empty;
    public ObservableCollection<LuaInfo> OnlineLuas { get; set; } = new();

    public FSLWindow()
    {
        InitializeComponent();
    }

    private void Window_FSL_Loaded(object sender, RoutedEventArgs e)
    {
        var downloadOpt = new DownloadConfiguration()
        {
            ClearPackageOnCompletionWithFailure = true,
            ReserveStorageSpaceBeforeStartingDownload = true
        };

        // 初始化下载库
        _downloader = new(downloadOpt);

        Button_GTA_STEAM_Dir.IsEnabled = false;
        Button_GTA_EPIC_Dir.IsEnabled = false;
        Button_StartDownload_Steam.IsEnabled = false;
        Button_StartDownload_Epic.IsEnabled = false;
        Button_RM_FSL_Steam.IsEnabled = false;
        Button_RM_FSL_Epic.IsEnabled = false;

        string registryPath = @"SOFTWARE\WOW6432Node\Rockstar Games\Grand Theft Auto V";
        string valueName_steam = "InstallFolderSteam";
        string valueName_epic = "InstallFolderEpic";
        using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryPath))
        {
            if (key != null)
            {
                object value_steam = key.GetValue(valueName_steam);
                if (value_steam != null)
                {
                    GTA5_InstallPath_Steam = value_steam.ToString();
                    if (Directory.Exists(GTA5_InstallPath_Steam))
                    {
                        Button_GTA_STEAM_Dir.IsEnabled = true;
                        Button_StartDownload_Steam.IsEnabled = true;
                        Button_RM_FSL_Steam.IsEnabled = true;
                        AppendLogger($"已从注册表获取GTA5 Steam版安装路径：{GTA5_InstallPath_Steam}");
                    }
                }
                object value_epic = key.GetValue(valueName_epic);
                if (value_epic != null)
                {
                    GTA5_InstallPath_Epic = value_epic.ToString();
                    if (Directory.Exists(GTA5_InstallPath_Epic))
                    {
                        Button_GTA_EPIC_Dir.IsEnabled = true;
                        Button_StartDownload_Epic.IsEnabled = true;
                        Button_RM_FSL_Epic.IsEnabled = true;
                        AppendLogger($"已从注册表获取GTA5 Epic版安装路径：{GTA5_InstallPath_Epic}");
                    }

                }
            }
        }
    }

    private async void Window_FSL_Closing(object sender, CancelEventArgs e)
    {
        await _downloader.CancelTaskAsync();
        _downloader.Dispose();
    }

    private void AppendLogger(string log)
    {
        TextBox_Logger.AppendText($"{log}\n");
        TextBox_Logger.ScrollToEnd();
    }

    //////////////////////////////////////////////////////////
    private void Button_RM_FSL_Steam_Click(object sender, RoutedEventArgs e)
    {
        
        if (File.Exists(Path.Combine(GTA5_InstallPath_Steam, "version.dll")))
        {
            if (!FileHelper.IsOccupied(Path.Combine(GTA5_InstallPath_Steam, "version.dll")))
            {
                File.Delete(Path.Combine(GTA5_InstallPath_Steam, "version.dll"));
                AppendLogger("已删除version.dll");
            }
            else
            {
                AppendLogger("version.dll被占用,无法删除请先关闭GTA5.");
            }
        }
        else
        {
            AppendLogger("version.dll不存在,无需移除");
        }
    }

    private void Button_RM_FSL_Epic_Click(object sender, RoutedEventArgs e)
    {
        
        if (File.Exists(Path.Combine(GTA5_InstallPath_Epic, "version.dll")))
        {
            if (!FileHelper.IsOccupied(Path.Combine(GTA5_InstallPath_Epic, "version.dll")))
            {
                AppendLogger("已删除version.dll");
                File.Delete(Path.Combine(GTA5_InstallPath_Epic, "version.dll"));
            }
            else
            {
                AppendLogger("version.dll被占用,无法删除.请先关闭GTA5.");
            }
        }
        else
        {
            AppendLogger("version.dll不存在,无需移除");
        }
    }

    private async void Button_StartDownload_Steam_Click(object sender, RoutedEventArgs e)
    {
        

        // ClearLogger();

        Button_StartDownload_Steam.IsEnabled = false;
        Button_StartDownload_Epic.IsEnabled = false;
        Button_CancelDownload.IsEnabled = true;
        Button_RM_FSL_Steam.IsEnabled = false;
        Button_RM_FSL_Epic.IsEnabled = false;

        ResetUIState();

        _downloader.DownloadStarted -= DownloadStarted;
        _downloader.DownloadProgressChanged -= DownloadProgressChanged;
        _downloader.DownloadFileCompleted -= DownloadFileCompleted;

        _downloader.DownloadStarted += DownloadStarted;
        _downloader.DownloadProgressChanged += DownloadProgressChanged;
        _downloader.DownloadFileCompleted += DownloadFileCompleted;

        tempPath = Path.Combine(GTA5_InstallPath_Steam, "version.dll");

        AppendLogger("开始下载...");
        await _downloader.DownloadFileTaskAsync(fsl_url, tempPath);
    }

    private async void Button_StartDownload_Epic_Click(object sender, RoutedEventArgs e)
    {
        

        // ClearLogger();

        Button_StartDownload_Steam.IsEnabled = false;
        Button_StartDownload_Epic.IsEnabled = false;
        Button_CancelDownload.IsEnabled = true;
        Button_RM_FSL_Steam.IsEnabled = false;
        Button_RM_FSL_Epic.IsEnabled = false;

        ResetUIState();

        _downloader.DownloadStarted -= DownloadStarted;
        _downloader.DownloadProgressChanged -= DownloadProgressChanged;
        _downloader.DownloadFileCompleted -= DownloadFileCompleted;

        _downloader.DownloadStarted += DownloadStarted;
        _downloader.DownloadProgressChanged += DownloadProgressChanged;
        _downloader.DownloadFileCompleted += DownloadFileCompleted;

        tempPath = Path.Combine(GTA5_InstallPath_Epic, "version.dll");

        AppendLogger("开始下载...");
        await _downloader.DownloadFileTaskAsync(fsl_url, tempPath);
    }

    private async void Button_CancelDownload_Click(object sender, RoutedEventArgs e)
    {
        

        Button_StartDownload_Steam.IsEnabled = false;
        Button_StartDownload_Epic.IsEnabled = false;
        Button_CancelDownload.IsEnabled = false;

        await _downloader.CancelTaskAsync();

        ResetUIState();
        AppendLogger("下载取消");
        if (GTA5_InstallPath_Steam != string.Empty)
        {
            Button_GTA_STEAM_Dir.IsEnabled = true;
            Button_StartDownload_Steam.IsEnabled = true;
            Button_RM_FSL_Steam.IsEnabled = true;
        }
        if (GTA5_InstallPath_Epic != string.Empty)
        {
            Button_GTA_EPIC_Dir.IsEnabled = true;
            Button_StartDownload_Epic.IsEnabled = true;
            Button_RM_FSL_Epic.IsEnabled = true;
        }

        Button_CancelDownload.IsEnabled = false;
    }

    //////////////////////////////////////////////////////////

    /// <summary>
    /// 重置UI显示
    /// </summary>
    private void ResetUIState()
    {
        ProgressBar_Download.Maximum = 1024;
        ProgressBar_Download.Value = 0;

        TaskbarItemInfo.ProgressValue = 0;

        TextBlock_Percentage.Text = "0KB / 0MB";
    }

    private void DownloadStarted(object sender, DownloadStartedEventArgs e)
    {
        this.Dispatcher.Invoke(() =>
        {
            ProgressBar_Download.Maximum = e.TotalBytesToReceive;
        });
    }

    private void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
    {
        this.Dispatcher.Invoke(() =>
        {
            ProgressBar_Download.Value = e.ReceivedBytesSize;
            TaskbarItemInfo.ProgressValue = ProgressBar_Download.Value / ProgressBar_Download.Maximum;

            TextBlock_Percentage.Text = $"{CoreUtil.GetFileForamtSize(e.ReceivedBytesSize)} / {CoreUtil.GetFileForamtSize(e.TotalBytesToReceive)}";
        });
    }

    private void DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
    {
        this.Dispatcher.Invoke(() =>
        {
            if (e.Error != null)
            {
                ResetUIState();

                if (GTA5_InstallPath_Steam != string.Empty)
                {
                    Button_GTA_STEAM_Dir.IsEnabled = true;
                    Button_StartDownload_Steam.IsEnabled = true;
                    Button_RM_FSL_Steam.IsEnabled = true;
                }
                if (GTA5_InstallPath_Epic != string.Empty)
                {
                    Button_GTA_EPIC_Dir.IsEnabled = true;
                    Button_StartDownload_Epic.IsEnabled = true;
                    Button_RM_FSL_Epic.IsEnabled = true;
                }

                Button_CancelDownload.IsEnabled = false;

                AppendLogger("下载失败.如果网络连接失败，请尝试使用 工具-运行网络诊断 修复");
                AppendLogger($"错误信息：{e.Error.Message}");
                return Task.CompletedTask;
            }

            try
            {
                AppendLogger("操作结束,下次启动GTA时FSL将自动注入");
            }
            catch (Exception ex)
            {
                AppendLogger($"捕获到异常：{ex.Message}");
            }
            finally
            {
                if (GTA5_InstallPath_Steam != string.Empty)
                {
                    Button_GTA_STEAM_Dir.IsEnabled = true;
                    Button_StartDownload_Steam.IsEnabled = true;
                    Button_RM_FSL_Steam.IsEnabled = true;
                }
                if (GTA5_InstallPath_Epic != string.Empty)
                {
                    Button_GTA_EPIC_Dir.IsEnabled = true;
                    Button_StartDownload_Epic.IsEnabled = true;
                    Button_RM_FSL_Epic.IsEnabled = true;
                }
                Button_CancelDownload.IsEnabled = false;
            }

            return Task.CompletedTask;
        });
    }

    private void Button_GTA_STEAM_Dir_Click(object sender, RoutedEventArgs e)
    {
        ProcessHelper.OpenDir(GTA5_InstallPath_Steam);
    }
    private void Button_GTA_EPIC_Dir_Click(object sender, RoutedEventArgs e)
    {
        ProcessHelper.OpenDir(GTA5_InstallPath_Epic);
    }

}
