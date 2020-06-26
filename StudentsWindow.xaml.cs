using System;
using System.Windows;
using System.Windows.Controls;
using DataAccessLayer;
using DataAccessLayer.Models;
namespace UI
{
    public partial class StudentsWindow : Window
    {
        private readonly DbStudents students = new DbStudents();
        public StudentsWindow()
        {
            InitializeComponent();
            Display();
        }
        private void Display()
        {
            studentsDataGrid.Items.Clear();
            foreach (Student student in students.GetStudents())
            {
                studentsDataGrid.Items.Add(student);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (!students.Insert(textBoxName.Text, textBoxSurname.Text, textBoxSsn.Text))
            {
                MessageBox.Show(String.Format("{0} is already exsit.", textBoxSsn.Text));
            }
            else
            {
                Display();
                MessageBox.Show(String.Format("{0} {1} successfully added.", textBoxName.Text, textBoxSurname.Text));
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (!students.Delete(textBoxName.Text, textBoxSurname.Text, textBoxSsn.Text))
            {
                MessageBox.Show(String.Format("{0} {1} is not exsit.", textBoxName.Text, textBoxSurname.Text));
            }
            else
            {
                Display();
                MessageBox.Show(String.Format("{0} {1} successfully deleted.", textBoxName.Text, textBoxSurname.Text));
            }
        }
        

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow adminWindow = new AdminWindow();
            Close();
            adminWindow.Show();
        }

        private void ViewStudentButton_Click(object sender, RoutedEventArgs e)
        {
            Student student = (Student)((Button)e.Source).DataContext;
            string name = student.name;
            string surname = student.surname;
            string SSN = student.SSN;
            ViewStudentWindow viewStudentWindow = new ViewStudentWindow(name, surname,SSN);
            Close();
            viewStudentWindow.Show();
        }
    }
}