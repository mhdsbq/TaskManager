IF NOT EXISTS (SELECT name
               FROM sys.databases
               WHERE name = N'TaskManagerDB')
CREATE DATABASE TaskManagerDB