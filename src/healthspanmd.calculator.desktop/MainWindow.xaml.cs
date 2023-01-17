using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace healthspanmd.calculator.desktop
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            { //clear any error labels
                nameError.Text = "";
                emailError.Text = "";
                ageError.Text = "";
                heightError.Text = "";
                weightError.Text = "";
            }
            
            bool error = false;
            Patient patient = new Patient();

            if (nameBox.Text != "")
            {
                patient.name = nameBox.Text;
            }
            else
            {
                nameError.Text = "Don't forget your name!";
                error = true;
            }

            if (isValidEmail(emailBox.Text))
            {
                patient.email = emailBox.Text;
            }
            else
            {
                error = true;
                emailError.Text = "ERROR: Invalid Email";
            }

            int age;
            if (int.TryParse(ageBox.Text, out age))
            {
                if (age > 5 && age < 120 )
                {
                    patient.age = age;
                }
                else
                {
                    ageError.Text = "ERROR: Enter an age between 5 and 120!";
                    error = true;
                }
            }
            else
            {
                ageError.Text = "ERROR: Invalid Age";
                error = true;
            }
            int heightFt;
            if (int.TryParse(heightFtBox.Text, out heightFt))
            {
                if (heightFt > 0 && heightFt < 8)
                {
                    patient.heightFt = heightFt;
                }
                else
                {
                    heightError.Text = "ERROR: Enter a foot number between 1 and 7!";
                    error = true;
                }
            }
            else
            {
                heightError.Text = "ERROR: Invalid Height";
                error = true;
            }
            int heightIn;
            if (int.TryParse(heightInBox.Text, out heightIn))
            {
                if (heightFt >= 0 && heightFt < 13)
                {
                    patient.heightIn = heightIn;
                }
                else
                {
                    heightError.Text = "ERROR: Enter a foot number between 0 and 12!";
                    error = true;
                }
            }
            else
            {
                heightError.Text = "ERROR: Invalid Height";
                error = true;
            }
            double weight;
            if (double.TryParse(weightBox.Text, out weight))
            {
                if (weight >= 0)
                {
                    patient.weight = weight;
                }
                else
                {
                    weightError.Text = "ERROR: Enter a positive number!";
                    error = true;
                }
            }
            else
            {
                weightError.Text = "ERROR: Invalid Weight";
                error = true;
            }
            if (!error)
            {
                
            }
        }
        public static bool isValidEmail(String email)
        {
            string regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$";

            return Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
        }

        private void BMIButton_Click(object sender, RoutedEventArgs e)
        {
            int heightFt;
            int heightIn;
            double weight;
            bool error = false;
            /*
            try
            {
                heightFt = Int32.Parse(heightFt.Text)
            }
            */
        }
    }
}
