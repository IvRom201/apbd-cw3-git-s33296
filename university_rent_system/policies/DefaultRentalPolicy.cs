using System;
using apbd_cw3_s33296.domain;

namespace apbd_cw3_s33296.policies;

public class DefaultRentalPolicy : IUserLimit, ILateFee
{
    private const int StudentLimit = 2;
    private const int EmployeeLimit = 5;
    private const decimal FeePerLateDay = 15m;

    public int GetRentalLimit(User user)
    {
        return user switch
        {
            Student => StudentLimit,
            Employee => EmployeeLimit,
            _ => throw new InvalidOperationException("Nieznany typ użytkownika")
        };
    }

    public decimal CalculateLateFee(DateTime dueDate, DateTime returnDate)
    {
        var lateDays = (returnDate.Date - dueDate.Date).Days;
        return lateDays > 0 ? lateDays * FeePerLateDay : 0m;
    }
}