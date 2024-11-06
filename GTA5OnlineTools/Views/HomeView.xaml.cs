namespace GTA5OnlineTools.Views;

/// <summary>
/// HomeView.xaml 的交互逻辑
/// </summary>
public partial class HomeView : UserControl
{
    private readonly StringBuilder builder = new();

    public HomeView()
    {
        InitializeComponent();
        MainWindow.WindowClosingEvent += MainWindow_WindowClosingEvent;

    }

    private void MainWindow_WindowClosingEvent()
    {

    }

}
