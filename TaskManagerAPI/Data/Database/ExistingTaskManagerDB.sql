IF EXISTS (SELECT 1
           FROM sys.databases
           WHERE name = N'TaskManagerDB')
    SELECT 1
ELSE
    SELECT 0;
