using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Spruce_Wood_Loggers_ERP
{
    class DatabaseConfig
    {
        public string ipAddress { get; set; }
        public int port { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        public static string getConfigPath()
        {
            try
            {
                return Environment.CurrentDirectory + @"\CutTrackerDBSettings.json";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting program directory: {ex.Message}\n\nApplication may need to be restarted.",
                    "Database Settings Path Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            return "";
        }
    }
}
