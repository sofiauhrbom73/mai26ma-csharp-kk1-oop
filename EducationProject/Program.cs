
using EducationProject;

// This program shows a simple menu-based education system.
// The user can add and remove students from courses,
// view rosters, and see each student's schedule.

Console.WriteLine("Hello, Education Project!");

using StringWriter runLog = new();
TextWriter originalOutput = Console.Out;
Console.SetOut(new TeeWriter(originalOutput, runLog));
DeletePreviousResultsFile();

Course csharp = new("C#", 2);
Course java = new("Java", 3);
Course python = new("Python", 5);

Student sofia = new("Sofia");
Student daniel = new("Daniel");
Student arne = new("Arne");
Student hadi = new("Hadi");
Student lara = new("Lara");

List<Student> allStudents = [sofia, daniel, arne, hadi, lara];
List<Course> allCourses = [csharp, java, python];

// Seed some students so the menu has a working starting point.
Console.WriteLine("====================================");
Console.WriteLine("     Initializing Students and Courses");
Console.WriteLine("====================================");
EnrollAndReport("Add student", sofia, csharp);
EnrollAndReport("Add student", daniel, csharp);
EnrollAndReport("Add student", hadi, python);
EnrollAndReport("Add student", arne, python);

while (true)
{
    Console.WriteLine();
    Console.WriteLine("====================================");
    Console.WriteLine("        Education Management Menu");
    Console.WriteLine("====================================");
    Console.WriteLine("1. Add student to a course");
    Console.WriteLine("2. Remove student from a course");
    Console.WriteLine("3. Show roster for a course");
    Console.WriteLine("4. Show schedule for a student");
    Console.WriteLine("5. Show all courses and students");
    Console.WriteLine("6. Exit");
    Console.Write("Choose an option: ");

    string? choice = Console.ReadLine();
    Console.WriteLine();

    switch (choice)
    {
        case "1":
            AddStudentToCourseMenu();
            break;
        case "2":
            RemoveStudentFromCourseMenu();
            break;
        case "3":
            ShowCourseRosterMenu();
            break;
        case "4":
            ShowStudentScheduleMenu();
            break;
        case "5":
            ShowAllCoursesAndStudents();
            break;
        case "6":
            ExitProgram();
            return;
        default:
            Console.WriteLine("Invalid choice. Please select a number from 1 to 6.");
            break;
    }
}

void AddStudentToCourseMenu()
{
    Console.Write("Enter student name: ");
    string? studentName = Console.ReadLine();
    Console.Write("Enter course name (C#, Java, Python): ");
    string? courseName = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(studentName) || string.IsNullOrWhiteSpace(courseName))
    {
        Console.WriteLine("Student name and course name cannot be empty.");
        return;
    }

    Student newStudent = FindOrCreateStudent(studentName);
    Course? course = FindCourse(courseName);

    if (course is null)
    {
        Console.WriteLine($"Course '{courseName}' does not exist.");
        return;
    }

    EnrollAndReport("Add student", newStudent, course);
}

void RemoveStudentFromCourseMenu()
{
    Console.Write("Enter student name: ");
    string? studentName = Console.ReadLine();
    Console.Write("Enter course name (C#, Java, Python): ");
    string? courseName = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(studentName) || string.IsNullOrWhiteSpace(courseName))
    {
        Console.WriteLine("Student name and course name cannot be empty.");
        return;
    }

    Course? course = FindCourse(courseName);
    Student? student = FindStudent(studentName);

    if (course is null)
    {
        Console.WriteLine($"Course '{courseName}' does not exist.");
        return;
    }

    if (student is null)
    {
        Console.WriteLine($"Student '{studentName}' does not exist.");
        return;
    }

    RemoveAndReport("Remove student", course, student);
}

void ShowCourseRosterMenu()
{
    Console.Write("Enter course name (C#, Java, Python): ");
    string? courseName = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(courseName))
    {
        Console.WriteLine("Course name cannot be empty.");
        return;
    }

    Course? course = FindCourse(courseName);

    if (course is null)
    {
        Console.WriteLine($"Course '{courseName}' does not exist.");
        return;
    }

    PrintCourseWithStudents(course);
}

void ShowStudentScheduleMenu()
{
    Console.Write("Enter student name: ");
    string? studentName = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(studentName))
    {
        Console.WriteLine("Student name cannot be empty.");
        return;
    }

    Student? student = FindStudent(studentName);

    if (student is null)
    {
        Console.WriteLine($"Student '{studentName}' does not exist.");
        return;
    }

    PrintSchedule(student);
}

void ShowAllCoursesAndStudents()
{
    Console.WriteLine("Course overview:");
    Console.WriteLine();

    foreach (Course course in allCourses)
    {
        // The ToString() method is used automatically when we print the object.
        Console.WriteLine($"Course object summary: {course}");
        PrintCourseWithStudents(course);
    }
}

