using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory_game_T2.Commands;
using System.Windows.Input;
using System.Windows;
using Memory_game_T2.Views;
using Memory_game_T2.Models;

namespace Memory_game_T2.ViewModels
{
    public class MenuVM : INotifyPropertyChanged
    {
        private string _category;
        private int _rows;
        private int _columns;
        public ICommand NewGameCommand { get; }
        public ICommand OpenGameCommand { get; }
        public ICommand SaveGameCommand { get; }
        public ICommand ShowStatsCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand ShowAboutCommand { get; }
        public ICommand SelectCategoryCommand { get; }
        public ICommand SetGameModeCommand { get; }

        //private RelayCommand setGameModeCommand;
        //public ICommand SetGameModeCommand => setGameModeCommand ??= new RelayCommand(SetGameMode);
        public string Category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged();
            }
        }

        public int Rows
        {
            get => _rows;
            set
            {
                _rows = value;
                OnPropertyChanged();
            }
        }

        public int Columns
        {
            get => _columns;
            set
            {
                _columns = value;
                OnPropertyChanged();
            }
        }

        public MenuVM(string category, int rows, int columns)
        {
            _category = category;
            _rows = rows;
            _columns = columns;

            NewGameCommand = new RelayCommand(_ => StartNewGame());
            OpenGameCommand = new RelayCommand(_ => OpenSavedGame());
            SaveGameCommand = new RelayCommand(_ => SaveCurrentGame());
            ShowStatsCommand = new RelayCommand(_ => ShowStatistics());
            ExitCommand = new RelayCommand(_ => ExitApplication());
            ShowAboutCommand = new RelayCommand(_ => ShowAbout());
            SelectCategoryCommand = new RelayCommand(o =>SelectCategory(o));
            SetGameModeCommand = new RelayCommand(o => SetGameMode(o));
        }

        private void StartNewGame()
        {
            var gameWindow = new GameWindow(_category, _rows, _columns);
            //var gameVM = new GameVM(_category, _rows, _columns);
           // gameWindow.DataContext = gameVM;
            gameWindow.Show();
        }

        private void SelectCategory(object parameter)
        {
            if (parameter is string category)
            {
                _category = category;
            }
        }
        private void SetGameMode(object commandParameter)
        {
            if (commandParameter is string mode)
            {
                switch (mode)
                {
                    case "Standard":
                        Rows = 4;
                        Columns = 4;
                        break;
                    case "Custom":
                        var customDialog = new CustomGameModeDialog();
                        if (customDialog.ShowDialog() == true)
                        {
                            Rows = customDialog.Rows;
                            Columns = customDialog.Columns;
                        }
                        break;
                }
            }
        }

        private void OpenSavedGame() 
        {
            /* Logica pentru deschiderea unui joc salvat */ 
        }
        private void SaveCurrentGame() 
        {
            /* Logica pentru salvarea jocului curent */ 
        }
        private void ShowStatistics() 
        {
            /* Logica pentru afișarea statisticilor */
        }
        private void ExitApplication()
        {
            //Application.Current.Shutdown();
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                //var loginPage = new MainWindow(); // aici ies de tot..........
                //mainWindow.MainFrame.Content = loginPage;
                mainWindow.MainFrame.Content = null;
                mainWindow.LoginArea.Visibility = Visibility.Visible; //ma duc la login
            }
        }
        private void ShowAbout()
        {
            MessageBox.Show("Student: Huruba Adriana\nEmail: adriana.huruba@student.unitbv.ro\nGrupa: 10LF331\nSpecializarea: Informatica Aplicata"); 
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

       
    }
}
