using GTA5OnlineTools.Data;
using GTA5OnlineTools.Utils;

using GTA5Shared.Helper;
using Microsoft.Win32;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace GTA5OnlineTools.Windows;

/// <summary>
/// FSLWindow.xaml 的交互逻辑
/// </summary>
public partial class FSLWindow
{
    private string tempPath = string.Empty;

    private const string fsl_url = "https://sstaticstp.cc2077.site/version.dll";
    private string GTA5_InstallPath_Steam = string.Empty;
    private string GTA5_InstallPath_Epic = string.Empty;
    public ObservableCollection<LuaInfo> OnlineLuas { get; set; } = new();
    private HttpClient _httpClient = new HttpClient();

    public FSLWindow()
    {
        InitializeComponent();
    }

    private void Window_FSL_Loaded(object sender, RoutedEventArgs e)
    {
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

    private void AppendLogger(string log)
    {
        TextBox_Logger.AppendText($"{log}\n");
        TextBox_Logger.ScrollToEnd();
    }

    private async void Button_StartDownload_Steam_Click(object sender, RoutedEventArgs e)
    {
        await StartDownload(GTA5_InstallPath_Steam);
    }

    private async void Button_StartDownload_Epic_Click(object sender, RoutedEventArgs e)
    {
        await StartDownload(GTA5_InstallPath_Epic);
    }

    private async Task StartDownload(string installPath)
    {
        Button_StartDownload_Steam.IsEnabled = false;
        Button_StartDownload_Epic.IsEnabled = false;
        Button_RM_FSL_Steam.IsEnabled = false;
        Button_RM_FSL_Epic.IsEnabled = false;

        ResetUIState();

        tempPath = Path.Combine(installPath, "version.dll");
        AppendLogger("开始下载...");

        try
        {
            using (var response = await _httpClient.GetAsync(fsl_url, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
                var totalBytes = response.Content.Headers.ContentLength;

                using (var fileStream = new FileStream(tempPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    var buffer = new byte[81920];
                    var receivedBytes = 0L;
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        int bytesRead;
                        while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            await fileStream.WriteAsync(buffer, 0, bytesRead);
                            receivedBytes += bytesRead;

                            // 更新进度
                            ProgressBar_Download.Value = receivedBytes;
                            if (totalBytes.HasValue)
                            {
                                TaskbarItemInfo.ProgressValue = (double)receivedBytes / totalBytes.Value;
                                TextBlock_Percentage.Text = $"{CoreUtil.GetFileForamtSize(receivedBytes)} / {CoreUtil.GetFileForamtSize(totalBytes.Value)}";
                            }
                        }
                    }
                }

                AppendLogger("下载完成,下次启动GTA时FSL将自动注入");
            }
        }
        catch (Exception ex)
        {
            AppendLogger($"下载失败: {ex.Message}");
        }
        finally
        {
            ResetButtons(installPath);
        }
    }

    private void ResetButtons(string installPath)
    {
        if (installPath == GTA5_InstallPath_Steam)
        {
            Button_GTA_STEAM_Dir.IsEnabled = true;
            Button_StartDownload_Steam.IsEnabled = true;
            Button_RM_FSL_Steam.IsEnabled = true;
        }
        else if (installPath == GTA5_InstallPath_Epic)
        {
            Button_GTA_EPIC_Dir.IsEnabled = true;
            Button_StartDownload_Epic.IsEnabled = true;
            Button_RM_FSL_Epic.IsEnabled = true;
        }
    }

    private void ResetUIState()
    {
        ProgressBar_Download.Maximum = 1024;
        ProgressBar_Download.Value = 0;

        TaskbarItemInfo.ProgressValue = 0;

        TextBlock_Percentage.Text = "0KB / 0MB";
    }

    private void Button_RM_FSL_Steam_Click(object sender, RoutedEventArgs e)
    {
        RemoveFile(GTA5_InstallPath_Steam);
    }

    private void Button_RM_FSL_Epic_Click(object sender, RoutedEventArgs e)
    {
        RemoveFile(GTA5_InstallPath_Epic);
    }

    private void RemoveFile(string installPath)
    {
        string filePath = Path.Combine(installPath, "version.dll");
        if (File.Exists(filePath))
        {
            if (!FileHelper.IsOccupied(filePath))
            {
                File.Delete(filePath);
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

    private void Button_GTA_STEAM_Dir_Click(object sender, RoutedEventArgs e)
    {
        ProcessHelper.OpenDir(GTA5_InstallPath_Steam);
    }
    private void Button_GTA_EPIC_Dir_Click(object sender, RoutedEventArgs e)
    {
        ProcessHelper.OpenDir(GTA5_InstallPath_Epic);
    }
}
