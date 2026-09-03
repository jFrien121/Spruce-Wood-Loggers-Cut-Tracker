using Microsoft.EntityFrameworkCore;
using Spruce_Wood_Loggers_ERP.Database_Objects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using System.Windows;

namespace Spruce_Wood_Loggers_ERP.Persistence
{
    class CutDimensionPersistence
    {

        public static async Task<List<double>> LoadCutLengths()
        {

            List<double> cutLengths = new List<double>();

            try
            {
                using (var db = new AppDbContext())
                {
                    cutLengths = await db.CutLengths.Select(l => l.length).ToListAsync();
                }
                cutLengths.Sort((a, b) => a.CompareTo(b));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading grid length dimensions: {ex.Message}\n\nApplication may need to be restarted.",
                    "Database Loading Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            return cutLengths;
        }

        public static async Task<List<CutSize>> LoadCutSizes()
        {
            List<CutSize> cutSizes = new List<CutSize>();

            try
            {
                using (var db = new AppDbContext())
                {
                    cutSizes = await db.CutSizes.ToListAsync();
                }

                cutSizes.Sort((a, b) =>
                {
                    int cmp = a.thickness.CompareTo(b.thickness); // primary sort
                    if (cmp == 0)
                        cmp = a.width.CompareTo(b.width); // secondary sort
                    return cmp;
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading grid size dimensions: {ex.Message}\n\nApplication may need to be restarted.",
                    "Database Loading Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            return cutSizes;
        }

        public static async Task<List<int>> LoadStandardNumPieces(double thickness, double width)
        {
            List<int> standardNumPieces = new List<int>();
            try
            {
                using (var db = new AppDbContext())
                {
                    standardNumPieces = await db.Database.SqlQuery<int>(
                            $"SELECT * FROM cut_tracker_standard_num_pieces_get({thickness},{width})").ToListAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading standard num pieces for selected dimensions: {ex.Message}\n\nApplication may need to be restarted.",
                    "Database Loading Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            return standardNumPieces;

        }
    }
}
