using System;
using System.Windows;
using DataAccessLayer;
using DataAccessLayer.Models;
namespace UI
{
    public partial class TeachersWindow : Window
    {
        private readonly DbTeachers teachers = new DbTeachers();
        public TeachersWindow()
        {
            InitializeComponent();
            FillComboBox();
            Display();
        }
        private void Display()
        {
            teachersDataGrid.Items.Clear();
            foreach (Teacher teacher in teachers.GetTeachers())
            {
                teachersDataGrid.Items.Add(teacher);
            }
        }

        private void FillComboBox()
        {
            DbCourses courses = new DbCourses();
            foreach (Course course in courses.GetCourses())
            {
                coursesComboBox.Items.Add(course.title);
            }
        }
        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            int success = teachers.Insert(nameTextBox.Text, surnameTextBox.Text, ssnTextBox.Text, coursesComboBox.Text);
            if (success == 1)
            {
                MessageBox.Show(String.Format("{0} {1} teacher already exsits and teach {2}.", nameTextBox.Text, surnameTextBox.Text, coursesComboBox.Text));
            }
            if (success == 2)
            { 
                Display();
                MessageBox.Show(String.Format("{2} course successfully added for {0} {1}.", nameTextBox.Text, surnameTextBox.Text, coursesComboBox.Text));         
            }
            if (success == 3)
            {
                Display();
                MessageBox.Show(String.Format("{0} {1} teacher successfully added for {2}.", nameTextBox.Text, surnameTextBox.Text, coursesComboBox.Text));               
            }
            if (success == 4)
            {
                MessageBox.Show(String.Format("There is already exist teacher who has SSN {0}", ssnTextBox.Text));
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            int success= teachers.Delete(nameTextBox.Text, surnameTextBox.Text, ssnTextBox.Text, coursesComboBox.Text);
            if (success == 1)
            {
                MessageBox.Show(String.Format("{0} {1} teacher is not exist.", nameTextBox.Text, surnameTextBox.Text));
            }
            if(success==2)
            {
                MessageBox.Show(String.Format("{0} {1} teacher doesn't teach {2} course.", nameTextBox.Text, surnameTextBox.Text, coursesComboBox.Text));
            }
            if (success == 3)
            {
                Display();
                MessageBox.Show(String.Format("{2} course successfully deleted for {0} {1} teacher.", nameTextBox.Text, surnameTextBox.Text, coursesComboBox.Text));
            }
            if (success == 4)
            {
                Display();
                MessageBox.Show(String.Format("{2} course successfully deleted for {0} {1} teacher. {0} {1} teacher successfully deleted,cause {0} {1} teacher didn't have any course.", nameTextBox.Text, surnameTextBox.Text, coursesComboBox.Text));
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow adminWindow = new AdminWindow();
            Close();
            adminWindow.Show();
        }
    }
}