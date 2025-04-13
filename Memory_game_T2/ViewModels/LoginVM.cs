using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;
using Memory_game_T2.Models;
using Microsoft.Win32;
using Newtonsoft.Json;
using Memory_game_T2.Commands;
using System.Windows;

//aifsare utiliz existenti, adaugare usrr nou, selectare image,
//activare but play si delete doar cand un user e selectat
//salvare in json si incarcare din fis

namespace Memory_game_T2.ViewModels
{
    public class LoginVM : INotifyPropertyChanged
    {
        private string _username;
        private string _selectedImgPath;
        private User _selectedUser;
        private List<string> Images = new();
        private int currentImgIndex = 0;


        public ObservableCollection<User> Users { get; set; } = new();
        public string Username
        {
            get => _username;
            set 
            { 
                _username = value;
                OnPropertyChanged();
            }
        }
        public string SelectedImgPath
        {
            get => _selectedImgPath;
            set 
            { 
                _selectedImgPath = value;
                OnPropertyChanged();
            }
        }
        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanPlay));
                OnPropertyChanged(nameof(CanDelete));
            }
        }

        public ICommand AddUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand PlayCommand { get; }
        public ICommand BrowseImageCommand {  get; }
        public ICommand PrevImageCommand {  get; }
        public ICommand NextImageCommand { get; }

        public bool CanPlay => SelectedUser != null;
        public bool CanDelete => SelectedUser != null;

        //public object JsonConvert { get; private set; }

        private readonly string userFilePath = "Data/users.json";

        public LoginVM()
        {
            AddUserCommand = new RelayCommand(_ => AddUser());
            DeleteUserCommand = new RelayCommand(_=> DeleteUser(), _ => CanDelete);
            PlayCommand = new RelayCommand(_=> Play(), _ => CanPlay);
            BrowseImageCommand = new RelayCommand(_ => BrowseImage());
            PrevImageCommand = new RelayCommand(_=> PreviousImage());
            NextImageCommand=new RelayCommand(_=>NextImage());

            LoadUsers();
            LoadImages();
        }

        private void AddUser()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(SelectedImgPath))
                return;

            var newUser = new User { UserName = Username, Image = SelectedImgPath };
            Users.Add(newUser);
            SaveUsers();
            Username = "";
            SelectedImgPath = "";
        }

        private void BrowseImage()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var assetsDirectory = Path.GetFullPath(Path.Combine(currentDirectory, "../../../Assets"));

            var dialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif",
                InitialDirectory = assetsDirectory
            };

            if (dialog.ShowDialog() == true)
                SelectedImgPath = Path.GetRelativePath(currentDirectory, dialog.FileName);
        }

        private void DeleteUser()
        {
            if (SelectedUser != null)
            {
                Users.Remove(SelectedUser);
                SelectedUser = null;
                SaveUsers();
            }
        }

        private void Play() //cand dau click pe play
        {
            var menuPage = new MenuPage(); //creare instanta a MenuPage
            string category = "default"; // Replace with appropriate category
            int rows = 4; // Default rows
            int columns = 4; // Default columns

            menuPage.DataContext = new MenuVM(category, rows, columns);

            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                // mainWindow.MainFrame.Content = menuPage;
                mainWindow.LoginArea.Visibility = Visibility.Collapsed; // Ascunde zona de login
                mainWindow.MainFrame.Content = menuPage;
            }
        }
        private void LoadUsers()
        {
            if (File.Exists(userFilePath))
            {
                try
                {
                    var json = File.ReadAllText(userFilePath);
                    var list = JsonConvert.DeserializeObject<ObservableCollection<User>>(json);
                    if (list != null)
                        foreach (var u in list)
                            Users.Add(u);
                }
                catch (Exception ex)
                {
                    // Log or display the error
                    System.Diagnostics.Debug.WriteLine($"Error loading users: {ex.Message}");
                }
            }
        }

        private void LoadImages()
        {
            try
            {
                var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "../../../Assets");
                System.Diagnostics.Debug.WriteLine($"Looking for images in: {imagesFolder}");

                // Create Assets folder if it doesn't exist
                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                    System.Diagnostics.Debug.WriteLine("Assets folder created");
                }

                // Get jpg files
                var jpgFiles = Directory.GetFiles(imagesFolder, "*.jpg");
                // Get png files
                var pngFiles = Directory.GetFiles(imagesFolder, "*.png");
                // Get gif files
                var gifFiles = Directory.GetFiles(imagesFolder, "*.gif");
                // Combine all image files
                Images = jpgFiles.Concat(pngFiles).Concat(gifFiles).ToList();

                System.Diagnostics.Debug.WriteLine($"Found {Images.Count} images");

                if (Images.Count > 0)
                {
                    currentImgIndex = 0;
                    SelectedImgPath = GetRelativePath(Images[currentImgIndex]);
                    System.Diagnostics.Debug.WriteLine($"Set image path to: {SelectedImgPath}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("No images found in Assets folder");
                    SelectedImgPath = ""; // Clear selection if no images
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading images: {ex.Message}");
                SelectedImgPath = ""; // Clear selection on error
            }
        }

        private void PreviousImage()
        {
            if (Images.Count == 0) 
                return;

            currentImgIndex=(currentImgIndex-1 + Images.Count) % Images.Count;
            SelectedImgPath=GetRelativePath(Images[currentImgIndex]);
        }

        private void NextImage()
        {
            if(Images.Count == 0) 
                return;
            currentImgIndex = (currentImgIndex + 1) % Images.Count;
            SelectedImgPath = GetRelativePath(Images[currentImgIndex]);
        }
        private string GetRelativePath(string fullPath)
        {
            return Path.GetRelativePath(Directory.GetCurrentDirectory(), fullPath);
        }

        private void SaveUsers()
        {
            var directoryPath = Path.GetDirectoryName(userFilePath);
            if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            var json = JsonConvert.SerializeObject(Users, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(userFilePath, json);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
