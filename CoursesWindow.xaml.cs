using System;
using System.Windows;
using System.Windows.Controls;
using DataAccessLayer;
using DataAccessLayer.Models;
namespace UI
{
    public partial class CoursesWindow : Window
    {
        private readonly DbCourses courses=new DbCourses();
        public CoursesWindow()
        {
            InitializeComponent();
            Display();
        }
        private void Display()
        {
            coursesdatagrid.Items.Clear();
            foreach (Course course in courses.GetCourses())
            {
                coursesdatagrid.Items.Add(course);
            }
        }
        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            if (!courses.Insert(titleTextBox.Text, descriptionTextBox.Text))
            {
                MessageBox.Show(String.Format("{0} course is already exist.", titleTextBox.Text));
            }
            else
            {
                Display();
                MessageBox.Show(String.Format("{0} course successfully added.", titleTextBox.Text));             
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (!courses.Delete(titleTextBox.Text))
            {
                MessageBox.Show(String.Format("{0} course isn't exist.", titleTextBox.Text));
            }
            else
            {
                Display();
                MessageBox.Show(String.Format("{0} course successfully deleted.", titleTextBox.Text));
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow adminWindow = new AdminWindow();
            Close();
            adminWindow.Show();
        }

        private void ViewTopcisButton_Click(object sender, RoutedEventArgs e)
        {
            Course course = (Course)((Button)e.Source).DataContext;
            string nameCourse = course.title;
            TopicsWindow topicsWindow = new TopicsWindow(nameCourse);
            Close();
            topicsWindow.Show();
        }
    }
}
