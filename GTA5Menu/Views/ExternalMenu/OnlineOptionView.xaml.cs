﻿using GTA5Core.Features;
using GTA5Core.GTA.Onlines;
using GTA5Shared.Helper;

namespace GTA5Menu.Views.ExternalMenu;

/// <summary>
/// OnlineOptionView.xaml 的交互逻辑
/// </summary>
public partial class OnlineOptionView : UserControl
{
    private class Options
    {
        public bool FreeChangeAppearance = false;
        public bool ChangeAppearanceCooldown = false;

        public bool PassiveModeCooldown = false;
        public bool SuicideCooldown = false;
        public bool OrbitalCooldown = false;
        public bool SellOnNonPublic = false;
        public bool SessionSnow = false;
    }
    private readonly Options _options = new();

    public OnlineOptionView()
    {
        InitializeComponent();
        GTA5MenuWindow.WindowClosingEvent += GTA5MenuWindow_WindowClosingEvent;
        GTA5MenuWindow.LoopSpeedNormalEvent += GTA5MenuWindow_LoopSpeedNormalEvent;
    }

    private void GTA5MenuWindow_WindowClosingEvent()
    {
        GTA5MenuWindow.WindowClosingEvent -= GTA5MenuWindow_WindowClosingEvent;
        GTA5MenuWindow.LoopSpeedNormalEvent -= GTA5MenuWindow_LoopSpeedNormalEvent;
    }

    private void GTA5MenuWindow_LoopSpeedNormalEvent()
    {
        // 免费更改角色外观
        if (_options.FreeChangeAppearance)
            Online.FreeChangeAppearance(true);
        // 移除更改角色外观冷却
        if (_options.ChangeAppearanceCooldown)
            Online.ChangeAppearanceCooldown(true);

        // 移除被动模式冷却
        if (_options.PassiveModeCooldown)
            Online.PassiveModeCooldown(true);
        // 移除自杀冷却
        if (_options.SuicideCooldown)
            Online.SuicideCooldown(true);
        // 移除天基炮冷却
        if (_options.OrbitalCooldown)
            Online.OrbitalCooldown(true);
        // 非公开战局运货
        if (_options.SellOnNonPublic)
            Online.SellOnNonPublic(true);
        // 战局雪天 (自己可见)
        if (_options.SessionSnow)
            Online.SessionSnow(true);
    }

    /////////////////////////////////////////////////////////////

    private void Button_Sessions_Click(object sender, RoutedEventArgs e)
    {
        

        if (sender is Button button)
        {
            var btnContent = button.Content.ToString();

            var session = OnlineData.Sessions.Find(t => t.Name == btnContent);
            if (session != null)
                Online.LoadSession(session.Value);
        }
    }

    private void Button_EmptySession_Click(object sender, RoutedEventArgs e)
    {
        

        Online.EmptySession();
    }

    private void Button_Disconnect_Click(object sender, RoutedEventArgs e)
    {
        

        Online.Disconnect();
    }

    private void Button_StopCutscene_Click(object sender, RoutedEventArgs e)
    {
        

        Online.StopCutscene();
    }

    private void CheckBox_FreeChangeAppearance_Click(object sender, RoutedEventArgs e)
    {
        _options.FreeChangeAppearance = CheckBox_FreeChangeAppearance.IsChecked == true;
        Online.FreeChangeAppearance(_options.FreeChangeAppearance);
    }

    private void CheckBox_ChangeAppearanceCooldown_Click(object sender, RoutedEventArgs e)
    {
        _options.ChangeAppearanceCooldown = CheckBox_ChangeAppearanceCooldown.IsChecked == true;
        Online.ChangeAppearanceCooldown(_options.ChangeAppearanceCooldown);
    }

    private void CheckBox_PassiveModeCooldown_Click(object sender, RoutedEventArgs e)
    {
        _options.PassiveModeCooldown = CheckBox_PassiveModeCooldown.IsChecked == true;
        Online.PassiveModeCooldown(_options.PassiveModeCooldown);
    }

    private void CheckBox_SuicideCooldown_Click(object sender, RoutedEventArgs e)
    {
        _options.SuicideCooldown = CheckBox_SuicideCooldown.IsChecked == true;
        Online.SuicideCooldown(_options.SuicideCooldown);
    }

    private void CheckBox_OrbitalCooldown_Click(object sender, RoutedEventArgs e)
    {
        _options.OrbitalCooldown = CheckBox_OrbitalCooldown.IsChecked == true;
        Online.OrbitalCooldown(_options.OrbitalCooldown);
    }

