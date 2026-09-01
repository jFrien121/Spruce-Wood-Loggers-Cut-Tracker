using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

/**
 * EntryConfirmation
 * 
 * Window that confirms the final details of a cut entry before saving
 * it to the database
 */

namespace Spruce_Wood_Loggers_ERP
{
    /// <summary>
    /// Interaction logic for EntryConfirmation.xaml
    /// </summary>
    public partial class EntryConfirmation : Window
    {

        private Grade grade;
        private double thickness;
        private double width;
        private double length;
        private int numPieces;
        private bool customPieceNumber;
        private int liftHeight;
        private int liftWidth;

        public EntryConfirmation(double thickness, double width, double length, int numPieces, int liftHeight, int liftWidth, bool customPieceNumber)
        {
            try
            {
                InitializeComponent();

                this.grade = Grade.UNGRADED;
                this.thickness = thickness;
                this.width = width;
                this.length = length;
                this.numPieces = numPieces;
                this.liftHeight = liftHeight;
                this.liftWidth = liftWidth;
                this.customPieceNumber = customPieceNumber;

                SetEntryText();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing entry confirmation screen: {ex.Message}\n\nApplication may need to be restarted.",
                    "Window Initialization Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void SetEntryText()
        {
            try
            {
                Entry_Description.Text = this.thickness + "\" x " + this.width + "\" x "
                    + this.length + "' x " + this.numPieces + " pieces ("
                    + GradeToString() + ")\n";

                if (this.customPieceNumber)
                {
                    Entry_Description.Inlines.Add(new Run("Lift Height x Width: " + this.liftHeight
                        + " x " + this.liftWidth)
                    { FontStyle = FontStyles.Italic });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting entry confirmation text: {ex.Message}\n\nApplication may need to be restarted.",
                    "Entry Text Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private string GradeToString()
        {
            try
            {
                switch (this.grade)
                {
                    case Grade.UNGRADED: return "Ungraded";
                    case Grade.ONE: return "#1";
                    case Grade.TWO: return "#2";
                    case Grade.THREE: return "#3";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error converting grade to string: {ex.Message}\n\nApplication may need to be restarted.",
                    "Grade Conversion Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            return "Ungraded";
        }

        // Cancel the entry
        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.DialogResult = false;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error closing Entry Confirmation Window: {ex.Message}\n\nApplication may need to be restarted.",
                    "Window Closing Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // Save the entry to the database
        private void Confirm_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Batch currBatch = new Batch(DateTime.Now, this.thickness,
                    this.width, this.length, GradeToString(), this.numPieces);
                BatchPersistence.SaveBatch(currBatch);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while confirming/saving the entry: {ex.Message}\n\nApplication may need to be restarted.",
                    "Entry Confirmation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void Ungraded_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Ungraded_Button.Style = (Style)Application.Current.FindResource("MaterialDesignRaisedSecondaryButton");
                Grade1_Button.Style = (Style)Application.Current.FindResource("MaterialDesignRaisedButton");
                Grade2_Button.Style = (Style)Application.Current.FindResource("MaterialDesignRaisedButton");
                Grade3_Button.Style = (Style)Application.Current.FindResource("MaterialDesignRaisedButton");
                this.grade = Grade.UNGRADED;
                SetEntryText();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating grade: {ex.Message}\n\nApplication may need to be restarted.",
                    "Updating Grade Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void Grade1_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Grade1_Button.Style = (Style)Application.Current.FindResource("MaterialDesignRaisedSecondaryButton");
                Ungraded_Button.Style = (Style)Application.Current.FindResource("MaterialDesignRaisedButton");
                Grade2_Button.Style = (Style)Application.Current.FindResource("MaterialDesignRaisedButton");
                Grade3_Button.Style = (Style)Application.Current.FindResource("MaterialDesignRaisedButton");
                this.grade = Grade.ONE;
                SetEntryText();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating grade: {ex.Message}\n\nApplication may need to be restarted.",
                    "Updating Grade Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void Grade2_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Grade2_Button.Style = (Style)Application.Current.FindResource("MaterialDesignRaisedSecondaryButton");
                Ungraded_Button.Style = (Style)Application.Current.FindResource("MaterialDesignRaisedButton");
                Grade1_Button.Style = (Style)Application.Current.FindResource("MaterialDesignRaisedButton");
                Grade3_Button.Style = (Style)Application.Current.FindResource("MaterialDesignRaisedButton");
                this.grade = Grade.TWO;
                SetEntryText();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating grade: {ex.Message}\n\nApplication may need to be restarted.",
                    "Updating Grade Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
}

        private void Grade3_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Grade3_Button.Style = (Style)Application.Current.FindResource("MaterialDesignRaisedSecondaryButton");
                Ungraded_Button.Style = (Style)Application.Current.FindResource("MaterialDesignRaisedButton");
                Grade1_Button.Style = (Style)Application.Current.FindResource("MaterialDesignRaisedButton");
                Grade2_Button.Style = (Style)Application.Current.FindResource("MaterialDesignRaisedButton");
                this.grade = Grade.THREE;
                SetEntryText();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating grade: {ex.Message}\n\nApplication may need to be restarted.",
                    "Updating Grade Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
