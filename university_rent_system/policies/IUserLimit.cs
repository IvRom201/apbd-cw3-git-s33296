using apbd_cw3_s33296.domain;

namespace apbd_cw3_s33296.policies;

public interface IUserLimit
{
    int GetRentalLimit(User user);
}