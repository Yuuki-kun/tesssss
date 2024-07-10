CREATE TABLE [dbo].[Medicine] (
    [Id]                 INT      PRIMARY KEY       IDENTITY (1, 1) NOT NULL,
    [Name]               NVARCHAR (255)  NOT NULL UNIQUE,
    [Description]        NVARCHAR (255)  NULL,
    [Price]              DECIMAL (19, 4) NOT NULL,
    [PrimaryImageId]     INT      NULL,
    [PopularityMedicine] BIT             NULL,
    [CategoryId]         INT     FOREIGN KEY REFERENCES [dbo].[MedicineCategory] ([Id]) NULL
);