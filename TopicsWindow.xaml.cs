using System;
using System.Windows;
using DataAccessLayer;
using DataAccessLayer.Models;
namespace UI
{
    public partial class TopicsWindow : Window
    {
        private readonly DbTopics topics = new DbTopics();
        string courseName;
        public TopicsWindow(string str)
        {
            InitializeComponent();
            courseName = str;
            courseNameLabel.Content = str;
            Display();
        }
        private void Display()
        {
            topicsDataGrid.Items.Clear();
            foreach (Topic topic in topics.GetTopics(courseName))
            {
                topicsDataGrid.Items.Add(topic);
            }
        }
        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            if (!topics.Insert(courseName, topicTextBox.Text))
            {
                MessageBox.Show(String.Format("{0} topic already exists in {1} course.", topicTextBox.Text, courseName));
            }
            else
            {
                Display();
                MessageBox.Show(String.Format("{0} topic successfully added for {1} course", topicTextBox.Text, courseName));
            }            
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (!topics.Delete(courseName, topicTextBox.Text))
            {
                MessageBox.Show(String.Format("{0} topic doesn't exsit in {1} course.", topicTextBox.Text, courseName));
            }
            else
            {
                MessageBox.Show(String.Format("{0} topic successfully deleted for {1} course.", topicTextBox.Text, courseName));
                Display();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            CoursesWindow coursesWindow = new CoursesWindow();
            Close();
            coursesWindow.Show();
        }
    }
}
