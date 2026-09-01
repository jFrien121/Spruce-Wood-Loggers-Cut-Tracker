using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using Spruce_Wood_Loggers_ERP.Database_Objects;
using Spruce_Wood_Loggers_ERP.Persistence;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.Reflection.Metadata;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static MaterialDesignThemes.Wpf.Theme;

/**
 * Main grid window for the Cut Tracker application, holding a grid of buttons 
 * for each dimension combination of thickness, width, and length.
 */

namespace Spruce_Wood_Loggers_ERP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Spruce_Wood_Loggers_Cut_Tracker : Window
    {

        public Spruce_Wood_Loggers_Cut_Tracker()
        {
            try
            {
                InitializeComponent();
                WindowState = WindowState.Maximized;
                ResizeMode = ResizeMode.NoResize;
                Topmost = true;

                // Ensure database is created
                PersistenceSetUp.ConnectToDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing application: {ex.Message}\n\nApplication may need to be restarted.", 
                    "Initialization Error", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
            }
        }

        // Dynamically create a grid with headers, and rows and
        // columns of buttons for each dimension combination
        private void InitGrid(List<double> cutLengths, List<CutSize> cutSizes)
        {
            try
            {
                for (int i = 0; i < cutSizes.Count; i++)
                {
                    ColumnDefinition newCol = new ColumnDefinition();
                    MainGrid.ColumnDefinitions.Add(newCol);

                    // Set up each column header
                    TextBlock header = new TextBlock
                    {
                        Text = $"{cutSizes.ElementAt(i).thickness}\" x {cutSizes.ElementAt(i).width}\"",
                        FontSize = 10,
                        TextAlignment = TextAlignment.Center,
                        Foreground = Brushes.White
                    };

                    header.Margin = new Thickness(0, 5, 0, 2);

                    Grid.SetRow(header, 0);
                    Grid.SetColumn(header, i + 1);

                    MainGrid.Children.Add(header);
                }

                for (int i = 0; i < cutLengths.Count; i++)
                {
                    RowDefinition newRow = new RowDefinition();
                    MainGrid.RowDefinitions.Add(newRow);

                    // Set up each row header
                    TextBlock header = new TextBlock
                    {
                        Text = $"{cutLengths[i]}'",
                        FontSize = 10,
                        VerticalAlignment = VerticalAlignment.Center,
                        TextAlignment = TextAlignment.Right,
                        Foreground = Brushes.White
                    };

                    header.Margin = new Thickness(4, 0, 3, 0);

                    Grid.SetRow(header, i + 1);
                    Grid.SetColumn(header, 0);

                    MainGrid.Children.Add(header);
                }

                bool isLightTheme = true;

                // Initialize grid of buttons
                for (int i = 0; i < cutLengths.Count; i++)
                {
                    for (int j = 0; j < cutSizes.Count; j++)
                    {
                        double thickness = cutSizes.ElementAt(j).thickness;
                        double width = cutSizes.ElementAt(j).width;
                        double length = cutLengths[i];

                        // Set button settings
                        var button = new DimensionButton
                        {
                            Margin = new Thickness(3),
                            IsEnabled = true, // This will be replaced by the binding below
                            Padding = new Thickness(2),
                            CutThickness = thickness,
                            CutWidth = width,
                            CutLength = length
                        };

                        // Set in place in grid
                        Grid.SetRow(button, i + 1);
                        Grid.SetColumn(button, j + 1);
                        button.Click += GridButton_Click; // Set up event handler

                        // Set style
                        ButtonAssist.SetCornerRadius(button, new CornerRadius(5));

                        // Alternate button colours for each thickness
                        if (j != 0 && cutSizes.ElementAt(j).thickness != cutSizes.ElementAt(j - 1).thickness)
                        {
                            isLightTheme = !isLightTheme;
                        }

                        if (isLightTheme)
                        {
                            button.Style = (Style)Application.Current.FindResource("MaterialDesignRaisedLightButton");
                        }
                        else
                        {
                            button.Style = (Style)Application.Current.FindResource("MaterialDesignRaisedButton");
                        }

                        // Set up text
                        button.Content = new TextBlock
                        {
                            Text = $"{thickness}\" x {width}\"\n x {length}'",
                            FontSize = 8,
                            TextWrapping = TextWrapping.Wrap,
                            TextAlignment = TextAlignment.Center

                        };

                        MainGrid.Children.Add(button);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing grid: {ex.Message}\n\nApplication may need to be restarted.",
                    "Grid Initialization Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // Initialize Entry Confirmation screen based on button that is clicked
        private async void GridButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as DimensionButton;

                int numPieces = 0;
                int cutThickness = (int)button!.CutThickness;
                int cutWidth = (int)button.CutWidth;
                int cutLength = (int)button.CutLength;
                int liftHeight = 0;
                int liftWidth = 0;

                var pieceSelection = new PieceSelectionWindow()
                {
                    Owner = this
                };

                pieceSelection.ShowDialog();

                var customHeightWindow = new CustomHeightWindow()
                {
                    Owner = this
                };
                var customWidthWindow = new CustomWidthWindow()
                {
                    Owner = this
                };

                // If a custom number of pieces was selected, open the custom height and width windows
                if (pieceSelection.getSelectCustomNumber())
                {
                    customHeightWindow.ShowDialog();

                    if (customHeightWindow.DialogResult != false)
                    {
                        liftHeight = customHeightWindow.getSelectedHeight();

                        customWidthWindow.ShowDialog();

                        if (customWidthWindow.DialogResult != false)
                        {
                            liftWidth = customWidthWindow.getSelectedWidth();
                            numPieces = liftHeight * liftWidth;
                        }
                    }
                }
                else
                {
                    numPieces = pieceSelection.getNumPieces();
                }

                if ((pieceSelection.DialogResult == true && !pieceSelection.getSelectCustomNumber()) || customWidthWindow.DialogResult == true)
                {
                    var entryConfirmationWindow = new EntryConfirmation(cutThickness, cutWidth, cutLength, numPieces, liftHeight, liftWidth, pieceSelection.getSelectCustomNumber())
                    {
                        Owner = this
                    };

                    entryConfirmationWindow.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing batch: {ex.Message}\n\nApplication may need to be restarted.",
                    "Batch Processing Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // Print a daily report
        private void Print_Button_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();

            bool? result = printDialog.ShowDialog();

            if (result == true)
            {
                FlowDocument doc = new FlowDocument();
                doc.Blocks.Add(new System.Windows.Documents.Paragraph(new Run("Hello World")));

                printDialog.PrintDocument(((IDocumentPaginatorSource)doc).DocumentPaginator, "Printing FlowDocument");
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var cutLengths = await CutLengthPersistence.LoadCutLengths();
                var cutSizes = await CutSizePersistence.LoadCutSizes();
                InitGrid(cutLengths, cutSizes);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading the window: {ex.Message}\n\nApplication may need to be restarted.",
                    "Window Loading Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}