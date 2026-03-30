using System.Collections.Generic;
using System.Linq;
using apbd_cw3_s33296.domain;

namespace apbd_cw3_s33296.repo;

public class InMemoryRentalRepo : IRentalRepo
{
    private readonly List<Rental> items = new();

    public void Add(Rental rental) => items.Add(rental);

    public Rental? GetById(int id) => items.FirstOrDefault(r => r.Id == id);

    public IReadOnlyList<Rental> GetAll() => items;

    public IReadOnlyList<Rental> GetActiveByUser(int userId) => items.Where(r => r.User.Id == userId && r.IsActive).ToList();
}