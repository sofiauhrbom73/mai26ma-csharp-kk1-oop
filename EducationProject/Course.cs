namespace EducationProject;

public enum EnrollmentResult
{
    Enrolled,
    AlreadyEnrolled,
    CourseFull
}

// Course represents a course.
// A course can contain multiple students. 
public class Course(string name, int maxSeats)
{
    // The course's name
    // Name can be publicly readable, but not publicly writable.
    public string Name { get; } = name;

    // Max number of students that can enroll in the course.
    // MaxSeats can be publicly readable, but not publicly writable.
    public int MaxSeats { get; } = maxSeats;

    // Creates an empty list that will contain students
    // The actual student list is private.
    // IReadOnlyList<Student> lets Program inspect and iterate 
    // over students without adding or removing them directly.
    private readonly List<Student> students = [];
    public IReadOnlyList<Student> Students => students;

    // Register a student for the course
    public EnrollmentResult Enroll(Student student)
    {
        // Check if the student is already enrolled
        if (students.Contains(student))
        {
            return EnrollmentResult.AlreadyEnrolled;
        }

        // Check if the course is full
        if (students.Count >= MaxSeats)
        {
            return EnrollmentResult.CourseFull;
        }

        // Add the student to the course's list
        students.Add(student);

        // Also add the course to the student's schedule
        // so the connection works in both directions
        student.AddCourse(this);

        return EnrollmentResult.Enrolled;
    }

    // Remove a student from the course.
    public bool Remove(Student student)
    {
        if (students.Remove(student))
        {
            // Also remove the course from the student's schedule.
            student.RemoveCourse(this);
            return true;
        }

        return false;
    }

    public string RollCall()
    {
        if (students.Count == 0)
        {
            return $"Roll call for {Name}:{Environment.NewLine}No students enrolled.";
        }

        string studentNames = string.Join(
            Environment.NewLine,
            students.Select(student => $"- {student.Name}"));
        return $"Roll call for {Name}:{Environment.NewLine}{studentNames}";
    }

    public string EnrollmentMessage(Student student, EnrollmentResult result)
    {
        return result switch
        {
            EnrollmentResult.Enrolled => $"Add student: {student.Name} was added to {Name}.",
            EnrollmentResult.AlreadyEnrolled => $"Add student: {student.Name} is already enrolled in {Name}.",
            EnrollmentResult.CourseFull => $"Add student: {Name} is full. {student.Name} could not be added.",
            _ => throw new ArgumentOutOfRangeException(nameof(result), result, "Unknown enrollment result.")
        };
    }

    public string RemovalMessage(Student student, bool removed)
    {
        return removed
            ? $"Remove student: {student.Name} was removed from {Name}."
            : $"Remove student: {student.Name} is not enrolled in {Name}.";
    }

    // This method tells C# how to show the object as text.
    // It is called automatically by Console.WriteLine(course).
    public override string ToString()
    {
        if (students.Count == 1)
        {
            return $"{Name} ({students.Count}/{MaxSeats} seat filled)";
        }
        else
        {
            return $"{Name} ({students.Count}/{MaxSeats} seats filled)";
        }
    }
}