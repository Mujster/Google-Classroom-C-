DROP TABLE student;
DROP TABLE teacher;
DROP TABLE class;
DROP TABLE announcement;
DROP TABLE student_class;
DROP TABLE comment;
DROP TABLE assignment;
DROP TABLE material;
DROP TABLE deadline;

-- Students table
CREATE TABLE student (
StudentId INT PRIMARY KEY,
StudentName VARCHAR(255) NOT NULL,
StudentEmail VARCHAR(255) NOT NULL,
StudentPassword VARCHAR(255) NOT NULL
);

-- Teachers table
CREATE TABLE teacher (
TeacherId INT PRIMARY KEY,
TeacherName VARCHAR(255) NOT NULL,
TeacherEmail VARCHAR(255) NOT NULL,
TeacherPassword VARCHAR(255) NOT NULL
);

-- Classes table
CREATE TABLE class (
ClassId INT PRIMARY KEY,
ClassName VARCHAR(255) NOT NULL,
TeacherId INT NOT NULL,
ClassCode VARCHAR(255) NOT NULL,
FOREIGN KEY (TeacherId) REFERENCES teacher(TeacherId)
);

-- Grade Table
CREATE TABLE grade(
StudentId INT,
GradeId INT,
AssignmentId INT,
GradeScore INT
FOREIGN KEY (StudentId) REFERENCES student(StudentId),
FOREIGN KEY (AssignmentId) REFERENCES assignment(AssignmentId)
);

-- Announcements table
CREATE TABLE announcement (
AnnouncementId INT PRIMARY KEY,
AnnouncementContent VARCHAR(255) NOT NULL,
AnnouncementTitle VARCHAR(255) NOT NULL,
ClassId INT NOT NULL,
TeacherId INT NOT NULL,
AnnouncementDate DATE NOT NULL,
FOREIGN KEY (ClassId) REFERENCES class(classid),
FOREIGN KEY (TeacherId) REFERENCES teacher(teacherid)
);

-- Assignments table
CREATE TABLE assignment (
AssignmentId INT PRIMARY KEY,
AssignmentTitle VARCHAR(255) NOT NULL,
AssignmentContent VARCHAR(255),
DeadlineId INT NOT NULL,
ClassId INT NOT NULL,
TeacherId INT NOT NULL,
TotalScore INT,
FOREIGN KEY (ClassId) REFERENCES class(classid),
FOREIGN KEY (TeacherId) REFERENCES teacher(teacherid),
FOREIGN KEY (DeadlineId) REFERENCES deadline(DeadlineId)
);

-- Deadline table
CREATE TABLE deadline(
DeadlineId INT PRIMARY KEY,
AssignmentId INT NOT NULL,
DeadlineDate DATE NOT NULL,
);

-- Materials table
CREATE TABLE material (
MaterialId INT PRIMARY KEY,
Title VARCHAR(255) NOT NULL,
ClassId INT NOT NULL,
TeacherId INT NOT NULL,
FOREIGN KEY (ClassId) REFERENCES class(classid),
FOREIGN KEY (TeacherId) REFERENCES teacher(teacherid)
);

-- Comments table
CREATE TABLE comment (
CommentId INT PRIMARY KEY,
CommentContent VARCHAR(255) NOT NULL,
DateCreated DATE NOT NULL,
ClassId INT NOT NULL,
CommenterType VARCHAR(255) NOT NULL,
TeacherId INT,
StudentId INT,
FOREIGN KEY (ClassId) REFERENCES class(classid),
FOREIGN KEY (TeacherId) REFERENCES teacher(teacherid),
FOREIGN KEY (StudentId) REFERENCES student(studentid)
);

--Student Class Table
CREATE TABLE student_class(
StudentId INT,
ClassId INT,
FOREIGN KEY (StudentId) REFERENCES student(StudentId),
FOREIGN KEY (ClassId) REFERENCES class(ClassId)
);

--Teacher Class Table
CREATE TABLE teacher_class(
TeacherId INT,
ClassId INT,
FOREIGN KEY (TeacherId) REFERENCES teacher(TeacherId),
FOREIGN KEY (ClassId) REFERENCES class(ClassId)
);

--Assignment Student Table
CREATE TABLE assignment_student(
AssignmentId INT,
StudentId INT,
SubmittedDate DATE,
AssignmentWork VARCHAR(255)
FOREIGN KEY (StudentId) REFERENCES student(StudentId),
FOREIGN KEY (AssignmentId) REFERENCES assignment(AssignmentId)
);

--INSERTION Queries
INSERT INTO student (StudentId, StudentName, StudentEmail, StudentPassword)
VALUES (1, 'Mujtaba Ahmad', 'mujtaba.ahmad@nu.edu.pk', 'p0001'),
       (2, 'Taha K', 'tahak@nu.edu.pk', 'p0002'),
       (3, 'Tassadaq A', 'tsdq@nu.edu.pk', 'p0003'),
	   (4, 'Faiq A', 'faiq@nu.edu.pk', 'p0004'),
	   (5, 'Tommy', 'talha@nu.edu.pk','p0005'),
	   (6, 'Gaizi', 'faizi@nu.edu.pk', 'p0006'),
	   (7, 'Commando', 'shah@nu.edu.pk', 'p0007'),
	   (8, 'Snooker', 'hammad@nu.edu.pk', 'p0008');

