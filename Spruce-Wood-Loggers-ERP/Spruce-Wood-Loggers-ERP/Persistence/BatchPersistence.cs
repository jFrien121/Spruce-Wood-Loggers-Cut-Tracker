using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

/**
 * BatchPersistence
 * Handles saving and retrieving batches from the database
 */

namespace Spruce_Wood_Loggers_ERP
{
    class BatchPersistence
    {

        public static void SaveBatch(Batch batch)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    db.Batches.Add(batch);
                    db.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error saving to database: {ex.Message}\n\nApplication may need to be restarted.",
                    "Saving Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
