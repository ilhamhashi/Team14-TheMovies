using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;

namespace TheMovies.MVVM.View
{
    /// <summary>
    /// Interaction logic for MovieProgram.xaml
    /// </summary>
    public partial class MovieProgramView : Window
    {
        public MovieProgramView()
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
                MovieProgramTable.Measure(pageSize);
                MovieProgramTable.Arrange(new Rect(5, 5, pageSize.Width, pageSize.Height));
                Printdlg.PrintVisual(MovieProgramTable, Title);
            }
        }

    }
}
