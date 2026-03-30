using System.Collections.Generic;
using apbd_cw3_s33296.domain;

namespace apbd_cw3_s33296.repo;

public interface IUserRepo
{
    void Add(User user);
    User? GetById(int id);
    IReadOnlyList<User> GetAll();
}