using System.IO;
using System.Windows;

namespace BusRoute
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            var ofd = new Microsoft.Win32.OpenFileDialog();

            var dfead = ofd.ShowDialog();
            if (dfead == true)
            {
                using ( var stream = new StreamReader( ofd.OpenFile()))
                {
                    while (true)
                    {
                        var str = stream.ReadLine();
                        if (str == null) break;
                    }
                }
            }
        }
    }
}
