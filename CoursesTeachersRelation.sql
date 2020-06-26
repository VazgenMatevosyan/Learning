CREATE TABLE [dbo].[CoursesTeachersRelation] (
    [id_teacher]   INT           NOT NULL,
    [title_course] VARCHAR (100) NOT NULL,
    constraint P_K_Courses_Teachers_Relation PRIMARY KEY CLUSTERED ([id_teacher] ASC, [title_course] ASC),
    constraint F_K_Id_Teacher_CTR FOREIGN KEY ([id_teacher]) REFERENCES [dbo].[Teachers] ([id]),
    constraint F_K_Title_Course_CTR FOREIGN KEY ([title_course]) REFERENCES [dbo].[Courses] ([title])
);