namespace apbd_cw3_s33296.domain;

public class Rental
{
    public string Id { get; }
    public User User { get; }
    public Equipment Equipment { get; }
    public DateTime BorrowedAt { get; }
    public DateTime DueDate { get; }
    public DateTime? ReturnedAt { get; private set; }
    public decimal LateFee { get; private set; }

    public bool IsActive => ReturnedAt is null;

    public bool WasReturnedOnTime =>
        ReturnedAt.HasValue && ReturnedAt.Value.Date <= DueDate.Date;

    public Rental(string id, User user, Equipment equipment, DateTime borrowedAt, DateTime dueDate)
    {
        Id = id;
        User = user;
        Equipment = equipment;
        BorrowedAt = borrowedAt.Date;
        DueDate = dueDate.Date;
    }

    public bool IsOverdue(DateTime today)
        => IsActive && DueDate.Date < today.Date;

    public void CompleteReturn(DateTime returnDate, decimal lateFee)
    {
        if (!IsActive)
            throw new InvalidOperationException("To wypożyczenie zostało już zakończone.");

        ReturnedAt = returnDate.Date;
        LateFee = lateFee;
    }

    public override string ToString()
    {
        var status = IsActive ? "Aktywne" : $"Zwrócone: {ReturnedAt:yyyy-MM-dd}, kara: {LateFee:C}";

        return $"{Id} | Użytkownik: {User.Name} {User.LastName} | Sprzęt: {Equipment.Name} | " + $"Od: {BorrowedAt:yyyy-MM-dd} | Termin: {DueDate:yyyy-MM-dd} | {status}";
    }
}