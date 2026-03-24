using System.Collections.Generic;
using System.Linq;
using apbd_cw3_s33296.domain;

namespace apbd_cw3_s33296.repo;

public class InMemoryUserRepo : IUserRepo
{
    private readonly List<User> _items = new();

    public void Add(User user) => _items.Add(user);

    public User? GetById(int id) => _items.FirstOrDefault(u => u.Id == id);

    public IReadOnlyList<User> GetAll() => _items;
}