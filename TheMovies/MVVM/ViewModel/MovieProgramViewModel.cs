using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using TheMovies.MVVM.Model.Classes;
using TheMovies.MVVM.Model.Repositories;

namespace TheMovies.MVVM.ViewModel
{
    internal class MovieProgramViewModel : ViewModelBase
    {
        private readonly MovieProgramFileRepository movieProgramRepository = new MovieProgramFileRepository("movieprograms.csv");

        public ObservableCollection<MovieProgram> MoviePrograms;
        public static ICollectionView MovieProgramCollectionView { get; set; }
        public static ICollectionView PrintCollectionView { get; set; }

        private Cinema selectedCinemaPrint;
        public Cinema SelectedCinemaPrint
        {
            get { return selectedCinemaPrint; }
            set
            {
                selectedCinemaPrint = value;
                OnPropertyChanged(nameof(PrintFilter));
                PrintCollectionView.Refresh();
            }
        }

        private DateTime selectedMonth;
        public DateTime SelectedMonth
        {
            get { return selectedMonth; }
            set
            {
                selectedMonth = value;
                OnPropertyChanged(nameof(PrintFilter));
                PrintCollectionView.Refresh();
            }
        }

        public MovieProgramViewModel()
        {
            MoviePrograms = new ObservableCollection<MovieProgram>(movieProgramRepository.GetAll());
            MovieProgramCollectionView = CollectionViewSource.GetDefaultView(MoviePrograms);

            PrintCollectionView = CollectionViewSource.GetDefaultView(MoviePrograms);
            PrintCollectionView.Filter = PrintFilter;
        }
        
        private bool PrintFilter(object obj)
        {
            if (obj is MovieProgram movieProgram)
            {
                return movieProgram.Cinema.Name.Equals(SelectedCinemaPrint.ToString(), StringComparison.InvariantCultureIgnoreCase) &&
                       movieProgram.ShowTime.Month.ToString().Equals(SelectedMonth.Month.ToString(), StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }
    }
}
