using Spruce_Wood_Loggers_ERP.Persistence;
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

        private double thickness;
        private double width;

        public PieceSelectionWindow(double thickness, double width)
        {
            InitializeComponent();

            this.selectCustomNumber = false;
            this.numPieces = 0;
            this.thickness = thickness;
            this.width = width;
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

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var standardNumPieces = await CutDimensionPersistence.LoadStandardNumPieces(this.thickness, this.width);
                var standardNumPiecesStrings = standardNumPieces.Select(x => x.ToString());

                foreach (var child in Standard_Piece_Stack_Panel.Children)
                {
                    if (child is Button button)
                    {
                        var textBlock = button.Content as TextBlock;

                        if (standardNumPiecesStrings.Contains(textBlock!.Text))
                        {
                            button.Style = (Style)Application.Current.FindResource("MaterialDesignRaisedSecondaryButton");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error highlighting matching standard number of pieces: {ex.Message}\n\nApplication may need to be restarted.",
                    "Number of Pieces Highlighting Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
