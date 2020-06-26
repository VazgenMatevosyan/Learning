using System.Windows;
namespace UI
{
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
        }

        private void CoursesButton_Click(object sender, RoutedEventArgs e)
        {
            CoursesWindow coursesWindow = new CoursesWindow();
            Close();
            coursesWindow.Show();
        }

        private void StudentsButton_Click(object sender, RoutedEventArgs e)
        {
            StudentsWindow studentsWindow = new StudentsWindow();
            Close();
            studentsWindow.Show();
        }

        private void TeachersButton_Click(object sender, RoutedEventArgs e)
        {
            TeachersWindow teachersWindow = new TeachersWindow();
            Close();
            teachersWindow.Show();
        }
    }
}
