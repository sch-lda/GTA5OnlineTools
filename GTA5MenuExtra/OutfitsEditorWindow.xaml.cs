﻿using GTA5Core.Features;
using GTA5Shared.Helper;

namespace GTA5MenuExtra;

/// <summary>
/// OutfitsEditorWindow.xaml 的交互逻辑
/// </summary>
public partial class OutfitsEditorWindow
{
    public OutfitsEditorWindow()
    {
        InitializeComponent();
    }

    private void Window_OutfitsEditor_Loaded(object sender, RoutedEventArgs e)
    {
        ComboBox_OutfitIndex.SelectedIndex = 0;
    }

    private void Window_OutfitsEditor_Closing(object sender, CancelEventArgs e)
    {

    }

    ///////////////////////////////////////////////////////////////////////

    private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
    {
        ProcessHelper.OpenLink(e.Uri.OriginalString);
        e.Handled = true;
    }

    private void ReadOutfitsData()
    {
        if (ComboBox_OutfitIndex is null)
            return;

        var index = ComboBox_OutfitIndex.SelectedIndex;
        if (index ==-1)
            return;

        Outfits.OutfitIndex = index;

        TextBox_OutfitName.Text = Outfits.GetOutfitNameByIndex();

        TextBox_TOP.Text = Outfits.TOP.ToString();
        TextBox_TOP_TEX.Text = Outfits.TOP_TEX.ToString();

        TextBox_UNDERSHIRT.Text = Outfits.UNDERSHIRT.ToString();
        TextBox_UNDERSHIRT_TEX.Text = Outfits.UNDERSHIRT_TEX.ToString();

        TextBox_LEGS.Text = Outfits.LEGS.ToString();
        TextBox_LEGS_TEX.Text = Outfits.LEGS_TEX.ToString();

        TextBox_FEET.Text = Outfits.FEET.ToString();
        TextBox_FEET_TEX.Text = Outfits.FEET_TEX.ToString();

        TextBox_ACCESSORIES.Text = Outfits.ACCESSORIES.ToString();
        TextBox_ACCESSORIES_TEX.Text = Outfits.ACCESSORIES_TEX.ToString();

        TextBox_BAGS.Text = Outfits.BAGS.ToString();
        TextBox_BAGS_TEX.Text = Outfits.BAGS_TEX.ToString();

        TextBox_GLOVES.Text = Outfits.GLOVES.ToString();
        TextBox_GLOVES_TEX.Text = Outfits.GLOVES_TEX.ToString();

        TextBox_DECALS.Text = Outfits.DECALS.ToString();
        TextBox_DECALS_TEX.Text = Outfits.DECALS_TEX.ToString();

        TextBox_MASK.Text = Outfits.MASK.ToString();
        TextBox_MASK_TEX.Text = Outfits.MASK_TEX.ToString();

        TextBox_ARMOR.Text = Outfits.ARMOR.ToString();
        TextBox_ARMOR_TEX.Text = Outfits.ARMOR_TEX.ToString();

        ///////////////////////////////////////////////////////////////////////

        TextBox_HATS.Text = Outfits.HATS.ToString();
        TextBox_HATS_TEX.Text = Outfits.HATS_TEX.ToString();

        TextBox_GLASSES.Text = Outfits.GLASSES.ToString();
        TextBox_GLASSES_TEX.Text = Outfits.GLASSES_TEX.ToString();

        TextBox_EARS.Text = Outfits.EARS.ToString();
        TextBox_EARS_TEX.Text = Outfits.EARS_TEX.ToString();

        TextBox_WATCHES.Text = Outfits.WATCHES.ToString();
        TextBox_WATCHES_TEX.Text = Outfits.WATCHES_TEX.ToString();

        TextBox_WRIST.Text = Outfits.WRIST.ToString();
        TextBox_WRIST_TEX.Text = Outfits.WRIST_TEX.ToString();
    }

