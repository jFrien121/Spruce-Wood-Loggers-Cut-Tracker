# Spruce Wood Loggers Cut Tracker
A C#, WPF application for tracking and reporting the daily production rates at Spruce Wood Loggers. Multi station database connection is enabled using PostgreSQL for persistence.

To establish initial database connection, a JSON config file must be set up in the executing directory of the program.

## Reporting

Report generation is enabled with the application, and it summarizes the amount of lifts per dimension that have been completed as well as total board feet, totalling from the day that the report is being generated.
