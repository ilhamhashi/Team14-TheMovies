
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using TheMovies.MVVM.Model.Classes;
using TheMovies.MVVM.Model.Repositories;
using TheMovies.MVVM.View;

namespace TheMovies.MVVM.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        // Binder til det view, som skal vises i indholdssektionen
        private object currentView;
        public object CurrentView
        {
            get { return currentView; }
            set { currentView = value; OnPropertyChanged(); }
        }

        // Command der binder til knapperne i menuen
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand MovieViewCommand { get; set; }
        public RelayCommand MovieProgramViewCommand { get; set; }
        public RelayCommand BookingViewCommand { get; set; }
        public RelayCommand CloseMainWindowCommand { get; set; }

        // Definerer de forskellige viewmodels, der kan vises i indholdssektionen
        public HomeViewModel HomeVM { get; set; }
        public MovieViewModel MovieVM { get; set; }
        public MovieProgramViewModel MovieProgramVM { get; set; }
        public BookingViewModel BookingVM { get; set; }

        public MainWindowViewModel()
        {
            HomeVM = new HomeViewModel();
            MovieVM = new MovieViewModel();
            MovieProgramVM = new MovieProgramViewModel();
            BookingVM = new BookingViewModel();

            CurrentView = HomeVM;

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeVM;
            });

            MovieViewCommand = new RelayCommand(o =>
            {
                CurrentView = MovieVM;
            });

            MovieProgramViewCommand = new RelayCommand(o =>
            {
                CurrentView = MovieProgramVM;
            });

            BookingViewCommand = new RelayCommand(o =>
            {
                CurrentView = BookingVM;
            });

            CloseMainWindowCommand = new RelayCommand(o =>
            {
                Application.Current.Shutdown();
            });
        }

    }
}
