using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Spruce_Wood_Loggers_ERP
{
    /// <summary>
    /// Interaction logic for PieceSelectionWindow.xaml
    /// </summary>
    public partial class PieceSelectionWindow : Window
    {

        private int numPieces;
        private bool selectCustomNumber;

        public PieceSelectionWindow()
        {
            InitializeComponent();

            this.selectCustomNumber = false;
            this.numPieces = 0;
        }

        private void NumberPiecesClose_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DialogResult = false;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error closing Number of Pieces Window: {ex.Message}\n\nApplication may need to be restarted.",
                    "Window Closing Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void SelectCustomNumber_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.selectCustomNumber = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error closing Custom Height Window: {ex.Message}\n\nApplication may need to be restarted.",
                    "Window Closing Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void PieceNumberButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                var textBlock = button!.Content as TextBlock;

                this.numPieces = int.Parse(textBlock!.Text);
                DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error confirming standard number of pieces: {ex.Message}\n\nApplication may need to be restarted.",
                    "Number of Pieces Confirmation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public int getNumPieces()
        {
            return numPieces;
        }

        public bool getSelectCustomNumber()
        {
            return selectCustomNumber;
        }
    }
}
