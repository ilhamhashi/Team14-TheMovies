using System.Windows;


namespace TheMovies.MVVM.View
{
    /// <summary>
    /// Interaction logic for PrintView.xaml
    /// </summary>
    public partial class PrintView : Window
    {
        public PrintView()
        {
            InitializeComponent();
        }

        private void OnListBoxPrinting(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.PrintDialog Printdlg = new System.Windows.Controls.PrintDialog();
            if ((bool)Printdlg.ShowDialog().GetValueOrDefault())
            {
                Size pageSize = new Size(Printdlg.PrintableAreaWidth, Printdlg.PrintableAreaHeight);
                // sizing of the element.
                PrintMovieProgramList.Measure(pageSize);
                PrintMovieProgramList.Arrange(new Rect(5, 5, pageSize.Width, pageSize.Height));
                Printdlg.PrintVisual(PrintMovieProgramList, Title);
            }
        }
    }
}
