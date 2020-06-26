using System;
using System.Text;
using System.Windows;
using System.Data;
using DataAccessLayer;
using DataAccessLayer.Models;

namespace UI
{
    public partial class ViewStudentWindow : Window
    {
        DbViewStudent viewStudent = new DbViewStudent();
        string nameStudent;
        string surnameStudent;
        string ssnStudent;
        public ViewStudentWindow(string name, string surname,string SSN)
        {
            InitializeComponent();
            nameStudent = name;
            surnameStudent = surname;
            ssnStudent = SSN;
            StringBuilder buildStudent = new StringBuilder();
            buildStudent.Append(name);
            buildStudent.Append(" ");
            buildStudent.Append(surname);
            studentLabel.Content = buildStudent;
            Display();
        }

        private void Display()
        {
            coursesStudentDataGrid.Items.Clear();
            foreach (StudentGrades studentGrades in viewStudent.ShowStudentGrades(ssnStudent))
            {
                coursesStudentDataGrid.Items.Add(studentGrades);
            }
            FillComboBox();            
        }
        private void FillComboBox()
        {
            availableCoursesComboBox.Items.Clear();
            foreach (Course course in viewStudent.FillComboBox(ssnStudent))
            {
                availableCoursesComboBox.Items.Add(course.title);
            }
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            StudentsWindow studentsWindow = new StudentsWindow();
            Close();
            studentsWindow.Show();
        }

        private void EnrolButton_Click(object sender, RoutedEventArgs e)
        {
            int success=viewStudent.Enrol(ssnStudent, titleCourseTextBox.Text);
            if (success == 1)
            {
                MessageBox.Show(String.Format("{0} course doesn't exist.", titleCourseTextBox.Text));
            }
            if (success == 2)
            {
                MessageBox.Show(String.Format("{0} {1} student already enrol {2} course",nameStudent,surnameStudent, titleCourseTextBox.Text));
            }
            if (success == 3)
            {
                Display();
                MessageBox.Show(String.Format("{0} {1} student successfully enroled for {2} course", nameStudent, surnameStudent, titleCourseTextBox.Text));
            }
        }

        private void UnenrolButton_Click(object sender, RoutedEventArgs e)
        {
            int success=viewStudent.Unenrol(ssnStudent, titleCourseTextBox.Text);
            if (success == 1)
            {
                MessageBox.Show(String.Format("{0} course doesn't exist.", titleCourseTextBox.Text));
            }
            if (success == 2)
            {
                MessageBox.Show(String.Format("{0} {1} student doesn't enrol {2} course", nameStudent, surnameStudent, titleCourseTextBox.Text));
            }
            if (success == 3)
            {
                Display();
                MessageBox.Show(String.Format("{0} {1} student successfully unenroled for {2} course", nameStudent, surnameStudent, titleCourseTextBox.Text));
            }
        }
    }
}