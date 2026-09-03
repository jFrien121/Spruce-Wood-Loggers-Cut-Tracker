using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

/**
 * BundlePersistence
 * Handles saving and retrieving bundles from the database
 */

namespace Spruce_Wood_Loggers_ERP
{
    class BundlePersistence
    {

        public static void SaveBundle(Bundle bundle)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    db.Bundles.Add(bundle);
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
