using Microsoft.EntityFrameworkCore;
using Spruce_Wood_Loggers_ERP.Database_Objects;
using System;
using System.Collections.Generic;
using System.Text;
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading grid size dimensions : {ex.Message}\n\nApplication may need to be restarted.",
                    "Database Loading Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            return cutSizes;
        }
    }
}
