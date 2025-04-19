using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory_game_T2.Models;

namespace Memory_game_T2.ViewModels
{
    public class StatisticsVM : INotifyPropertyChanged
    {
        public User CurrentUser { get; }
        public StatisticsVM(User currentUser)
        {
            CurrentUser = currentUser;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
