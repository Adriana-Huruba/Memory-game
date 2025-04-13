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
        private DispatcherTimer _timer;
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
        public string TimeLeftDisplay => _timeLeft.ToString(@"mm\:ss");
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
            _timer.Tick += TimerTick;

            StartNewGame();
        }
        private void StartNewGame()
        {
            _flips = 0;
            _pairsFound = 0;
            _timeLeft = TimeSpan.FromMinutes(2); //initial time 2 minutes
            OnPropertyChanged(nameof(Flips));
            OnPropertyChanged(nameof(PairsFound));
            OnPropertyChanged(nameof(TimeLeftDisplay));

            InitializeGame();
            _timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            _timeLeft = _timeLeft.Subtract(TimeSpan.FromSeconds(1));
            OnPropertyChanged(nameof(TimeLeftDisplay));

            if (_timeLeft <= TimeSpan.Zero)
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

            MessageBox.Show($"Found {images.Count} images in category {_category}.");
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
                cardsList.Add(new Card(img, _category)); // Two cards per image for matching
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

            // Get the base directory of the application
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Go up three directories to reach the project root
            string projectRoot = Path.GetFullPath(Path.Combine(baseDirectory, "..", "..", ".."));

            // Path to the category folder
            string fullFolderPath = Path.Combine(projectRoot, "Assets", category);

            Console.WriteLine($"Looking for images in folder: {fullFolderPath}");

            if (Directory.Exists(fullFolderPath))
            {
                // Search for image files with the desired extensions
                var imageFiles = Directory.GetFiles(fullFolderPath, "*.*")
                    .Where(f => f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                           f.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                           f.EndsWith(".gif", StringComparison.OrdinalIgnoreCase));

                foreach (var file in imageFiles)
                {
                    // Just use the filename, we'll reconstruct the path when needed
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

            // Don't allow clicking already flipped or matched cards
            if (card.IsFlipped || card.IsMatched)
                return;

            Console.WriteLine($"Card clicked: {card.ImageCardPath}");

            // Flip the card
            card.IsFlipped = true;
            _flips++;
            OnPropertyChanged(nameof(Flips));

            if (_firstCard == null)
            {
                // This is the first card in the pair
                _firstCard = card;
                Console.WriteLine("First card selected");
            }
            else // This is the second card
            {
                Console.WriteLine("Second card selected");

                // Check if we have a match
                if (_firstCard.ImageCardPath == card.ImageCardPath)
                {
                    Console.WriteLine("Cards match!");
                    // Cards match
                    _pairsFound++;
                    OnPropertyChanged(nameof(PairsFound));

                    // Mark both cards as matched
                    _firstCard.IsMatched = true;
                    card.IsMatched = true;

                    // Reset the first card selection
                    _firstCard = null;

                    // Check for win condition
                    if (_pairsFound == _rows * _columns / 2)
                    {
                        _timer.Stop();
                        MessageBox.Show("Congratulations! You won!");
                    }
                }
                else
                {
                    Console.WriteLine("Cards don't match!");
                    // Cards don't match - flip them back after a delay
                    _timer.Stop(); // Pause the main timer

                    // Create a short timer to show the cards briefly
                    var flipBackTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(1000) };
                    flipBackTimer.Tick += (s, e) =>
                    {
                        flipBackTimer.Stop();
                        Console.WriteLine("Flipping cards back");
                        _firstCard.IsFlipped = false;
                        card.IsFlipped = false;
                        _firstCard = null;
                        _timer.Start(); // Restart the main timer
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