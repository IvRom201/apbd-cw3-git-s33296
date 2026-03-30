namespace apbd_cw3_s33296.domain;

public class Student : User
{
    public string SNumber { get; }
    public string Faculty { get; }

    public Student(int id, string name, string lastName, string sNumber, string faculty) : base(id, name, lastName)
    {
        SNumber = sNumber;
        Faculty = faculty;
    }
    
    public override string ToString() => $" {base.ToString()} | Student | Nr albumu: {SNumber} | Wydział: {Faculty}";
}
