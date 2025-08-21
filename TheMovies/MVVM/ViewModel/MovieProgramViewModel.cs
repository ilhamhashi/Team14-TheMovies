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

        public MovieProgramViewModel()
        {
            MoviePrograms = new ObservableCollection<MovieProgram>(movieProgramRepository.GetAll());
            MovieProgramCollectionView = CollectionViewSource.GetDefaultView(MovieProgramCollectionView);

        }
    }
}
