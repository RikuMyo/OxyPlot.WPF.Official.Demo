using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace WpfExamples
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;
            this.Examples = this.GetExamples(this.GetType().Assembly).OrderBy(e => e.Title).ToArray();
        }

        /// <summary>
        /// Gets the examples.
        /// </summary>
        /// <value>The examples.</value>
        public IList<Example> Examples { get; private set; }

        /// <summary>
        /// Creates a thumbnail of the specified window.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="width">The width of the thumbnail.</param>
        /// <param name="path">The output path.</param>
        private static void CreateThumbnail(Window window, int width, string path)
        {
            var bitmap = ScreenCapture.Capture(
                (int)window.Left,
                (int)window.Top,
                (int)window.ActualWidth,
                (int)window.ActualHeight);
            var newHeight = width * bitmap.Height / bitmap.Width;
            var resizedBitmap = BitmapTools.Resize(bitmap, width, newHeight);
            resizedBitmap.Save(path);
        }

        /// <summary>
        /// Handles the MouseDoubleClick event of the ListBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs" /> instance containing the event data.</param>
        private void ListBoxMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var lb = (ListBox)sender;
            var example = lb.SelectedItem as Example;
            if (example != null)
            {
                var window = example.Create();
                window.Icon = this.Icon;
                window.Show();

                window.KeyDown += (s, args) =>
                {
                    if (args.Key == Key.F12)
                    {
                        CreateThumbnail(window, 120, System.IO.Path.Combine(@"..\..\Images\", example.ThumbnailFileName));
                        MessageBox.Show(window, "Demo image updated.");
                        e.Handled = true;
                    }
                };
            }
        }

        /// <summary>
        /// Gets the examples in the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly to scan.</param>
        /// <returns>A sequence of <see cref="Example" /> objects.</returns>
        private IEnumerable<Example> GetExamples(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                var ea = type.GetCustomAttributes(typeof(ExampleAttribute), false).FirstOrDefault() as ExampleAttribute;
                if (ea != null)
                {
                    yield return new Example(type, ea.Title, ea.Description);
                }
            }
        }
    }
}
