namespace apbd_cw3_s33296.service;

public class SequentialIdGenerator : IIdGenerator
{
    private int equipmentCounter = 1;
    private int userCounter = 1;
    private int rentalCounter = 1;

    public int NextEquipmentId() => equipmentCounter++;
    public int NextUserId() => userCounter++;
    public int NextRentalId() => rentalCounter++;
}