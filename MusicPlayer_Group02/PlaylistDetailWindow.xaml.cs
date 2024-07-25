using Microsoft.VisualBasic.ApplicationServices;
using MusicPlayer.BLL.Services;
using MusicPlayer.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MusicPlayer_Group02
{
    /// <summary>
    /// Interaction logic for PlaylistDetailWindow.xaml
    /// </summary>
    public partial class PlaylistDetailWindow : Window
    {
        public PlaylistDetailWindow()
        {
            InitializeComponent();
        }
        private PlaylistService _playlistService = new();
        public UserAccount User { get; set; }
        public Playlist EditedSong { get; set; }
        private void ChooseFileButton_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "Music files (*.mp3;*.wav)|*.mp3;*.wav|All files (*.*)|*.*";
            openFileDialog.Filter = "Media Files|*.mp3;*.mp4";
            var result = openFileDialog.ShowDialog();

            // Kiểm tra xem người dùng đã chọn file hay không
            // Người dùng đã chọn một file, xử lý dữ liệu tại đây
            string selectedFileName = openFileDialog.FileName;
            //System.Windows.MessageBox.Show("Đường dẫn file: " + selectedFileName, "ahha", MessageBoxButton.OK, MessageBoxImage.Error);
            ChooseFileTextBox.Text = selectedFileName;

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Playlist song = new Playlist() { };
                song.Url = ChooseFileTextBox.Text;
               
                song.UserId = User.UserId;
                
                song.SongName = SongNameTextBox.Text;
                if (EditedSong ==  null)
                {

                    _playlistService.AddSong(song);
                }
                else
                {
                    _playlistService.UpdateSong(song, EditedSong.PlaylistId);
                }

                System.Windows.MessageBox.Show(DetailLabel.Content + " successfully!", DetailLabel.Content + "", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
                
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(DetailLabel.Content + " failed!", DetailLabel.Content + "", MessageBoxButton.OK, MessageBoxImage.Error);
                
            }


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (EditedSong != null)
            {
                DetailLabel.Content = "Edit song";
                SongNameTextBox.Text = EditedSong.SongName;
                ChooseFileTextBox.Text = EditedSong?.Url;
            }
                
            else 
                DetailLabel.Content = "Add new song";

        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


    }
}
