using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Spruce_Wood_Loggers_ERP.Persistence
{
    class CutLengthPersistence
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
    }
}
