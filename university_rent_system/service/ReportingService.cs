using System;
using System.Linq;
using System.Text;
using apbd_cw3_s33296.domain;
using apbd_cw3_s33296.repo;

namespace apbd_cw3_s33296.service;

public class ReportingService
{
    private readonly IEquipmentRepo _equipmentRepository;
    private readonly IUserRepo _userRepository;
    private readonly IRentalRepo _rentalRepository;

    public ReportingService(
        IEquipmentRepo equipmentRepository,
        IUserRepo userRepository,
        IRentalRepo rentalRepository)
    {
        _equipmentRepository = equipmentRepository;
        _userRepository = userRepository;
        _rentalRepository = rentalRepository;
    }

    public string GenerateSummary(DateTime today)
    {
        var allEquipment = _equipmentRepository.GetAll();
        var allUsers = _userRepository.GetAll();
        var allRentals = _rentalRepository.GetAll();

        var available = allEquipment.Count(e => e.Status == EquipmentStatus.Available);
        var rented = allEquipment.Count(e => e.Status == EquipmentStatus.Rented);
        var unavailable = allEquipment.Count(e => e.Status == EquipmentStatus.Unavailable);
        var activeRentals = allRentals.Count(r => r.IsActive);
        var overdue = allRentals.Count(r => r.IsOverdue(today));
        var totalLateFees = allRentals.Where(r => !r.IsActive).Sum(r => r.LateFee);

        var sb = new StringBuilder();
        sb.AppendLine("=== RAPORT KOŃCOWY WYPOŻYCZALNI ===");
        sb.AppendLine($"Data raportu: {today:yyyy-MM-dd}");
        sb.AppendLine($"Liczba użytkowników: {allUsers.Count}");
        sb.AppendLine($"Liczba egzemplarzy sprzętu: {allEquipment.Count}");
        sb.AppendLine($"- Dostępny: {available}");
        sb.AppendLine($"- Wypożyczony: {rented}");
        sb.AppendLine($"- Niedostępny: {unavailable}");
        sb.AppendLine($"Aktywne wypożyczenia: {activeRentals}");
        sb.AppendLine($"Przeterminowane wypożyczenia: {overdue}");
        sb.AppendLine($"Suma naliczonych kar: {totalLateFees:C}");

        return sb.ToString();
    }
}