INSERT INTO teacher_class(TeacherId,ClassId)
VALUES 
(1,1),
(2,2),
(3,3);

INSERT INTO student_class (StudentId, ClassId)
VALUES
(1, 1),(1, 2),(1, 3),
(2, 1),(2, 2),(2, 3),
(3, 1),(3, 2),
(4, 1),(4, 3),
(5, 1),(5, 2),(5, 3),
(6, 2),(6, 3),
(7, 1),(7, 2),
(8, 1),(8, 2),(8, 3);
    

INSERT INTO teacher (TeacherId, TeacherName, TeacherEmail, TeacherPassword)
VALUES (1, 'Abeeda Akram', 'abeeda.akram@nu.edu.pk', 'p0001'),
       (2, 'Waqas Manzoor', 'waqas.manzoor@nu.edu.pk', 'p0002'),
       (3, 'Zeeshan Rana', 'zeeshan.rana@nu.edu.pk', 'p0003');

INSERT INTO class (ClassId, ClassName, TeacherId, ClassCode)
VALUES (1, 'Programming Fundamentals', 1, 'CS1001'),
       (2, 'O.O.P', 2, 'CS2001'),
       (3, 'Software Engineering', 3, 'SE2001');

INSERT INTO grade(GradeId,StudentId,AssignmentId,GradeScore) VALUES
(1,1,1,90),(2,1,2,205),
(3,2,1,87),(4,2,2,190),
(5,3,1,90),(6,3,2,200),
(7,4,1,90),(8,4,2,180),
(9,5,1,90),(10,5,2,140),
(11,6,1,90),(12,6,2,109),
(13,7,1,90),(14,7,2,185),
(15,8,1,56);

INSERT INTO announcement (AnnouncementId, AnnouncementContent, AnnouncementTitle, ClassId, TeacherId, AnnouncementDate)
VALUES (1, 'Reminder: Test next Monday', 'Test Next Week', 1, 1, '2023-05-17'),
       (2, 'Homework assignment due Friday', 'Homework Assignment', 2, 2, '2023-05-20'),
       (3, 'Software Lifecycle Evaluation', 'Evaluation', 3, 3, '2023-05-18');

INSERT INTO assignment (AssignmentId, AssignmentTitle,AssignmentContent ,DeadlineId, ClassId, TeacherId,TotalScore)
VALUES (1, 'PF Assigment 3','C++ code to calculate factorial using loop' ,1, 1, 1,100),
       (2, 'Operator Overloading','Virtual and friend functions' ,2, 2, 2,220);
       
INSERT INTO deadline (DeadlineId, AssignmentId, DeadlineDate)
VALUES (1, 1, '2023-05-19'),
       (2, 2, '2023-05-21');

INSERT INTO assignment_student(AssignmentId,StudentId,SubmittedDate,AssignmentWork) VALUES
(1,1,'2023-05-18','It is a process of checking whether the software'),(2,1,'2023-05-20',' is up to the mark and has met the 
requirements.'),
(1,2,'2023-05-17','t checks whether a developer is developing is a right produc'),(2,2,'2023-05-19','t checks whether a developer is developing is a right produc'),
(1,3,'2023-05-18','t checks whether a developer is developing is a right produc'),(2,3,'2023-05-17','t checks whether a developer is developing is a right produc'),
(1,4,'2023-05-19','t checks whether a developer is developing is a right produc'),(2,4,'2023-05-20','t checks whether a developer is developing is a right produc'),
(1,5,'2023-05-17','t checks whether a developer is developing is a right produc'),(2,5,'2023-05-16','t checks whether a developer is developing is a right produc'),
(1,6,'2023-05-16','t checks whether a developer is developing is a right produc'),(2,6,'2023-05-17','t checks whether a developer is developing is a right produc'),
(1,7,'2023-05-18','t checks whether a developer is developing is a right produc'),(2,7,'2023-05-19','t checks whether a developer is developing is a right produc'),
(1,8,'2023-05-20','t checks whether a developer is developing is a right produc');

INSERT INTO material (MaterialId, Title, ClassId, TeacherId)
VALUES (1, 'C++ D.S Malik', 1, 1),
       (2, 'OOP C++ Polymorphism pptx', 2, 2),
       (3, 'Software LifeCycles', 3, 3);

INSERT INTO comment (CommentId, CommentContent, DateCreated, ClassId,CommenterType, TeacherId, StudentId)
VALUES (1, 'Great job on the presentation!', '2023-05-14', 1,'Teacher', 1, 1),
       (2, 'Can we have a review session?', '2023-05-13', 2,'Student', 2, 1),
       (3, 'I need help with this concept', '2023-05-12', 3,'Student', 3, 3),
	   (4, 'Pls extend deadline', '2023-05-12', 1,'Student', 1, 3),
	   (5, 'Great work', '2023-05-12', 1,'Student', 1, 3),
	   (6, 'New Topics learned?', '2023-05-12', 3,'Student', 3, 3);

-- SELECT Queries
SELECT * FROM student;
SELECT * FROM teacher;
SELECT * FROM class;
SELECT * FROM assignment;
SELECT * FROM deadline;
SELECT * FROM material;
SELECT * FROM comment;
SELECT * FROM teacher_class;
SELECT * FROM student_class;
SELECT * FROM assignment_student;


DELETE FROM class WHERE ClassId='4';

DELETE FROM class WHERE ClassId='4' AND TeacherId='1';