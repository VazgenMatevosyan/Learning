CREATE TABLE [dbo].[Courses] (
	[id] int identity(1,1) not null,
    [title]       VARCHAR (100)  NOT NULL unique,
    [description] VARCHAR (8000) NULL,
    constraint P_k_Courses PRIMARY KEY CLUSTERED ([id] ASC)
);