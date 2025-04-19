using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Memory_game_T2.Models
{
    public class User
    {
        public string UserName { get; set; }
        public string Image {  get; set; } //cale relativa catre imaginile user-ului
        public int GamesPlayed { get; set; } = 0;
        public int GamesWon { get; set; } = 0;

        public TimeSpan TotalTimePlayed { get; set; } = TimeSpan.Zero;

        [JsonIgnore]
        public string FormattedTime => $"{(int)TotalTimePlayed.TotalMinutes} min {(int)TotalTimePlayed.Seconds} sec";
    }
}
