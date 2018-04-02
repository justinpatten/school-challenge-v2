use  master
go

if exists (select [name] from sys.databases where [name] = 'SchoolChallengeDB' )
drop database SchoolChallengeDB
go

create database SchoolChallengeDB
go

use SchoolChallengeDB
go

if exists (select [name] from sys.tables where [name] = 'Students' )
drop table Students
go

if exists (select [name] from sys.tables where [name] = 'Teachers' )
drop table Teachers
go

create table [dbo].[Students](
	    [Id] int identity primary key,   
        [StudentNumber] [varchar](100) not null,      
        [FirstName]  [varchar](50) not null,      
        [LastName]  [varchar](50) not null,      
        [HasScholarship]  [bit] not null   
)

create table [dbo].[Teachers](
	    [Id] int identity primary key,     
        [FirstName]  [varchar](50) not null,      
        [LastName]  [varchar](50) not null,      
        [NumberOfStudents]  [int] null    
)
