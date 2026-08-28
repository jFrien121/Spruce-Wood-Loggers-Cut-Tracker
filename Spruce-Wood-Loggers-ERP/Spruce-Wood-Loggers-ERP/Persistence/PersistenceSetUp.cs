using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Spruce_Wood_Loggers_ERP.Persistence
{
    class PersistenceSetUp
    {

        public static void ConnectToDatabase()
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error connecting to the database: {ex.Message}\n\nApplication may need to be restarted.",
                    "Database connecting Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
