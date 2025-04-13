using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory_game_T2.Models
{
    public class User
    {
        public string UserName { get; set; }
        public string Image {  get; set; } //cale relativa catre imaginile user-ului
        public int GamesPlayed {  get; set; }
        public int GamesWon {  get; set; }
    }
}
