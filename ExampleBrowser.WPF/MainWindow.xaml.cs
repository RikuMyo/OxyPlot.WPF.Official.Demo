using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExampleBrowser.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The vm.
        /// </summary>
        private readonly MainWindowViewModel vm = new MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();

            this.InitializeComponent();

            this.DataContext = this.vm;

            SystemEvents.DisplaySettingsChanged += this.SystemEvents_DisplaySettingsChanged;
        }
        private void SystemEvents_DisplaySettingsChanged(object sender, System.EventArgs e)
        {
            this.vm.ActiveModel?.PlotView?.InvalidatePlot(false);
        }

    }
}
