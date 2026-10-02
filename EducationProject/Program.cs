
using EducationProject;

// This program shows a simple menu-based education system.
// The user can add and remove students from courses,
// view rosters, and see each student's schedule.

Console.WriteLine("Hello, Education Project!");

using StringWriter runLog = new();
TextWriter originalOutput = Console.Out;
Console.SetOut(new TeeWriter(originalOutput, runLog));

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

// Demonstrate the enrollment rules before the interactive menu starts.
Console.WriteLine("====================================");
Console.WriteLine("     Enrollment Rule Demonstration");
Console.WriteLine("====================================");
EnrollAndReport(sofia, csharp);
EnrollAndReport(daniel, csharp);
EnrollAndReport(sofia, csharp);
EnrollAndReport(lara, csharp);
RemoveAndReport(csharp, lara);
EnrollAndReport(hadi, python);
EnrollAndReport(arne, python);

Console.WriteLine();
Console.WriteLine(csharp.RollCall());
Console.WriteLine(sofia.Schedule());
Console.WriteLine($"Student summary: {sofia}");

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

    if (choice is null)
    {
        Console.WriteLine("Input ended. Exiting program.");
        FinishProgram();
        return;
    }

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
            if (ExitProgram())
            {
                return;
            }
            break;
        default:
            if (int.TryParse(choice, out _))
            {
                Console.WriteLine("Invalid choice. Please select a number from the list.");
            }
            else
            {
                Console.WriteLine("Please enter a number from the list.");
            }
            break;
    }
}

void AddStudentToCourseMenu()
{
    string? studentName = ReadRequiredInput("Enter student name: ", "Student name cannot be empty.");
    if (studentName is null)
    {
        return;
    }

    Course? course = FindCourseWithRetry();
    if (course is null)
    {
        return;
    }

    Student newStudent = FindOrCreateStudent(studentName);
    EnrollAndReport(newStudent, course);
}

void RemoveStudentFromCourseMenu()
{
    Course? course = FindCourseWithRetry();
    if (course is null)
    {
        return;
    }

    string? studentName = ReadRequiredInput("Enter student name: ", "Student name cannot be empty.");
    if (studentName is null)
    {
        return;
    }

    Student? student = FindStudentWithRetry(studentName);

    if (student is null)
    {
        return;
    }

    RemoveAndReport(course, student);
}

void ShowCourseRosterMenu()
{
    Course? course = FindCourseWithRetry();
    if (course is null)
    {
        return;
    }

    Console.WriteLine(course.RollCall());
}

void ShowStudentScheduleMenu()
{
    string? studentName = ReadRequiredInput("Enter student name: ", "Student name cannot be empty.");
    if (studentName is null)
    {
        return;
    }

    Student? student = FindStudentWithRetry(studentName);

    if (student is null)
    {
        return;
    }

    Console.WriteLine(student.Schedule());
}

void ShowAllCoursesAndStudents()
{
    Console.WriteLine("Course overview:");
    Console.WriteLine();

    foreach (Course course in allCourses)
    {
        // The ToString() method is used automatically when we print the object.
        Console.WriteLine($"Course object summary: {course}");
        Console.WriteLine(course.RollCall());
    }
}

bool ExitProgram()
{
    Console.Write("Are you sure you want to quit? (Y/N): ");
    string? answer = Console.ReadLine();

    if (answer is null)
    {
        Console.WriteLine("Input ended. Exiting program.");
        FinishProgram();
        return true;
    }

    if (!string.Equals(answer, "Y", StringComparison.OrdinalIgnoreCase) &&
        !string.Equals(answer, "Yes", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("Exit cancelled.");
        return false;
    }

    FinishProgram();
    return true;
}

void FinishProgram()
{
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

bool SaveResultsFile()
{
    Console.SetOut(originalOutput);

    try
    {
        FileHandler.SaveToFile("results.txt", runLog.ToString(), csharp, java, python);
        return true;
    }
    catch (IOException exception)
    {
        Console.WriteLine($"Warning: Could not save results.txt. {exception.Message}");
        return false;
    }
    catch (UnauthorizedAccessException exception)
    {
        Console.WriteLine($"Warning: Could not save results.txt. {exception.Message}");
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

Student? FindStudentWithRetry(string name)
{
    while (true)
    {
        Student? student = FindStudent(name);
        if (student is not null)
        {
            return student;
        }

        Console.WriteLine($"Student '{name}' does not exist.");
        if (!AskToRetry())
        {
            return null;
        }

        string? nextName = ReadRequiredInput("Enter student name: ", "Student name cannot be empty.");
        if (nextName is null)
        {
            return null;
        }

        name = nextName;
    }
}

Course? FindCourseWithRetry()
{
    string? name = ReadRequiredInput("Enter course name (C#, Java, Python): ", "Course name cannot be empty.");
    if (name is null)
    {
        return null;
    }

    while (true)
    {
        Course? course = FindCourse(name);
        if (course is not null)
        {
            return course;
        }

        Console.WriteLine($"Course '{name}' does not exist.");
        if (!AskToRetry())
        {
            return null;
        }

        string? nextName = ReadRequiredInput("Enter course name (C#, Java, Python): ", "Course name cannot be empty.");
        if (nextName is null)
        {
            return null;
        }

        name = nextName;
    }
}

string? ReadRequiredInput(string prompt, string errorMessage)
{
    while (true)
    {
        Console.Write(prompt);
        string? input = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(input))
        {
            return input.Trim();
        }

        Console.WriteLine(errorMessage);
        if (!AskToRetry())
        {
            return null;
        }
    }
}

bool AskToRetry()
{
    Console.Write("Would you like to try again? (Y/N): ");
    string? answer = Console.ReadLine();
    return string.Equals(answer, "Y", StringComparison.OrdinalIgnoreCase) ||
        string.Equals(answer, "Yes", StringComparison.OrdinalIgnoreCase);
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

void EnrollAndReport(Student student, Course course)
{
    EnrollmentResult result = student.Join(course);
    Console.WriteLine(course.EnrollmentMessage(student, result));
}

void RemoveAndReport(Course course, Student student)
{
    bool removed = course.Remove(student);
    Console.WriteLine(course.RemovalMessage(student, removed));
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

}
