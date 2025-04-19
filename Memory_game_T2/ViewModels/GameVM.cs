using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Memory_game_T2.Commands;
using Memory_game_T2.Models;

namespace Memory_game_T2.ViewModels
{
    public class GameVM : INotifyPropertyChanged
    {
        private string _category;
        private int _rows;
        private int _columns;
        private Card _firstCard;
        private DispatcherTimer _timer; //timer
        private TimeSpan _timeLeft;
        private int _flips; 
        private int _pairsFound;

        private static readonly IReadOnlyDictionary<string, string> CategoryPaths = new Dictionary<string, string>()
        {
            {"Disney", "../../../Assets/Disney" },
            {"Marvel", "../../../Assets/Marvel" },
            {"Fruits", "../../../Assets/Fruits" }
        };
        public ObservableCollection<Card> Cards { get; set; } = new ObservableCollection<Card>();

        public ICommand CardClickCommand { get; }
        public ICommand NewGameCommand { get; }

        public int Columns
        {
            get => _columns;
            set
            {
                _columns = value;
                OnPropertyChanged(nameof(Columns));
            }
        }
        public int Rows
        {
            get => _rows;
            set
            {
                _rows = value;
                OnPropertyChanged(nameof(Rows));
            }
        }
        public string TimeLeftDisplay => _timeLeft.ToString(@"mm\:ss"); //minutes & seconds
        public int Flips => _flips;
        public int PairsFound => _pairsFound;

        public GameVM(string category, int rows, int columns)
        {
            _category = category;
            _rows = rows;
            _columns = columns;

            CardClickCommand = new RelayCommand(o => CardClick(o));
            NewGameCommand = new RelayCommand(o => StartNewGame());

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += TimerTick; //se apeleaza fct Timertick la fiecare ticait
            
            StartNewGame();
        }
        private void StartNewGame()
        {
            _flips = 0;
            _pairsFound = 0;
            _timeLeft = TimeSpan.FromMinutes(1); //initial time 1 minute
            OnPropertyChanged(nameof(Flips));
            OnPropertyChanged(nameof(PairsFound));
            OnPropertyChanged(nameof(TimeLeftDisplay));

            InitializeGame();
            _timer.Start(); 
        }

        private void TimerTick(object sender, EventArgs e)
        {
            _timeLeft = _timeLeft.Subtract(TimeSpan.FromSeconds(1)); //scad cate o sec
            OnPropertyChanged(nameof(TimeLeftDisplay)); //actualizez UI-ul

            if (_timeLeft <= TimeSpan.Zero) //timeleft<=0->se opreste timpul
            {
                _timer.Stop();
                MessageBox.Show("Time's up! You lost!");
            }
        }
        private void InitializeGame()
        {
            Cards.Clear();
            var images = GetCategoryImages(_category);

            if (images.Count == 0)
            {
                MessageBox.Show($"No images found in category {_category}. Please check the image folder.");
                return;
            }

            //MessageBox.Show($"Found {images.Count} images in category {_category}.");
            int cardCount = _rows * _columns;

            if (cardCount % 2 != 0)
            {
                MessageBox.Show("The number of cards must be even!");
                return;
            }

            int pairsCount = cardCount / 2;
            if (images.Count < pairsCount)
            {
                MessageBox.Show("Not enough images in the selected category!");
                return;
            }

            images = images.Take(pairsCount).ToList();

            var cardsList = new List<Card>();
            foreach (var img in images)
            {
                cardsList.Add(new Card(img, _category));
                cardsList.Add(new Card(img, _category)); // 2 cards per image for matching
            }

            var random = new Random();
            cardsList = cardsList.OrderBy(x => random.Next()).ToList();

            foreach (var card in cardsList)
                Cards.Add(card);
        }


        private List<string> GetCategoryImages(string category)
        {
            var images = new List<string>();

            if (!CategoryPaths.TryGetValue(category, out string relativeFolderPath))
            {
                MessageBox.Show($"Category {category} does not exist in the dictionary!");
                return images;
            }

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;          
            string projectRoot = Path.GetFullPath(Path.Combine(baseDirectory, "..", "..", ".."));   // go back 3 directories to reach the project root
            string fullFolderPath = Path.Combine(projectRoot, "Assets", category);  // path to the category folder Assets

            Console.WriteLine($"Looking for images in folder: {fullFolderPath}");

            if (Directory.Exists(fullFolderPath))
            {
                var imageFiles = Directory.GetFiles(fullFolderPath, "*.*")
                    .Where(f => f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                           f.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                           f.EndsWith(".gif", StringComparison.OrdinalIgnoreCase));

                foreach (var file in imageFiles)
                {
                    var fileName = Path.GetFileName(file);
                    images.Add(fileName);
                    Console.WriteLine($"Found image: {fileName}");
                }
                Console.WriteLine($"Found {images.Count} images in category {category}.");
            }
            else
            {
                Console.WriteLine($"Folder {fullFolderPath} does not exist.");
                MessageBox.Show($"Image folder for {category} not found at: {fullFolderPath}");
            }

            return images;
        }
        private void CardClick(object cardObj)
        {
            if (cardObj is not Card card)
                return;

            if (card.IsFlipped || card.IsMatched)  // not allow clicking already flipped/matched cards
                return;

            Console.WriteLine($"Card clicked: {card.ImageCardPath}");
            card.IsFlipped = true;
            _flips++;
            OnPropertyChanged(nameof(Flips));

            if (_firstCard == null)
            {
                _firstCard = card;
                Console.WriteLine("First card selected");
            }
            else //  second card
            {
                Console.WriteLine("Second card selected");
                if (_firstCard.ImageCardPath == card.ImageCardPath) //match
                {
                    Console.WriteLine("Cards match!");
                    _pairsFound++;
                    OnPropertyChanged(nameof(PairsFound));

                    _firstCard.IsMatched = true;
                    card.IsMatched = true;

                    _firstCard.IsFlipped = true; //
                    card.IsFlipped = true;
                  
                    _firstCard = null; //reset the first card
                    if (_pairsFound == _rows * _columns / 2) //found all the matches
                    {
                        _timer.Stop();
                        MessageBox.Show("Congratulations! You won!");
                    }
                }
                else
                {
                    Console.WriteLine("Cards don't match!"); // flip them back after a delay

                    var first = _firstCard;
                    var second = card;

                    _timer.Stop(); // pause the main timer

                    var flipBackTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(1000) }; // create a short timer to show the cards 
                    
                    flipBackTimer.Tick += (s, e) => //temporar timer
                    {
                        flipBackTimer.Stop();
                        Console.WriteLine("Flipping cards back");

                        if (first != null) 
                            first.IsFlipped = false; //flip the first card

                        if (second != null)
                            second.IsFlipped = false; //flip the 2nd card

                        _firstCard = null;
                        _timer.Start(); // start the main timer
                    };
                    flipBackTimer.Start();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}