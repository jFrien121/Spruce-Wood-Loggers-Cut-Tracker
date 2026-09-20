# Spruce Wood Loggers Cut Tracker
A C#, WPF application for tracking and reporting the daily production rates at Spruce Wood Loggers. Multi station database connection is enabled using PostgreSQL for persistence.

To establish initial database connection, a JSON config file must be set up in the executing directory of the program.

## Reporting

Report generation is enabled with the application, and it summarizes the amount of lifts per dimension that have been completed as well as total board feet, totalling from the day that the report is being generated.

## Tracked Information

For each bundle, its thickness, width, and length are stored as well as the number of pieces on the lift and the grade. Several dimensions have a standard number of pieces that are generally on the lift for that dimension. To increase the application efficiency, the program suggests the corresponding standard value when the appropriate dimension is selected.
