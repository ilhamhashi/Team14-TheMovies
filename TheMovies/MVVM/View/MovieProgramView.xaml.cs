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

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
