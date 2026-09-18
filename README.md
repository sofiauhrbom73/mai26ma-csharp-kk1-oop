# Part B — Courses and Students (Classes and Objects)

Write a small system for courses and students that models their relationship in both directions: a course can have multiple students, and a student can attend multiple courses.

This is a C# project *(created with `dotnet new console`; see the instructions in the course overview article from the first lesson).* Each class should be placed in its own file, and you should test them by creating objects and calling methods in **Program.cs**.

## Getting Started

Install the .NET 10 SDK, then clone and start the project:

```bash
git clone https://github.com/<your-username>/<your-repository>.git
cd <your-repository>/EducationProject
dotnet restore
dotnet run
```

Use menu option **6. Exit** and confirm with `Y` or `Yes` to finish normally and create `results.txt`.

You should have (at least) two classes:

## Course *(in Course.cs)*

* Fields: `Name`, a capacity `MaxSeats` (maximum number of seats), and a list `Students`.
* Method `Enroll(student)` — enrolls a student in the course if there is available space.
* Method `Remove(student)` — removes a student from the course.
* Method `RollCall()` — prints all students enrolled in the course.
* A `ToString()` method that returns something like `"Mathematics (2/5 seats)"`.

## Student *(in Student.cs)*

* Fields: `Name` and a list `Courses`.
* Method `Join(course)` — joins a course.
* Method `Leave(course)` — leaves a course.
* Method `Schedule()` — prints the courses the student is attending.
* A `ToString()` method that returns the student's name.

## The Rules That Make the Assignment

This is where the logic belongs:

### Consistency in Both Directions

When a student is enrolled in a course (whether through the course's `Enroll()` method or the student's `Join()` method), the student must be added to the course's `Students` list and the course must be added to the student's `Courses` list. The same applies when removing a student from a course.

### No Duplicates

The same student must not appear more than once in a course, regardless of how many times enrollment is attempted.

### Capacity

A course cannot enroll more students than its `MaxSeats` value. Instead of adding the student, display a message (for example, `"The course is full"`).

### No Crashes

The program must not crash if someone attempts to remove a student who is not enrolled in the course.

## In Program.cs

Create several courses and several students. Enroll and remove them using both the course methods and the student methods. Use `RollCall()` and `Schedule()` to demonstrate that the two-way relationship works correctly and that all the rules above are enforced (for example, that a full course rejects new students and that duplicate enrollments do not create duplicates).

## Saving Results

The program follows this normal-exit workflow:

* When the program starts, an old `results.txt` is deleted.
* When the user chooses **6. Exit** and confirms with `Y` or `Yes`, a new `results.txt` is created containing the run results.
* When the user presses `Ctrl+C`, the program stops immediately. The file is not removed, and the current run is not saved.

The file is recreated on each confirmed exit and contains:

* the output produced during the program run;
* the final list of students in each course.

This makes it possible to submit or review the result after the console program has finished.