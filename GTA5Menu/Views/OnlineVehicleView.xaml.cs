﻿using CommunityToolkit.Mvvm.Input;
using GTA5Shared.Helper;

namespace GTA5Menu.Views;

/// <summary>
/// OnlineVehicleView.xaml 的交互逻辑
/// </summary>
public partial class OnlineVehicleView : UserControl
{
    /// <summary>
    /// 导航字典
    /// </summary>
    private readonly Dictionary<string, UserControl> NavDictionary = new();

    public OnlineVehicleView()
    {
        InitializeComponent();

        CreateView();
        Navigate(NavDictionary.First().Key);
    }

    /// <summary>
    /// 创建页面
    /// </summary>
    private void CreateView()
    {
        foreach (var item in ControlHelper.GetControls(Grid_NavMenu).Cast<RadioButton>())
        {
            var viewName = item.CommandParameter.ToString();

            if (NavDictionary.ContainsKey(viewName))
                continue;

            var typeView = Type.GetType($"GTA5Menu.Views.OnlineVehicle.{viewName}");
            if (typeView == null)
                continue;

            NavDictionary.Add(viewName, Activator.CreateInstance(typeView) as UserControl);
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
}
