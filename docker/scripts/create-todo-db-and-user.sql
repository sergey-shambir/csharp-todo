IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'todo')
BEGIN
    CREATE DATABASE [todo];
END
GO

USE [todo]
GO

IF NOT EXISTS (SELECT * FROM sys.syslogins WHERE name = 'todoapi')
BEGIN
    CREATE LOGIN todoapi WITH PASSWORD = 'em4xooNu';
    CREATE USER todoapi FOR LOGIN todoapi;
    EXEC sp_addrolemember 'db_datareader', 'todoapi';
    EXEC sp_addrolemember 'db_datawriter', 'todoapi';
    EXEC sp_addrolemember 'db_ddladmin', 'todoapi';
END
GO
