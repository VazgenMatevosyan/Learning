﻿Insert INTO Courses ([title],[description])
Values ('C++','C++')
Insert INTO Courses ([title],[description])
Values ('Math','Math')
Insert INTO Courses ([title],[description])
Values ('Physic','Physic')

Insert Into Students([name],[surname],[SSN])
Values ('Hrant','Arakelyan','AP1231345')
Insert Into Students([name],[surname],[SSN])
Values('Narek','Hovhannisyan','AP154812')
Insert Into Students([name],[surname],[SSN])
Values ('Vrezh','Arakelyan','AP4556789')

Insert Into Teachers([name],[surname],[SSN])
Values	('Georg','Cantor','AR458756')
Insert Into Teachers([name],[surname],[SSN])
Values	('Albert','Einstein','AR49864')
Insert Into Teachers([name],[surname],[SSN])
Values	('Bjarne','Stroustrup','AR125888')

Insert Into Topics([course_title],[name])
Values	('C++','Introduction')
Insert Into Topics([course_title],[name])
Values	('C++','HelloWorld')
Insert Into Topics([course_title],[name])
Values	('Math','Euclids algorithm')
Insert Into Topics([course_title],[name])
Values	('Physic','Relativity theory')

Insert Into CoursesTeachersRelation([id_teacher],[title_course])
Values(1,'Math')
Insert Into CoursesTeachersRelation([id_teacher],[title_course])
Values(2,'Physic')
Insert Into CoursesTeachersRelation([id_teacher],[title_course])
Values(3,'C++')

Insert Into CoursesStudentsRelation([id_student],[title_course],[grade])
Values(1,'C++',20)
Insert Into CoursesStudentsRelation([id_student],[title_course],[grade])
Values(2,'C++',19)
Insert Into CoursesStudentsRelation([id_student],[title_course])
Values(1,'Math')

