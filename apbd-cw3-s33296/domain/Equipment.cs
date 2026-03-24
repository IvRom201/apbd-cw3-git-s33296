namespace apbd_cw3_s33296.domain;

public abstract class Equipment
{
    public int Id { get; }
    public string Name { get; }
    public string Producent { get; }
    public EquipmentStatus Status { get; set; }
    public string? UnavailableReason { get; set; }

    protected Equipment(int id, string name, string producent)
    {
        Id = id;
        Name = name;
        Producent = producent;
        Status = EquipmentStatus.Available;
    }

    public void Rent()
    {
        if (Status != EquipmentStatus.Available) throw new InvalidOperationException("Sprzęt nie jest dostępny do wypożyczenia");

        Status = EquipmentStatus.Rented;
        UnavailableReason = null;
    }

    public void Return()
    {
        Status = EquipmentStatus.Available;
        UnavailableReason = null;
    }

    public void Unavailable(string reason)
    {
        if (Status == EquipmentStatus.Rented)
            throw new InvalidOperationException("Nie można oznaczyć jako niedostępny sprzętu aktualnie wypożyczonego.");

        Status = EquipmentStatus.Unavailable;
        UnavailableReason = reason;
    }
    
    public override string ToString()
    {
        var reasonPart = UnavailableReason is null ? "" : $" | Powód: {UnavailableReason}";
        return $"{Id} | {Name} | {Producent} | Status: {Status} | {GetSpecificDetails()}{reasonPart}";
    }

    protected abstract string GetSpecificDetails();
}