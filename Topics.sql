CREATE TABLE [dbo].[Topics] (
    [id] int identity(1,1) not null,
    [name]         VARCHAR (100) NOT NULL ,
    [course_title] VARCHAR (100) NOT NULL,
    constraint P_K_Topics PRIMARY KEY CLUSTERED ([id] ASC),
    constraint F_K_Course_Title_Topics FOREIGN KEY ([course_title]) REFERENCES [dbo].[Courses] ([title])
);