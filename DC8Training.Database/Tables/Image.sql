CREATE TABLE [dbo].[Image] (
    [Id]         INT     PRIMARY KEY IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (1000)  NOT NULL,
    [URL]        NVARCHAR (1000) NOT NULL,
    [MedicineId] INT FOREIGN KEY REFERENCES [dbo].[Medicine] ([Id]) NULL
);