using MusicPlayer.BLL.Services;
using MusicPlayer.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MusicPlayer_Group02
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void CheckBoxHide_Checked(object sender, RoutedEventArgs e)
        {
            PasswordShowBox.Text = PasswordHiddenBox.Password;
            PasswordShowBox.Visibility = Visibility.Visible;
            PasswordHiddenBox.Visibility = Visibility.Collapsed;
        }

        private void CheckBoxHide_Unchecked(object sender, RoutedEventArgs e)
        {
            PasswordHiddenBox.Password = PasswordShowBox.Text;
            PasswordShowBox.Visibility = Visibility.Collapsed;
            PasswordHiddenBox.Visibility = Visibility.Visible;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            //If user login but email or password or both is empty
            //validation
            if (string.IsNullOrEmpty(EmailTextBox.Text) || string.IsNullOrEmpty(PasswordHiddenBox.Password))
            {
                MessageBox.Show("Email or Password is empty. Please fill in all fields.", "Empty fields", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            UserService service = new();
            UserAccount? acc = service.CheckLogin(EmailTextBox.Text, PasswordHiddenBox.Password);

            if (acc == null)
            { 
                MessageBox.Show("Login failed. Check email and password again.", "Wrong credentials", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MainWindow main = new(this);
            main.User = acc;
            main.Show();
            this.Hide();
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