void ExitProgram()
{
    Console.Write("Are you sure you want to quit? (Y/N): ");
    string? answer = Console.ReadLine();

    if (!string.Equals(answer, "Y", StringComparison.OrdinalIgnoreCase) &&
        !string.Equals(answer, "Yes", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("Exit cancelled.");
        return;
    }

    Console.WriteLine();
    Console.WriteLine("Program finished.");
    Console.WriteLine("Final summary:");
    ShowAllCoursesAndStudents();
    if (SaveResultsFile())
    {
        Console.WriteLine("Results saved to results.txt.");
    }
    else
    {
        Console.WriteLine("Results could not be saved to results.txt.");
    }
    Console.WriteLine("Thank you for using the Education Program. Goodbye!");
}

void DeletePreviousResultsFile()
{
    try
    {
        if (File.Exists("results.txt"))
        {
            File.Delete("results.txt");
        }
    }
    catch (IOException)
    {
    }
    catch (UnauthorizedAccessException)
    {
    }
}

bool SaveResultsFile()
{
    Console.SetOut(originalOutput);

    try
    {
        DeletePreviousResultsFile();
        FileHandler.SaveToFile("results.txt", runLog.ToString(), csharp, java, python);
        return true;
    }
    catch (IOException)
    {
        return false;
    }
    catch (UnauthorizedAccessException)
    {
        return false;
    }
}

Student FindOrCreateStudent(string name)
{
    Student? student = FindStudent(name);
    if (student is not null)
    {
        return student;
    }

    Student newStudent = new(name);
    allStudents.Add(newStudent);
    return newStudent;
}

Student? FindStudent(string name)
{
    foreach (Student student in allStudents)
    {
        if (string.Equals(student.Name, name, StringComparison.OrdinalIgnoreCase))
        {
            return student;
        }
    }

    return null;
}

Course? FindCourse(string name)
{
    foreach (Course course in allCourses)
    {
        if (string.Equals(course.Name, name, StringComparison.OrdinalIgnoreCase))
        {
            return course;
        }
    }

    return null;
}

void EnrollAndReport(string action, Student student, Course course)
{
    EnrollmentResult result = student.Join(course);

    switch (result)
    {
        case EnrollmentResult.Enrolled:
            Console.WriteLine($"{action}: {student.Name} was added to {course.Name}.");
            break;
        case EnrollmentResult.AlreadyEnrolled:
            Console.WriteLine($"{action}: {student.Name} is already enrolled in {course.Name}.");
            break;
        case EnrollmentResult.CourseFull:
            Console.WriteLine($"{action}: {course.Name} is full. {student.Name} could not be added.");
            break;
    }
}

void RemoveAndReport(string action, Course course, Student student)
{
    if (course.Remove(student))
    {
        Console.WriteLine($"{action}: {student.Name} was removed from {course.Name}.");
    }
    else
    {
        Console.WriteLine($"{action}: {student.Name} is not enrolled in {course.Name}.");
    }
}

void PrintCourseWithStudents(Course course)
{
    if (course.Students.Count == 0)
    {
        Console.WriteLine($"Course {course.Name} has no students enrolled.");
        return;
    }

    string students = string.Join(", ", course.Students.Select(student => student.Name));
    Console.WriteLine($"Course {course.Name} has students: {students}");
    // This shows how ToString() is called automatically when an object is interpolated.
    Console.WriteLine($"ToString output: {course}");
}

void PrintSchedule(Student student)
{
    Console.WriteLine($"\nSchedule for {student.Name}:");
    // When a Student is printed, C# calls ToString() automatically.
    Console.WriteLine($"Student summary: {student}");

    if (student.Courses.Count == 0)
    {
        Console.WriteLine("No courses enrolled.");
        return;
    }

    foreach (Course course in student.Courses)
    {
        Console.WriteLine($"- {course}");
    }
}

internal sealed class TeeWriter(TextWriter first, TextWriter second) : TextWriter
{
    public override System.Text.Encoding Encoding => first.Encoding;

    public override void Write(char value)
    {
        first.Write(value);
        second.Write(value);
    }

    public override void WriteLine(string? value)
    {
        first.WriteLine(value);
        second.WriteLine(value);
    }
}

// Handles saving the program's output and final course information to a text file.
internal class FileHandler
{
    // Creates the output file and writes both the run log and the final course lists.
    internal static void SaveToFile(string fileName, string runLog, Course csharp, Course java, Course python)
    {
        // The writer is closed automatically when this method finishes.
        using StreamWriter writer = new(fileName);

        // Write the output from the program run first.
        writer.WriteLine("Run results");
        writer.WriteLine("================");
        writer.WriteLine("Steps during the run:");
        writer.WriteLine(runLog);
        writer.WriteLine();

        // Then write the final state of each course.
        writer.WriteLine("Courses after the run:");
        WriteCourse(writer, csharp);
        WriteCourse(writer, java);
        WriteCourse(writer, python);
    }

    // Writes one course's name, seat usage, and enrolled students.
    private static void WriteCourse(StreamWriter writer, Course course)
    {
        if (course.Students.Count == 0)
        {
            writer.WriteLine($"Course {course.Name} has no students enrolled.");
            return;
        }

        string students = string.Join(", ", course.Students.Select(student => student.Name));
        writer.WriteLine($"Course {course.Name} has students: {students}");
    }

    private static void WriteSchedule(StreamWriter writer, Student student)
    {
        writer.WriteLine($"Schedule for {student.Name}:");

        if (student.Courses.Count == 0)
        {
            writer.WriteLine("No courses enrolled.");
            return;
        }

        foreach (Course course in student.Courses)
        {
            writer.WriteLine($"- {course.Name}");
        }
    }
}
