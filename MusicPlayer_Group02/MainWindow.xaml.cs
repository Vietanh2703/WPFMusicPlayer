using MusicPlayer.BLL.Services;
using MusicPlayer.DAL.Entities;
using MahApps.Metro.IconPacks;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Forms;
using MusicPlayer_Group02.Components;
using System.IO;

namespace MusicPlayer_Group02
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LoginWindow _loginWindow;
        private MainViewModel ViewModel { get; set; }
        private PlaylistService _playlistService = new();
        private bool isPlaying = false;
        private DispatcherTimer timer;
        private List<Playlist> _playlistList;
        private int _currentSongIndex = -1;

        public UserAccount User { get; set; }

        public MainWindow(LoginWindow loginWindow)
        {
            InitializeComponent();
            InitializeTimer();
            _loginWindow = loginWindow;
            ViewModel = new MainViewModel();
            DataContext = ViewModel;
            var mediaElement = FindChild<MediaElement>(MainContentControl, "MediaElement");
            if (mediaElement != null)
            {
                mediaElement.MediaEnded += MediaElement_MediaEnded;
            }
        }

        public MainWindow() => InitializeComponent();

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _loginWindow.Show();
            this.Close();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedMenuItem = "Home";
            HomeButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#02BE68"));
            AlbumsButton.ClearValue(System.Windows.Controls.Button.BackgroundProperty);
        }

        private void AlbumsButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedMenuItem = "Albums";
            AlbumsButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#02BE68"));
            HomeButton.ClearValue(System.Windows.Controls.Button.BackgroundProperty);

            Dispatcher.BeginInvoke(new Action(() =>
            {
                LoadPlaylistList(User.UserId);
            }), System.Windows.Threading.DispatcherPriority.Background);
        }

        private void LoadPlaylistList(int userId)
        {
            var playlistListDataGrid = FindChild<DataGrid>(MainContentControl, "PlaylistListDataGrid");

            if (playlistListDataGrid != null)
            {
                _playlistList = _playlistService.GetAllPlaylistByUser(userId);
                playlistListDataGrid.ItemsSource = _playlistList;
            }
            else
            {
                System.Windows.MessageBox.Show("Cannot found", "Error", MessageBoxButton.OK);
            }
        }

        private void AddSongButton_Click(object sender, RoutedEventArgs e)
        {
            PlaylistDetailWindow detail = new();
            detail.User = User;
            detail.ShowDialog();
            LoadPlaylistList(User.UserId);
        }

        private void EditSongButton_Click(object sender, RoutedEventArgs e)
        {
            Playlist selected = FindChild<DataGrid>(MainContentControl, "PlaylistListDataGrid").SelectedItem as Playlist;
            if (selected == null)
            {
                System.Windows.MessageBox.Show("Please select a song to edit!", "Edit song", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            PlaylistDetailWindow detail = new();
            detail.EditedSong = selected;
            detail.User = User;
            detail.ShowDialog();
            LoadPlaylistList(User.UserId);
        }

        private void DeleteSongButton_Click(object sender, RoutedEventArgs e)
        {
            Playlist selected = FindChild<DataGrid>(MainContentControl, "PlaylistListDataGrid").SelectedItem as Playlist;
            if (selected == null)
            {
                System.Windows.MessageBox.Show("Please select a song to delete!", "Delete song", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBoxResult answer = System.Windows.MessageBox.Show("Do you really want to delete the selected song?", "Delete confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.No)
                return;

            _playlistService.DeleteSong(selected, selected.PlaylistId);
            LoadPlaylistList(User.UserId);
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var mediaElement = FindChild<MediaElement>(MainContentControl, "MediaElement");
            var timeTextBlock = FindChild<TextBlock>(MainContentControl, "TimeTextBlock");
            if (mediaElement != null && timeTextBlock != null)
            {
                if (mediaElement.Source != null && mediaElement.NaturalDuration.HasTimeSpan)
                {
                    timeTextBlock.Text = $"{mediaElement.Position.ToString(@"mm\:ss")} / {mediaElement.NaturalDuration.TimeSpan.ToString(@"mm\:ss")}";
                }
            }
        }

        private void OpenSongButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "Media Files|*.mp3;*.mp4";
            openFileDialog.ShowDialog();

            string selectedFile = openFileDialog.FileName;

            if (!string.IsNullOrEmpty(selectedFile))
            {
                var mediaElement = FindChild<MediaElement>(MainContentControl, "MediaElement");
                if (mediaElement != null)
                {
                    mediaElement.Source = new Uri(selectedFile);
                    mediaElement.Play();

                    PauseButton.Content = new PackIconMaterial() { Kind = PackIconMaterialKind.Pause, Style = (Style)FindResource("PlayerButtonIcon") };
                    isPlaying = true;
                    string songName = Path.GetFileNameWithoutExtension(selectedFile);
                    playName.Text = songName;
                    timer.Start();
                }
                else
                {
                    System.Windows.MessageBox.Show("Cannot play the file", "Error", MessageBoxButton.OK);
                }
            }
        }

        private void MediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            var mediaElement = sender as MediaElement;
            var defaultImage = FindChild<Image>(MainContentControl, "DefaultImage");

            if (mediaElement != null && defaultImage != null)
            {
                if (mediaElement.NaturalVideoHeight == 0 && mediaElement.NaturalVideoWidth == 0)
                {
                    defaultImage.Visibility = Visibility.Visible;
                    mediaElement.Visibility = Visibility.Collapsed;
                }
                else
                {
                    defaultImage.Visibility = Visibility.Collapsed;
                    mediaElement.Visibility = Visibility.Visible;
                }
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            var mediaElement = FindChild<MediaElement>(MainContentControl, "MediaElement");
            if (mediaElement != null)
            {
                if (isPlaying)
                {
                    mediaElement.Pause();
                    PauseButton.Content = new PackIconMaterial() { Kind = PackIconMaterialKind.Play, Style = (Style)FindResource("PlayerButtonIcon") };
                    isPlaying = false;
                }
                else
                {
                    mediaElement.Play();
                    PauseButton.Content = new PackIconMaterial() { Kind = PackIconMaterialKind.Pause, Style = (Style)FindResource("PlayerButtonIcon") };
                    isPlaying = true;
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Cannot pause the file", "Error", MessageBoxButton.OK);
            }
        }

        private static T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                T childType = child as T;
                if (childType == null)
                {
                    foundChild = FindChild<T>(child, childName);
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (NumberVolumeTextBlock != null && slider.Value > 0)
            {
                var mediaElement = FindChild<MediaElement>(MainContentControl, "MediaElement");
                if (mediaElement != null)
                {
                    mediaElement.Volume = (slider.Value / 100);
                }
                NumberVolumeTextBlock.Text = ((int)slider.Value).ToString();
            }
        }

        private void PlaylistListDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedPlaylist = (sender as DataGrid).SelectedItem as Playlist;
            if (selectedPlaylist != null)
            {
                _currentSongIndex = _playlistList.IndexOf(selectedPlaylist);
                PlaySongAtIndex(_currentSongIndex);
            }
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentSongIndex > 0)
            {
                _currentSongIndex--;
                PlaySongAtIndex(_currentSongIndex);
            }
            else
            {
                System.Windows.MessageBox.Show("This is the first song.", "Information", MessageBoxButton.OK);
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentSongIndex < _playlistList.Count - 1)
            {
                _currentSongIndex++;
                PlaySongAtIndex(_currentSongIndex);
            }
            else
            {
                System.Windows.MessageBox.Show("This is the last song.", "Information", MessageBoxButton.OK);
            }
        }

        private void PlaySongAtIndex(int index)
        {
            if (index >= 0 && index < _playlistList.Count)
            {
                var song = _playlistList[index];
                var mediaElement = FindChild<MediaElement>(MainContentControl, "MediaElement");

                if (mediaElement != null)
                {
                    try
                    {
                        var uri = new Uri(song.Url, UriKind.RelativeOrAbsolute);
                        mediaElement.Source = uri;
                        mediaElement.Play();

                        playName.Text = song.SongName;

                        PauseButton.Content = new PackIconMaterial() { Kind = PackIconMaterialKind.Pause, Style = (Style)FindResource("PlayerButtonIcon") };
                        isPlaying = true;
                        timer.Start();
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show($"Error playing file: {ex.Message}", "Error", MessageBoxButton.OK);
                    }
                }
            }
        }


        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (_currentSongIndex < _playlistList.Count - 1)
            {
                _currentSongIndex++;
                PlaySongAtIndex(_currentSongIndex);
            }
            else
            {
                var mediaElement = FindChild<MediaElement>(MainContentControl, "MediaElement");
                if (mediaElement != null)
                {
                    mediaElement.Stop();
                    timer.Stop();
                    isPlaying = false;
                    PauseButton.Content = new PackIconMaterial() { Kind = PackIconMaterialKind.Play, Style = (Style)FindResource("PlayerButtonIcon") };
                }
            }
        }




    }
}
