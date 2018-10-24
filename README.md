## Overview ##
A new client requires a simple web based application to manage students and teachers. A sample of their data is in the "SampleData" folder.

## Deliverables ##
A web application project, all code files, as well as any database create scripts necessary. The project should be runnable directly from within Visual Studio. Any external dependencies should be listed. 

## Instructions ##
- Fork this repository
- Clone your fork of the repository
- Commit / Push your code to the forked repository
- Send the URL of the repository to the person that sent you the original link to this repository


## Technical Requirements ##
- Final solution based on code provided
- Commit / Change history visible in repository
- TypeScript
- Custom styling - using some libraries, e.g. Bootstrap, is allowed, but please write some of your own as well
- Unit tests for C# code

## Implementation Requirements ##
- Convert existing database calls to utilize Entity Framework 
  - Connection string should be stored in the configuration file
  - Entity Framework context should be available via Dependency Injection in applicable MVC Controllers
- Teachers View 
  - Margins should be added to the left/right of the screen to ensure that content does not run to the edge
  - Insert/Edit teacher should allow the end user to enter the number of students that the teacher has
  - User should be able to upload a file to import a list of teachers. Sample file is provided
- Unit Tests 
  - Create a unit test to validate that the CSV files are parsed correctly


## Guide ##
Here are a couple user stories to help guide the development of the solution. 

- User wants to import data into students and teacher tables, via attached flat file.
- User wants to prevent the duplication of data that is imported, but update any changes detected in the file
- User wants to add and edit students
- User wants to see a list of students
- User wants lists to be sortable, have pagination, and be searchable
- Student list should show
    - Student ID
    - Student Number
    - First Name
    - Last Name
    - Has Scholarship
- User wants to add and edit teachers
- User wants to see a list of teachers
- Teacher list should show
    - Teacher ID
    - First Name
    - Last Name
    - Number of Students
- User wants to assign students to teachers
- User wants validation to prevent errors in data entry

These are just samples. Please add additional features to highlight your skills and thought process.

## External Dependencies ##
- Please have SQL Express 2016 installed on local machine with a .\SQLEXPRESS instance for the database.
- The CSV files should be in C:\Sample Data\, they should be named: Interview-Data-Students.csv and Interview-Data-Teachers.csv.