    private void CheckBox_SellOnNonPublic_Click(object sender, RoutedEventArgs e)
    {
        _options.SellOnNonPublic = CheckBox_SellOnNonPublic.IsChecked == true;
        Online.SellOnNonPublic(_options.SellOnNonPublic);
    }

    private void CheckBox_SessionSnow_Click(object sender, RoutedEventArgs e)
    {
        _options.SessionSnow = CheckBox_SessionSnow.IsChecked == true;
        Online.SessionSnow(_options.SessionSnow);
    }

    /////////////////////////////////////////////////////////////

    private void CheckBox_OffRadar_Click(object sender, RoutedEventArgs e)
    {
        Online.OffRadar(CheckBox_OffRadar.IsChecked == true);
    }

    private void CheckBox_GhostOrganization_Click(object sender, RoutedEventArgs e)
    {
        Online.GhostOrganization(CheckBox_GhostOrganization.IsChecked == true);
    }

    private void CheckBox_BribeOrBlindCops_Click(object sender, RoutedEventArgs e)
    {
        Online.BribeOrBlindCops(CheckBox_BribeOrBlindCops.IsChecked == true);
    }

    private void CheckBox_BribeAuthorities_Click(object sender, RoutedEventArgs e)
    {
        Online.BribeAuthorities(CheckBox_BribeAuthorities.IsChecked == true);
    }

    private void CheckBox_RevealPlayers_Click(object sender, RoutedEventArgs e)
    {
        Online.RevealPlayers(CheckBox_RevealPlayers.IsChecked == true);
    }

    private void Button_Blips_Click(object sender, RoutedEventArgs e)
    {
        

        if (sender is Button button)
        {
            var btnContent = button.Content.ToString();

            var blip = OnlineData.Blips.Find(t => t.Name == btnContent);
            if (blip != null)
                Teleport.ToBlips(blip.Value);
        }
    }

    private void Button_Teleport_Freak_Shop_Click (object sender, RoutedEventArgs e)
    {
        

        Teleport.SetTeleportPosition(new Vector3(605.638f, -409.474f, 24.748f));
    }

    private async void Button_Teleport_Gun_Van_Click(object sender, RoutedEventArgs e)
    {
        

        Globals.Set_Global_Value<float>(1949748, -1005.75f); // freemode.ysc return -29.532f, 6435.136f, 31.162f;
        Globals.Set_Global_Value<float>(1949748 + 1, 25.2f);
        Globals.Set_Global_Value<float>(1949748 + 2, 49.25f);
        await Task.Delay(2000);
        Teleport.SetTeleportPosition(new Vector3(-1001.53f, 25.01f, 52.01f));
    }

    private void Button_MerryweatherServices_Click(object sender, RoutedEventArgs e)
    {
        

        if (sender is Button button)
        {
            var btnContent = button.Content.ToString();

            var service = OnlineData.MerryWeatherServices.Find(t => t.Name == btnContent);
            if (service != null)
                Online.MerryWeatherServices(service.Value);
        }
    }

    private void Button_InstantBullShark_Click(object sender, RoutedEventArgs e)
    {
        

        Online.InstantBullShark(true);
    }

    private void Button_RemoveBullShark_Click(object sender, RoutedEventArgs e)
    {
        

        Online.InstantBullShark(false);
    }

    private void Button_BackupHeli_Click(object sender, RoutedEventArgs e)
    {
        

        Online.CallBackupHeli(true);
    }

    private void Button_Airstrike_Click(object sender, RoutedEventArgs e)
    {
        

        Online.CallAirstrike(true);
    }

    /////////////////////////////////////////////////////////////

    private void Button_RequestKosatka_Click(object sender, RoutedEventArgs e)
    {
        

        Online.RequestKosatka();
    }

    private async void Button_TriggerRCBandito_Click(object sender, RoutedEventArgs e)
    {
        

        Online.TriggerRCBandito(true);
        await Task.Delay(100);
        Online.TriggerRCBandito(false);
    }

    private async void Button_TriggerMiniTank_Click(object sender, RoutedEventArgs e)
    {
        

        Online.TriggerMiniTank(true);
        await Task.Delay(100);
        Online.TriggerMiniTank(false);
    }

    private void CheckBox_KosatkaMissleCooldown_Click(object sender, RoutedEventArgs e)
    {
        Online.KosatkaMissleCooldown(CheckBox_KosatkaMissleCooldown.IsChecked == true);
    }
}