    private void WriteOutfitsData()
    {
        if (ComboBox_OutfitIndex is null)
            return;

        var index = ComboBox_OutfitIndex.SelectedIndex;
        if (index == -1)
            return;

        Outfits.OutfitIndex = index;

        Outfits.SetOutfitNameByIndex(TextBox_OutfitName.Text);

        Outfits.TOP = Convert.ToInt32(TextBox_TOP.Text);
        Outfits.TOP_TEX = Convert.ToInt32(TextBox_TOP_TEX.Text);

        Outfits.UNDERSHIRT = Convert.ToInt32(TextBox_UNDERSHIRT.Text);
        Outfits.UNDERSHIRT_TEX = Convert.ToInt32(TextBox_UNDERSHIRT_TEX.Text);

        Outfits.LEGS = Convert.ToInt32(TextBox_LEGS.Text);
        Outfits.LEGS_TEX = Convert.ToInt32(TextBox_LEGS_TEX.Text);

        Outfits.FEET = Convert.ToInt32(TextBox_FEET.Text);
        Outfits.FEET_TEX = Convert.ToInt32(TextBox_FEET_TEX.Text);

        Outfits.ACCESSORIES = Convert.ToInt32(TextBox_ACCESSORIES.Text);
        Outfits.ACCESSORIES_TEX = Convert.ToInt32(TextBox_ACCESSORIES_TEX.Text);

        Outfits.BAGS = Convert.ToInt32(TextBox_BAGS.Text);
        Outfits.BAGS_TEX = Convert.ToInt32(TextBox_BAGS_TEX.Text);

        Outfits.GLOVES = Convert.ToInt32(TextBox_GLOVES.Text);
        Outfits.GLOVES_TEX = Convert.ToInt32(TextBox_GLOVES_TEX.Text);

        Outfits.DECALS = Convert.ToInt32(TextBox_DECALS.Text);
        Outfits.DECALS_TEX = Convert.ToInt32(TextBox_DECALS_TEX.Text);

        Outfits.MASK = Convert.ToInt32(TextBox_MASK.Text);
        Outfits.MASK_TEX = Convert.ToInt32(TextBox_MASK_TEX.Text);

        Outfits.ARMOR = Convert.ToInt32(TextBox_ARMOR.Text);
        Outfits.ARMOR_TEX = Convert.ToInt32(TextBox_ARMOR_TEX.Text);

        ///////////////////////////////////////////////////////////////////////

        Outfits.HATS = Convert.ToInt32(TextBox_HATS.Text);
        Outfits.HATS_TEX = Convert.ToInt32(TextBox_HATS_TEX.Text);

        Outfits.GLASSES = Convert.ToInt32(TextBox_GLASSES.Text);
        Outfits.GLASSES_TEX = Convert.ToInt32(TextBox_GLASSES_TEX.Text);

        Outfits.EARS = Convert.ToInt32(TextBox_EARS.Text);
        Outfits.EARS_TEX = Convert.ToInt32(TextBox_EARS_TEX.Text);

        Outfits.WATCHES = Convert.ToInt32(TextBox_WATCHES.Text);
        Outfits.WATCHES_TEX = Convert.ToInt32(TextBox_WATCHES_TEX.Text);

        Outfits.WRIST = Convert.ToInt32(TextBox_WRIST.Text);
        Outfits.WRIST_TEX = Convert.ToInt32(TextBox_WRIST_TEX.Text);
    }

    ///////////////////////////////////////////////////////////////////////

    private void ComboBox_OutfitIndex_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ReadOutfitsData();
    }

    private void Button_Read_Click(object sender, RoutedEventArgs e)
    {
        AudioHelper.PlayClickSound();

        ReadOutfitsData();

        NotifierHelper.Show(NotifierType.Success, $"读取 槽位{Outfits.OutfitIndex} 角色服装数据 成功");
    }

    private void Button_Write_Click(object sender, RoutedEventArgs e)
    {
        AudioHelper.PlayClickSound();

        try
        {
            WriteOutfitsData();

            NotifierHelper.Show(NotifierType.Success, $"写入 槽位{Outfits.OutfitIndex} 角色服装数据 成功");
        }
        catch
        {
            NotifierHelper.Show(NotifierType.Warning, "部分数据不合法，请检查后重新写入");
        }
    }
}
