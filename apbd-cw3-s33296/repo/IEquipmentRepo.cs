using System.Collections.Generic;
using apbd_cw3_s33296.domain;

namespace apbd_cw3_s33296.repo;

public interface IEquipmentRepo
{
    void Add(Equipment equipment);
    Equipment? GetById(int id);
    IReadOnlyList<Equipment> GetAll();
}