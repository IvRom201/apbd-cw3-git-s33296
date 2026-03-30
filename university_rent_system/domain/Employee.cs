namespace apbd_cw3_s33296.domain;

public class Employee : User
{
    public string Department { get; }
    public string Position { get; }

    public Employee(int id, string name, string lastName, string department, string position)
        : base(id, name, lastName)
    {
        Department = department;
        Position = position;
    }
    
    public override string ToString() => $"{base.ToString()} | Pracownik | Dział: {Department} | Stanowisko: {Position}";
}