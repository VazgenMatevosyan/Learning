CREATE TABLE [dbo].[Students] (
    [id]            INT          IDENTITY (1, 1) NOT NULL,
    [name]          VARCHAR (20) NOT NULL,
    [surname]       VARCHAR (20) NOT NULL,
    [SSN] varchar(50) unique not null,
    constraint P_K_Students PRIMARY KEY CLUSTERED ([id] ASC)
);