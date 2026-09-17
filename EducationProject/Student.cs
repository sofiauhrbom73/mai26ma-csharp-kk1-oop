namespace EducationProject;

// Student represents a student.
// A student can enroll in multiple courses.
public class Student(string name)
{
    // The student's name
    // Name can be publicly readable, but not publicly writable.
    public string Name { get; } = name;

    // Creates an empty list that will contain courses.
    // The actual courses list is private.
    // IReadOnlyList<Course> lets Program inspect and iterate 
    // over courses without adding or removing them directly.
    private readonly List<Course> courses = [];
    public IReadOnlyList<Course> Courses => courses;

    internal void AddCourse(Course course)
    {
        if (!courses.Contains(course))
        {
            courses.Add(course);
        }
    }

    internal void RemoveCourse(Course course)
    {
        courses.Remove(course);
    }

    // Enroll the student in a course
    public EnrollmentResult Join(Course course)
    {
        // We let the course handle the logic itself
        return course.Enroll(this);
    }

    // Leave a course
    public bool Leave(Course course)
    {
        // We let the course handle the removal
        return course.Remove(this);
    }

    // This method tells C# how to show the object as text.
    // It is called automatically by Console.WriteLine(student).
    public override string ToString()
    {
        if (courses.Count == 1)
        {
            return Name + " (enrolled in 1 course)";
        }
        else if (courses.Count > 0)
        {
            return Name + " (enrolled in " + courses.Count + " courses)";
        }
        else
        {
            return Name + " (enrolled in no courses)";
        }
    }
}
