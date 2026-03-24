using System.Collections.Generic;
using System.Linq;
using apbd_cw3_s33296.domain;

namespace apbd_cw3_s33296.repo;

public class InMemoryEquipmentRepo : IEquipmentRepo
{
    private readonly List<Equipment> items = new();

    public void Add(Equipment equipment) => items.Add(equipment);

    public Equipment? GetById(int id) => items.FirstOrDefault(e => e.Id == id);

    public IReadOnlyList<Equipment> GetAll() => items;
}