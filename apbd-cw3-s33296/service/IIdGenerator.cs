namespace apbd_cw3_s33296.service;

public interface IIdGenerator
{
    int NextEquipmentId();
    int NextUserId();
    int NextRentalId();
}