CREATE TABLE [dbo].[MedicineCategory] (
    [Id]   INT    PRIMARY KEY       IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (255) NOT NULL UNIQUE 
);
