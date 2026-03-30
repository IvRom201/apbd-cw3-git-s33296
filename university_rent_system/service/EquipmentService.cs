using System;
using System.Collections.Generic;
using System.Linq;
using apbd_cw3_s33296.domain;
using apbd_cw3_s33296.repo;

namespace apbd_cw3_s33296.service;

public class EquipmentService
{
    private readonly IEquipmentRepo equipmentRepo;
    private readonly IIdGenerator idGenerator;

    public EquipmentService(IEquipmentRepo equipmentRepo, IIdGenerator idGenerator)
    {
        this.equipmentRepo = equipmentRepo;
        this.idGenerator = idGenerator;
    }

    public Laptop AddLaptop(string name, string producent, string processor, int ramGb, bool hasDedicatedGpu)
    {
        var laptop = new Laptop(idGenerator.NextEquipmentId(), name, producent, processor, ramGb, hasDedicatedGpu);
        equipmentRepo.Add(laptop);
        return laptop;
    }

    public Projector AddProjector(string name, string producent, string resolution, int lumens)
    {
        var projector = new Projector(idGenerator.NextEquipmentId(), name, producent, resolution, lumens);
        equipmentRepo.Add(projector);
        return projector;
    }

    public Camera AddCamera(string name, string producent, int megapixels, string lensMount)
    {
        var camera = new Camera(idGenerator.NextEquipmentId(), name, producent, megapixels, lensMount);
        equipmentRepo.Add(camera);
        return camera;
    }

    public IReadOnlyList<Equipment> GetAllEquipment() => equipmentRepo.GetAll();

    public IReadOnlyList<Equipment> GetAvailableEquipment()
        => equipmentRepo.GetAll()
            .Where(e => e.Status == EquipmentStatus.Available)
            .ToList();

    public OperationResult MarkAsUnavailable(int equipmentId, string reason)
    {
        var equipment = equipmentRepo.GetById(equipmentId);
        if (equipment is null)
            return OperationResult.Fail("Nie znaleziono sprzętu.");

        try
        {
            equipment.Unavailable(reason);
            return OperationResult.Ok($"Sprzęt {equipment.Id} oznaczono jako niedostępny.");
        }
        catch (InvalidOperationException ex)
        {
            return OperationResult.Fail(ex.Message);
        }
    }
}