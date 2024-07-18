using MahApps.Metro.IconPacks;
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

        private bool isPlaying = false;
        public MainWindow(LoginWindow loginWindow)
        {
            InitializeComponent();
            _loginWindow = loginWindow;
            ViewModel = new MainViewModel();
            DataContext = ViewModel;
        }

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
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedMenuItem = "Albums";
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
                    // Thực hiện các thao tác với MediaElement
                    mediaElement.Source = new Uri(selectedFile);
                    mediaElement.Play();
                    isPlaying = true;
                }
                else
                {
                    System.Windows.MessageBox.Show("Cannot play the file", "Error", MessageBoxButton.OK);
                }
            }
        }

        //Bắt sk Khi MidiaElement được mở lên
        private void MediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            var mediaElement = sender as MediaElement;
            var defaultImage = FindChild<Image>(MainContentControl, "DefaultImage");

            if (mediaElement != null && defaultImage != null)
            {
                // Kiểm tra nếu MediaElement có video
                if (mediaElement.NaturalVideoHeight == 0 && mediaElement.NaturalVideoWidth == 0)
                {
                    // Nếu không có video, hiển thị ảnh và ẩn MediaElement
                    defaultImage.Visibility = Visibility.Visible;
                    mediaElement.Visibility = Visibility.Collapsed;
                }
                else
                {
                    // Nếu có video, hiển thị MediaElement và ẩn ảnh
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
                    // Nếu đang phát, thì dừng lại và chuyển nút sang trạng thái Play
                    mediaElement.Pause();
                    PauseButton.Content = new PackIconMaterial() { Kind = PackIconMaterialKind.Play, Style = (Style)FindResource("PlayerButtonIcon") };
                    isPlaying = false;
                }
                else
                {
                    // Nếu đang dừng, thì phát lại và chuyển nút sang trạng thái Pause
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

        // Hàm tìm kiếm trong cây Visual
        private static T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
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
    }
}