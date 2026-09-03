## **Updating Database Structure in C# Program Instructions**



If some alteration is made to the model, in the AppDbContext class, the migration documents must be updated before the program can run again.



This can be done by running the following command in the Package Manager Console in Visual Studio:



&#x09;**Add-Migration *NewMigrationName***



The migration name can be any description of the changes.



Then the next command should also be run:



&#x09;**Update-Database**



The program should recognize which migrations have already been applied to the database, and should only apply the new ones.

