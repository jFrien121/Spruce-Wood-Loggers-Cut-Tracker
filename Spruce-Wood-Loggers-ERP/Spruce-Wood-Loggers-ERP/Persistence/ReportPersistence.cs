using Microsoft.EntityFrameworkCore;
using Spruce_Wood_Loggers_ERP.Database_Objects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

/**
 * ReportPersistence
 * Handle database persistence for report data.
 */

namespace Spruce_Wood_Loggers_ERP.Persistence
{
    internal class ReportPersistence
    {

        public static async Task<List<LiftResult>> LoadLiftNumbersPerDimension()
        {

            List<LiftResult> cutLengths = new List<LiftResult>();

            try
            {
                using (var db = new AppDbContext())
                {
                    cutLengths = await db.LiftResults
                                         .FromSqlRaw("SELECT * FROM Cut_Tracker_Number_Of_Lifts_Get()")
                                         .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading dimension lift summary from database: {ex.Message}\n\nApplication may need to be restarted.",
                    "Database Loading Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            return cutLengths;
        }
    }
}
