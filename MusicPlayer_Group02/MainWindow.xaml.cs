using MusicPlayer.BLL.Services;
using MusicPlayer.DAL.Entities;
using MusicPlayer_Group02.Components;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        public UserAccount User { get; set; }

        public MainWindow(LoginWindow loginWindow)
        {
            InitializeComponent();
            _loginWindow = loginWindow;
            ViewModel = new MainViewModel();
            DataContext = ViewModel;
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
            // Đặt nền cho AlbumsButton và thiết lập lại nền cho các nút khác nếu cần
            // ko thể thay đổi truc tiếp
            //cần chuyển đổi chuỗi màu thành một đối tượng Brush. 

            ViewModel.SelectedMenuItem = "Albums";
            AlbumsButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#02BE68"));
            HomeButton.ClearValue(System.Windows.Controls.Button.BackgroundProperty);
            //FindPlaylistListDataGrid().ItemsSource = null;
            // Đảm bảo giao diện được cập nhật trước khi thực hiện lệnh LoadPlaylistList
            Dispatcher.BeginInvoke(new Action(() =>
            {
                LoadPlaylistList(User.UserId);
            }), System.Windows.Threading.DispatcherPriority.Background);

        }



        private void LoadPlaylistList(int userId)
        {

            //FindPlaylistListDataGrid().ItemsSource = null;
            var playlistListDataGrid = FindChild<DataGrid>(MainContentControl, "PlaylistListDataGrid");

            if (playlistListDataGrid != null)
            {

                playlistListDataGrid.ItemsSource = null;
                playlistListDataGrid.ItemsSource = _playlistService.GetAllPlaylistByUser(userId);

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






        public static T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            // Nếu không có con, trả về null
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // Kiểm tra xem child có phải là loại mong muốn
                T childType = child as T;
                if (childType == null)
                {
                    // Tiếp tục tìm kiếm trong cây con
                    foundChild = FindChild<T>(child, childName);

                    // Nếu tìm thấy, trả về
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // Nếu tên khớp, trả về
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // Nếu không có tên, trả về child đầu tiên tìm thấy
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
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
    }
}