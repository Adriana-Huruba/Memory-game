using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Memory_game_T2.Models
{
    public class Card : INotifyPropertyChanged
    {
        private string _imageCardPath;
        private bool _isFlipped;
        private bool _isMatched;
        private string _category;
        private ImageSource _cachedImage;

        public string ImageCardPath
        {
            get => _imageCardPath;
            set
            {
                if (_imageCardPath != value)
                {
                    _imageCardPath = value;
                    OnPropertyChanged();
                    // Clear cached image when path changes
                    _cachedImage = null;
                    OnPropertyChanged(nameof(Image));
                }
            }
        }

        public bool IsFlipped
        {
            get => _isFlipped;
            set
            {
                if (_isFlipped != value)
                {
                    _isFlipped = value;
                    OnPropertyChanged();
                    Console.WriteLine($"Card flipped: {_isFlipped}, Image path: {_imageCardPath}");
                }
            }
        }

        public bool IsMatched
        {
            get => _isMatched;
            set
            {
                if (_isMatched != value)
                {
                    _isMatched = value;
                    OnPropertyChanged();
                }
            }
        }

        public Card(string imagePath, string category)
        {
            _imageCardPath = imagePath;
            _isFlipped = false;
            _isMatched = false;
            _category = category;
            Console.WriteLine($"Card created with image: {imagePath}, category: {category}");
        }

        public ImageSource Image
        {
            get
            {
                // Return cached image if available
                if (_cachedImage != null)
                    return _cachedImage;

                try
                {
                    // Get the base directory of the application
                    string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

                    // Go up three directories from the bin/Debug/net folder to reach the project root
                    string projectRoot = Path.GetFullPath(Path.Combine(baseDirectory, "..", "..", ".."));

                    // Combine to get the full path to the image
                    string fullPath = Path.Combine(projectRoot, "Assets", _category, _imageCardPath);

                    Console.WriteLine($"Attempting to load image from: {fullPath}");

                    if (File.Exists(fullPath))
                    {
                        Console.WriteLine($"File exists: {fullPath}");
                        // Create a BitmapImage with the correct settings
                        var bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.UriSource = new Uri(fullPath, UriKind.Absolute);
                        bitmapImage.EndInit();
                        bitmapImage.Freeze(); // This helps with performance

                        // Cache the image
                        _cachedImage = bitmapImage;
                        return bitmapImage;
                    }
                    else
                    {
                        Console.WriteLine($"File not found: {fullPath}");
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading image: {ex.Message}");
                    return null;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}