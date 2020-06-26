using System.Windows;
using System.Data.SqlClient;
using DataAccessLayer;
namespace UI
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            DbInfo dbInfo = new DbInfo();
            dbInfo.LoginPassword(usernameTextBox.Text, passwordTextBox.Password);
            try
            {
                dbInfo.ForLogin();
                AdminWindow adminWindow = new AdminWindow();
                Close();
                adminWindow.Show();
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
