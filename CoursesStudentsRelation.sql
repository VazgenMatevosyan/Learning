CREATE TABLE [dbo].[CoursesStudentsRelation] (
    [id_student]   INT           NOT NULL,
    [title_course] VARCHAR (100) NOT NULL,
    [grade]        FLOAT (53)    NULL default null,
    constraint P_K_Courses_Students_Relation PRIMARY KEY CLUSTERED ([id_student] ASC, [title_course] ASC),
    constraint F_K_Title_Course_CSR FOREIGN KEY ([title_course]) REFERENCES [dbo].[Courses] ([title]),
    constraint F_K_Id_Student_CSR FOREIGN KEY ([id_student]) REFERENCES [dbo].[Students] ([id])
);