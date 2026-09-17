REMEMBER for future study and implementations.

The project is easy to follow and demonstrates the main principles of object-oriented programming, such as encapsulation, association, and the use of a custom enum for handling different enrollment outcomes. The Student and Course classes are clearly connected through a many-to-many relationship, and the enrollment rules are implemented in a simple and understandable way. However, the code could be improved by adding validation for null values, making method names and visibility more consistent.

The important rules are clearly tested:

add student successfully
student already in course
course is full
remove student successfully
remove student that is not in course

The ToString() method defines how an object is shown as text. In this project, it is used so that a student or course is displayed in a clear and readable way.

When we write Console.WriteLine(student); or Console.WriteLine(course);, C# automatically calls ToString(). This makes the output easier to understand without writing extra formatting code.