using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory_game_T2.Models
{
    public class Game
    {
        public string Category {  get; set; }
        public List<Card> Cards { get; set; } 
        public TimeSpan TimeRemaining { get; set; } 
        public TimeSpan TimeElapsed { get; set; } 
        public int Rows { get; set; }
        public int Columns { get; set; }
        public Game(string category, int rows, int columns)
        {
            Category = category;
            Rows = rows;
            Columns = columns;
            Cards = new List<Card>();
            TimeRemaining = TimeSpan.FromMinutes(5); 
            TimeElapsed = TimeSpan.Zero;
        }
    }
}
