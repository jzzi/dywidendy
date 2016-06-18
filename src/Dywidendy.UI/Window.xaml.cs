using Microsoft.Practices.Unity;

namespace Dywidendy.UI
{
    /// <summary>
    /// Interaction logic for Window.xaml
    /// </summary>
    public partial class Window
    {
        public Window()
        {
            InitializeComponent();
        }

        [Dependency]
        public WplacViewModel ViewModel { set { DataContext = value; } }
    }
}
