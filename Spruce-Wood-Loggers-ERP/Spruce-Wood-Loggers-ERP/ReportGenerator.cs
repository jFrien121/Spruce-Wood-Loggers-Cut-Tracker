using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using Spruce_Wood_Loggers_ERP.Database_Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

/**
 * ReportGenerator
 * Creates a Report using MigraDoc, which summerizes the number of lifts per 
 * dimension, and the total FBM
 */

namespace Spruce_Wood_Loggers_ERP
{
    internal class ReportGenerator
    {
        public static bool CreatePdf(string filePath, List<LiftResult> numberOfLiftResults, double fbm)
        {

            bool success = true;

            try
            {
                Document document = new Document();
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

                // Page setup ----------------------------------------------

                Section section = document.AddSection();

                section.PageSetup.PageFormat = PageFormat.Letter;

                section.PageSetup.TopMargin = Unit.FromInch(0.6);
                section.PageSetup.BottomMargin = Unit.FromInch(0.6);
                section.PageSetup.LeftMargin = Unit.FromInch(0.6);
                section.PageSetup.RightMargin = Unit.FromInch(0.6);

                // Styles --------------------------------------------------

                MigraDoc.DocumentObjectModel.Style normalStyle = document.Styles["Normal"]!;

                normalStyle.Font.Name = "Arial";
                normalStyle.Font.Size = 10;

                MigraDoc.DocumentObjectModel.Style titleStyle = document.Styles.AddStyle(
                    "ReportTitle",
                    "Normal");

                titleStyle.Font.Name = "Arial";
                titleStyle.Font.Size = 20;
                titleStyle.Font.Bold = true;
                titleStyle.Font.Color = Colors.DarkSlateGray;

                // Header --------------------------------------------------

                Paragraph header = section.Headers.Primary.AddParagraph();

                header.Format.Alignment = ParagraphAlignment.Right;
                header.Format.Font.Size = 12;
                header.Format.Font.Color = Colors.Gray;

                header.AddText("Spruce Wood Loggers");
                section.PageSetup.TopMargin = Unit.FromInch(1);

                // Title ---------------------------------------------------

                Paragraph title = section.AddParagraph();
                title.Style = "ReportTitle";

                title.AddText("Sawmill Production Report - " + DateTime.Now.ToString("MMM") + ". "
                                + DateTime.Now.ToString("dd") + ", " + DateTime.Now.ToString("yyyy"));

                title.Format.SpaceAfter = Unit.FromPoint(25);
                title.Format.Alignment = ParagraphAlignment.Center;

                // Table ---------------------------------------------------

                Table table = section.AddTable();

                // Add margins to center the table
                double pageWidth = 8.5;
                double leftMargin = 0.6;
                double rightMargin = 0.6;

                double tableWidth = 5.95;

                double availableWidth =
                    pageWidth - leftMargin - rightMargin;

                double leftIndent =
                    (availableWidth - tableWidth) / 2.0;

                table.Rows.LeftIndent =
                    Unit.FromInch(leftIndent);

                table.Borders.Width = 0.5;
                table.Borders.Color = Colors.LightGray;

                // Column widths

                Column numberColumn = table.AddColumn(
                    Unit.FromInch(0.55));

                Column lengthColumn = table.AddColumn(
                    Unit.FromInch(1.35));

                Column widthColumn = table.AddColumn(
                    Unit.FromInch(1.35));

                Column thicknessColumn = table.AddColumn(
                    Unit.FromInch(1.35));

                Column valueColumn = table.AddColumn(
                    Unit.FromInch(1.35));

                // Header row ----------------------------------------------

                Row headerRow = table.AddRow();

                headerRow.HeadingFormat = true;

                headerRow.Height = Unit.FromInch(0.4);

                headerRow.VerticalAlignment =
                    MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;

                headerRow.Shading.Color =
                    Colors.DarkSlateGray;

                AddCellText(
                    headerRow.Cells[0],
                    " ",
                    true);

                AddCellText(
                    headerRow.Cells[1],
                    "Length",
                    true);

                AddCellText(
                    headerRow.Cells[2],
                    "Thickness",
                    true);

                AddCellText(
                    headerRow.Cells[3],
                    "Width",
                    true);

                AddCellText(
                    headerRow.Cells[4],
                    "Number of Lifts",
                    true);

                // Data rows -----------------------------------------------

                for (int i = 0; i < numberOfLiftResults.Count; i++)
                {
                    LiftResult item = numberOfLiftResults[i];

                    Row row = table.AddRow();

                    row.Height = Unit.FromInch(0.35);

                    row.VerticalAlignment =
                        MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;

                    // Alternating row background
                    if (i % 2 == 0)
                    {
                        row.Shading.Color =
                            Colors.WhiteSmoke;
                    }

                    AddCellText(
                        row.Cells[0],
                        (i + 1).ToString());

                    AddCellText(
                        row.Cells[1],
                        FormatDimension(item.length, true));

                    AddCellText(
                        row.Cells[2],
                        FormatDimension(item.thickness, false));

                    AddCellText(
                        row.Cells[3],
                        FormatDimension(item.width, false));

                    AddCellText(
                        row.Cells[4],
                        item.numberOfLifts.ToString());
                }

                // Result / summary value ----------------------------------

                // Space between main table and result
                Paragraph spacing = section.AddParagraph();
                spacing.Format.SpaceAfter = Unit.FromPoint(8);

                // Create result table
                Table resultTable = section.AddTable();

                resultTable.Borders.Width = 0.5;
                resultTable.Borders.Color = Colors.LightGray;

                // Two equal columns
                resultTable.AddColumn(Unit.FromInch(2.975));
                resultTable.AddColumn(Unit.FromInch(2.975));

                // Center the table
                resultTable.Rows.LeftIndent = Unit.FromInch(0.675);

                // Create row
                Row resultRow = resultTable.AddRow();

                resultRow.Height = Unit.FromInch(0.45);
                resultRow.VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;

                // Name cell -------------------------------------------

                Cell nameCell = resultRow.Cells[0];
                nameCell.Shading.Color = Colors.DarkSlateGray;

                Paragraph nameParagraph =
                    nameCell.AddParagraph();

                nameParagraph.Format.Alignment =
                    ParagraphAlignment.Center;

                nameParagraph.Format.Font.Size = 10;
                nameParagraph.Format.Font.Bold = true;

                nameParagraph.Format.Font.Color = Colors.White;

                nameParagraph.AddText("Total FBM");

                // Value cell -----------------------------------------

                Cell valueCell = resultRow.Cells[1];

                Paragraph valueParagraph =
                    valueCell.AddParagraph();

                valueParagraph.Format.Alignment =
                    ParagraphAlignment.Center;

                valueParagraph.Format.Font.Size = 10;
                valueParagraph.Format.Font.Bold = true;

                valueParagraph.AddText(
                    fbm.ToString("N0") + " FBM");

                // Footer --------------------------------------------------

                Paragraph footer = section.Footers.Primary.AddParagraph();

                footer.Format.Alignment = ParagraphAlignment.Center;
                footer.Format.Font.Size = 8;
                footer.Format.Font.Color = Colors.Gray;

                footer.AddText("Sawmill Production Report  •  ");
                footer.AddPageField();

                // Render PDF ----------------------------------------------

                PdfDocumentRenderer renderer = new PdfDocumentRenderer();
                renderer.Document = document;
                renderer.RenderDocument();

                renderer.PdfDocument.Save(filePath);
            }
            catch (Exception ex)
            {
                success = false;
                MessageBox.Show($"Error generating daily report: {ex.Message}\n\nApplication may need to be restarted.",
                    "Report Generation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            return success;
        }

        private static string FormatDimension(double value, bool length)
        {
            if (length)
            {
                return $"{value:0.###}'";
            }
            return $"{value:0.###}\"";
        }

        private static void AddCellText(Cell cell, string text, bool header = false)
        {
            try
            {
                Paragraph paragraph = cell.AddParagraph();

                paragraph.Format.Alignment =
                    ParagraphAlignment.Center;

                paragraph.Format.SpaceBefore = 0;
                paragraph.Format.SpaceAfter = 0;

                paragraph.Format.Font.Size = 9;

                if (header)
                {
                    paragraph.Format.Font.Bold = true;
                    paragraph.Format.Font.Color = Colors.White;
                }

                paragraph.AddText(text);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting cell text in report: {ex.Message}\n\nApplication may need to be restarted.",
                    "Report Generation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

        }
    }
}
