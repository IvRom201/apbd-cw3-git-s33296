using System;

namespace apbd_cw3_s33296.policies;

public interface ILateFee
{
    decimal CalculateLateFee(DateTime dueDate, DateTime returnDate);
}