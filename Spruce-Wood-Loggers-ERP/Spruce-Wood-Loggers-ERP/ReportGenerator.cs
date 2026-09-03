using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using Npgsql.Internal.Postgres;
using Spruce_Wood_Loggers_ERP.Database_Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Spruce_Wood_Loggers_ERP
{
    internal class ReportGenerator
    {
        public static void CreatePdf(string filePath, List<LiftResult> numberOfLiftResults)
        {
            Document document = new Document();
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

            // ---------------------------------------------------------
            // Page setup
            // ---------------------------------------------------------

            Section section = document.AddSection();

            section.PageSetup.PageFormat = PageFormat.Letter;

            section.PageSetup.TopMargin = Unit.FromInch(0.6);
            section.PageSetup.BottomMargin = Unit.FromInch(0.6);
            section.PageSetup.LeftMargin = Unit.FromInch(0.6);
            section.PageSetup.RightMargin = Unit.FromInch(0.6);

            // ---------------------------------------------------------
            // Styles
            // ---------------------------------------------------------

            Style normalStyle = document.Styles["Normal"]!;

            normalStyle.Font.Name = "Arial";
            normalStyle.Font.Size = 10;

            Style titleStyle = document.Styles.AddStyle(
                "ReportTitle",
                "Normal");

            titleStyle.Font.Name = "Arial";
            titleStyle.Font.Size = 20;
            titleStyle.Font.Bold = true;
            titleStyle.Font.Color = Colors.DarkSlateGray;

            // ---------------------------------------------------------
            // Header
            // ---------------------------------------------------------

            Paragraph header = section.Headers.Primary.AddParagraph();

            header.Format.Alignment = ParagraphAlignment.Right;
            header.Format.Font.Size = 12;
            header.Format.Font.Color = Colors.Gray;

            header.AddText("Spruce Wood Loggers");
            section.PageSetup.TopMargin = Unit.FromInch(0.8);

            // ---------------------------------------------------------
            // Title
            // ---------------------------------------------------------

            Paragraph title = section.AddParagraph();
            title.Style = "ReportTitle";

            title.AddText("Sawmill Production Report - " + DateTime.Now.ToString("MMM") + ". "
                            + DateTime.Now.ToString("dd") + ", " + DateTime.Now.ToString("yyyy"));

            title.Format.SpaceAfter = Unit.FromPoint(25);
            title.Format.Alignment = ParagraphAlignment.Center;

            // ---------------------------------------------------------
            // Subtitle
            // ---------------------------------------------------------

            //Paragraph subtitle = section.AddParagraph();

            //subtitle.AddText(
            //    $"Total dimension sets: {numberOfLiftResults.Count}");

            //subtitle.Format.Font.Size = 10;
            //subtitle.Format.Font.Color = Colors.Gray;
            //subtitle.Format.SpaceAfter = Unit.FromPoint(15);

            // ---------------------------------------------------------
            // Table
            // ---------------------------------------------------------

            Table table = section.AddTable();

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

            Column heightColumn = table.AddColumn(
                Unit.FromInch(1.35));

            Column valueColumn = table.AddColumn(
                Unit.FromInch(1.35));

            // ---------------------------------------------------------
            // Header row
            // ---------------------------------------------------------

            Row headerRow = table.AddRow();

            headerRow.HeadingFormat = true;

            headerRow.Height = Unit.FromInch(0.4);

            headerRow.VerticalAlignment =
                VerticalAlignment.Center;

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
                "Width",
                true);

            AddCellText(
                headerRow.Cells[3],
                "Height",
                true);

            AddCellText(
                headerRow.Cells[4],
                "Number of Lifts",
                true);

            // ---------------------------------------------------------
            // Data rows
            // ---------------------------------------------------------

            for (int i = 0; i < numberOfLiftResults.Count; i++)
            {
                LiftResult item = numberOfLiftResults[i];

                Row row = table.AddRow();

                row.Height = Unit.FromInch(0.35);

                row.VerticalAlignment =
                    VerticalAlignment.Center;

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
                    FormatDimension(item.length));

                AddCellText(
                    row.Cells[2],
                    FormatDimension(item.thickness));

                AddCellText(
                    row.Cells[3],
                    FormatDimension(item.width));

                AddCellText(
                    row.Cells[4],
                    item.numberOfLifts.ToString());
            }

            // ---------------------------------------------------------
            // Footer
            // ---------------------------------------------------------

            Paragraph footer = section.Footers.Primary.AddParagraph();

            footer.Format.Alignment = ParagraphAlignment.Center;
            footer.Format.Font.Size = 8;
            footer.Format.Font.Color = Colors.Gray;

            footer.AddText("Dimension Report  •  ");
            footer.AddPageField();

            // ---------------------------------------------------------
            // Render PDF
            // ---------------------------------------------------------

            PdfDocumentRenderer renderer = new PdfDocumentRenderer();
            renderer.Document = document;
            renderer.RenderDocument();

            renderer.PdfDocument.Save(filePath);
        }

        private static string FormatDimension(double value)
        {
            return $"{value:0.###}\"";
        }

        private static void AddCellText(
            Cell cell,
            string text,
            bool header = false)
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
    }
}
