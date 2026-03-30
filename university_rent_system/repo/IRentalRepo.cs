using System.Collections.Generic;
using apbd_cw3_s33296.domain;

namespace apbd_cw3_s33296.repo;

public interface IRentalRepo
{
    void Add(Rental rental);
    Rental? GetById(int id);
    IReadOnlyList<Rental> GetAll();
    IReadOnlyList<Rental> GetActiveByUser(int userId);
}