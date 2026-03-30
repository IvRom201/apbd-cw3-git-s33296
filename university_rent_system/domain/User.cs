namespace apbd_cw3_s33296.domain;

public class User
{
    public int Id { get; }
    public string Name { get; }
    public string LastName { get; }

    protected User(int id, string name, string lastName)
    {
        Id = id;
        Name = name;
        LastName = lastName;
    }

    public override string ToString() => $"{Id} | {Name} {LastName}";
}