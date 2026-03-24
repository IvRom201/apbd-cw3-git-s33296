using System;
using System.Collections.Generic;
using System.Linq;
using apbd_cw3_s33296.domain;
using apbd_cw3_s33296.policies;
using apbd_cw3_s33296.repo;

namespace apbd_cw3_s33296.service;

public class RentalService
{
    private readonly IUserRepo userRepository;
    private readonly IEquipmentRepo equipmentRepository;
    private readonly IRentalRepo rentalRepository;
    private readonly IUserLimit userLimitPolicy;
    private readonly ILateFee lateFeePolicy;
    private readonly IIdGenerator idGenerator;

    public RentalService(
        IUserRepo userRepository,
        IEquipmentRepo equipmentRepository,
        IRentalRepo rentalRepository,
        IUserLimit userLimitPolicy,
        ILateFee lateFeePolicy,
        IIdGenerator idGenerator)
    {
        this.userRepository = userRepository;
        this.equipmentRepository = equipmentRepository;
        this.rentalRepository = rentalRepository;
        this.userLimitPolicy = userLimitPolicy;
        this.lateFeePolicy = lateFeePolicy;
        this.idGenerator = idGenerator;
    }

    public OperationResult<Rental> BorrowEquipment(int userId, int equipmentId, DateTime borrowedAt, int days)
    {
        if (days <= 0)
            return OperationResult<Rental>.Fail("Liczba dni wypożyczenia musi być większa od zera");

        var user = userRepository.GetById(userId);
        if (user is null)
            return OperationResult<Rental>.Fail("Nie znaleziono użytkownika");

        var equipment = equipmentRepository.GetById(equipmentId);
        if (equipment is null)
            return OperationResult<Rental>.Fail("Nie znaleziono sprzętu");

        if (equipment.Status != EquipmentStatus.Available)
            return OperationResult<Rental>.Fail("Sprzęt nie jest dostępny do wypożyczenia");

        var activeRentals = rentalRepository.GetActiveByUser(userId).Count;
        var limit = userLimitPolicy.GetRentalLimit(user);

        if (activeRentals >= limit)
            return OperationResult<Rental>.Fail($"Użytkownik przekroczył limit aktywnych wypożyczeń ({limit})");

        try
        {
            equipment.Rent();

            var rental = new Rental(
                idGenerator.NextRentalId(),
                user,
                equipment,
                borrowedAt.Date,
                borrowedAt.Date.AddDays(days));

            rentalRepository.Add(rental);

            return OperationResult<Rental>.Ok(
                rental,
                $"Wypożyczono sprzęt {equipment.Id} użytkownikowi {user.Name} {user.LastName} do {rental.DueDate:yyyy-MM-dd}.");
        }
        catch (InvalidOperationException ex)
        {
            return OperationResult<Rental>.Fail(ex.Message);
        }
    }

    public OperationResult<Rental> ReturnEquipment(int rentalId, DateTime returnDate)
    {
        var rental = rentalRepository.GetById(rentalId);
        if (rental is null)
            return OperationResult<Rental>.Fail("Nie znaleziono wypożyczenia.");

        if (!rental.IsActive)
            return OperationResult<Rental>.Fail("To wypożyczenie jest już zakończone.");

        var fee = lateFeePolicy.CalculateLateFee(rental.DueDate, returnDate.Date);

        try
        {
            rental.CompleteReturn(returnDate.Date, fee);
            rental.Equipment.Return();

            var message = fee > 0
                ? $"Zwrot z opóźnieniem. Naliczona kara: {fee:C}."
                : "Zwrot terminowy. Kara nie została naliczona.";

            return OperationResult<Rental>.Ok(rental, message);
        }
        catch (InvalidOperationException ex)
        {
            return OperationResult<Rental>.Fail(ex.Message);
        }
    }

    public IReadOnlyList<Rental> GetActiveRentalsForUser(int userId)
        => rentalRepository.GetActiveByUser(userId);

    public IReadOnlyList<Rental> GetOverdueRentals(DateTime today)
        => rentalRepository.GetAll()
            .Where(r => r.IsOverdue(today))
            .ToList();

    public IReadOnlyList<Rental> GetAllRentals() => rentalRepository.GetAll();
}