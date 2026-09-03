using System;
using System.Collections.Generic;
using System.Text;

/**
 * Bundle
 * Holds information about a processed bundle of logs
 */

namespace Spruce_Wood_Loggers_ERP
{
    class Bundle
    {
        public int id { get; set; } // primary key
        public DateTime timeProcessed { get; set; }
        public double thickness { get; set; }
        public double width { get; set; }
        public double length { get; set; }
        public string grade { get; set; }
        public int numPieces { get; set; }

        public Bundle(DateTime timeProcessed, double thickness, double width, double length, string grade, int numPieces)
        {
            this.timeProcessed = timeProcessed;
            this.thickness = thickness;
            this.width = width;
            this.length = length;
            this.grade = grade;
            this.numPieces = numPieces;
        }

        // Emptry constructor for database purposes
        public Bundle()
        {

        }
    }
}
