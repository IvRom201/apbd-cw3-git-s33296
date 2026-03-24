using System.Text;
using apbd_cw3_s33296.policies;
using apbd_cw3_s33296.repo;
using apbd_cw3_s33296.service;

namespace apbd_cw3_s33296;
using System;

public class Program
{
    private static UserService _userService = null!;
    private static EquipmentService _equipmentService = null!;
    private static RentalService _rentalService = null!;
    private static ReportingService _reportingService = null!;

    private static bool _demoDataLoaded = false;

    private static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        InitializeApplication();
        RunMenu();
    }

    private static void InitializeApplication()
    {
        var userRepository = new InMemoryUserRepo();
        var equipmentRepository = new InMemoryEquipmentRepo();
        var rentalRepository = new InMemoryRentalRepo();

        var idGenerator = new SequentialIdGenerator();
        var policy = new DefaultRentalPolicy();

        _userService = new UserService(userRepository, idGenerator);
        _equipmentService = new EquipmentService(equipmentRepository, idGenerator);
        _rentalService = new RentalService(
            userRepository,
            equipmentRepository,
            rentalRepository,
            policy,
            policy,
            idGenerator);

        _reportingService = new ReportingService(
            equipmentRepository,
            userRepository,
            rentalRepository);
    }

    private static void RunMenu()
    {
        while (true)
        {
            Console.Clear();
            PrintMenu();

            Console.Write("Wybierz opcję: ");
            var choice = Console.ReadLine()?.Trim();

            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    AddUserMenu();
                    break;
                case "2":
                    AddEquipmentMenu();
                    break;
                case "3":
                    ShowAllUsers();
                    break;
                case "4":
                    ShowAllEquipment();
                    break;
                case "5":
                    ShowAvailableEquipment();
                    break;
                case "6":
                    BorrowEquipment();
                    break;
                case "7":
                    ReturnEquipment();
                    break;
                case "8":
                    MarkEquipmentUnavailable();
                    break;
                case "9":
                    ShowUserActiveRentals();
                    break;
                case "10":
                    ShowOverdueRentals();
                    break;
                case "11":
                    ShowAllRentals();
                    break;
                case "12":
                    ShowReport();
                    break;
                case "13":
                    LoadDemoData();
                    break;
                case "0":
                    Console.WriteLine("Zamykanie programu...");
                    return;
                default:
                    Console.WriteLine("Niepoprawna opcja.");
                    Pause();
                    break;
            }
        }
    }

    private static void PrintMenu()
    {
        Console.WriteLine("==================================================");
        Console.WriteLine(" UCZELNIANA WYPOŻYCZALNIA SPRZĘTU");
        Console.WriteLine("==================================================");
        Console.WriteLine("1. Dodaj użytkownika");
        Console.WriteLine("2. Dodaj sprzęt");
        Console.WriteLine("3. Wyświetl wszystkich użytkowników");
        Console.WriteLine("4. Wyświetl cały sprzęt");
        Console.WriteLine("5. Wyświetl dostępny sprzęt");
        Console.WriteLine("6. Wypożycz sprzęt");
        Console.WriteLine("7. Zwróć sprzęt");
        Console.WriteLine("8. Oznacz sprzęt jako niedostępny");
        Console.WriteLine("9. Pokaż aktywne wypożyczenia użytkownika");
        Console.WriteLine("10. Pokaż przeterminowane wypożyczenia");
        Console.WriteLine("11. Pokaż wszystkie wypożyczenia");
        Console.WriteLine("12. Wygeneruj raport");
        Console.WriteLine("13. Załaduj dane demonstracyjne");
        Console.WriteLine("0. Wyjście");
        Console.WriteLine("==================================================");
    }

    private static void AddUserMenu()
    {
        Console.WriteLine("Dodawanie użytkownika:");
        Console.WriteLine("1. Student");
        Console.WriteLine("2. Employee");
        Console.Write("Wybierz typ: ");

        var type = Console.ReadLine()?.Trim();

        Console.Write("Imię: ");
        var firstName = ReadRequiredText();

        Console.Write("Nazwisko: ");
        var lastName = ReadRequiredText();

        switch (type)
        {
            case "1":
                Console.Write("Nr albumu: ");
                var studentNumber = ReadRequiredText();

                Console.Write("Wydział: ");
                var faculty = ReadRequiredText();

                var student = _userService.AddStudent(firstName, lastName, studentNumber, faculty);
                Console.WriteLine($"Dodano studenta: {student}");
                break;

            case "2":
                Console.Write("Dział: ");
                var department = ReadRequiredText();

                Console.Write("Stanowisko: ");
                var position = ReadRequiredText();

                var employee = _userService.AddEmployee(firstName, lastName, department, position);
                Console.WriteLine($"Dodano pracownika: {employee}");
                break;

            default:
                Console.WriteLine("Niepoprawny typ użytkownika.");
                break;
        }

        Pause();
    }

    private static void AddEquipmentMenu()
    {
        Console.WriteLine("Dodawanie sprzętu:");
        Console.WriteLine("1. Laptop");
        Console.WriteLine("2. Projector");
        Console.WriteLine("3. Camera");
        Console.Write("Wybierz typ: ");

        var type = Console.ReadLine()?.Trim();

        Console.Write("Nazwa: ");
        var name = ReadRequiredText();

        Console.Write("Producent: ");
        var producent = ReadRequiredText();

        switch (type)
        {
            case "1":
                Console.Write("Processor: ");
                var processor = Console.ReadLine();
                
                Console.Write("RAM (GB): ");
                var ramGb = ReadInt();

                Console.Write("Czy ma dedykowane GPU? (t/n): ");
                var hasGpu = ReadBoolYesNo();

                var laptop = _equipmentService.AddLaptop(name, producent, processor, ramGb, hasGpu);
                Console.WriteLine($"Dodano laptop: {laptop}");
                break;

            case "2":
                Console.Write("Rozdzielczość: ");
                var resolution = ReadRequiredText();

                Console.Write("Jasność (lumens): ");
                var lumens = ReadInt();

                var projector = _equipmentService.AddProjector(name, producent, resolution, lumens);
                Console.WriteLine($"Dodano projektor: {projector}");
                break;

            case "3":
                Console.Write("Megapiksele: ");
                var megapixels = ReadInt();

                Console.Write("Mocowanie obiektywu: ");
                var lensMount = ReadRequiredText();

                var camera = _equipmentService.AddCamera(name, producent, megapixels, lensMount);
                Console.WriteLine($"Dodano aparat: {camera}");
                break;

            default:
                Console.WriteLine("Niepoprawny typ sprzętu.");
                break;
        }

        Pause();
    }

    private static void ShowAllUsers()
    {
        var users = _userService.GetAllUsers();

        Console.WriteLine("=== UŻYTKOWNICY ===");
        if (!users.Any())
        {
            Console.WriteLine("Brak użytkowników.");
        }
        else
        {
            foreach (var user in users)
            {
                Console.WriteLine(user);
            }
        }

        Pause();
    }

    private static void ShowAllEquipment()
    {
        var equipment = _equipmentService.GetAllEquipment();

        Console.WriteLine("=== CAŁY SPRZĘT ===");
        if (!equipment.Any())
        {
            Console.WriteLine("Brak sprzętu.");
        }
        else
        {
            foreach (var item in equipment)
            {
                Console.WriteLine(item);
            }
        }

        Pause();
    }

    private static void ShowAvailableEquipment()
    {
        var equipment = _equipmentService.GetAvailableEquipment();

        Console.WriteLine("=== DOSTĘPNY SPRZĘT ===");
        if (!equipment.Any())
        {
            Console.WriteLine("Brak dostępnego sprzętu.");
        }
        else
        {
            foreach (var item in equipment)
            {
                Console.WriteLine(item);
            }
        }

        Pause();
    }

    private static void BorrowEquipment()
    {
        Console.WriteLine("=== WYPOŻYCZENIE SPRZĘTU ===");

        ShowUsersInline();
        Console.Write("Podaj ID użytkownika: ");
        var userId = ReadInt();

        ShowAvailableEquipmentInline();
        Console.Write("Podaj ID sprzętu: ");
        var equipmentId = ReadInt();

        Console.Write("Data wypożyczenia (rrrr-mm-dd, Enter = dziś): ");
        var borrowedAt = ReadDateOrDefault(DateTime.Today);

        Console.Write("Na ile dni wypożyczyć?: ");
        var days = ReadInt();

        var result = _rentalService.BorrowEquipment(userId, equipmentId, borrowedAt, days);
        Console.WriteLine(result.Message);

        if (result.Success && result.Value is not null)
        {
            Console.WriteLine(result.Value);
        }

        Pause();
    }

    private static void ReturnEquipment()
    {
        Console.WriteLine("=== ZWROT SPRZĘTU ===");

        ShowAllRentalsInline();
        Console.Write("Podaj ID wypożyczenia: ");
        var rentalId = ReadInt();

        Console.Write("Data zwrotu (rrrr-mm-dd, Enter = dziś): ");
        var returnDate = ReadDateOrDefault(DateTime.Today);

        var result = _rentalService.ReturnEquipment(rentalId, returnDate);
        Console.WriteLine(result.Message);

        if (result.Success && result.Value is not null)
        {
            Console.WriteLine(result.Value);
        }

        Pause();
    }

    private static void MarkEquipmentUnavailable()
    {
        Console.WriteLine("=== OZNACZANIE SPRZĘTU JAKO NIEDOSTĘPNEGO ===");

        ShowAllEquipmentInline();
        Console.Write("Podaj ID sprzętu: ");
        var equipmentId = ReadInt();

        Console.Write("Powód niedostępności: ");
        var reason = ReadRequiredText();

        var result = _equipmentService.MarkAsUnavailable(equipmentId, reason);
        Console.WriteLine(result.Message);

        Pause();
    }

    private static void ShowUserActiveRentals()
    {
        Console.WriteLine("=== AKTYWNE WYPOŻYCZENIA UŻYTKOWNIKA ===");

        ShowUsersInline();
        Console.Write("Podaj ID użytkownika: ");
        var userId = ReadInt();

        var rentals = _rentalService.GetActiveRentalsForUser(userId);

        if (!rentals.Any())
        {
            Console.WriteLine("Brak aktywnych wypożyczeń.");
        }
        else
        {
            foreach (var rental in rentals)
            {
                Console.WriteLine(rental);
            }
        }

        Pause();
    }

    private static void ShowOverdueRentals()
    {
        Console.WriteLine("=== PRZETERMINOWANE WYPOŻYCZENIA ===");
        Console.Write("Data odniesienia (rrrr-mm-dd, Enter = dziś): ");
        var today = ReadDateOrDefault(DateTime.Today);

        var rentals = _rentalService.GetOverdueRentals(today);

        if (!rentals.Any())
        {
            Console.WriteLine("Brak przeterminowanych wypożyczeń.");
        }
        else
        {
            foreach (var rental in rentals)
            {
                Console.WriteLine(rental);
            }
        }

        Pause();
    }

    private static void ShowAllRentals()
    {
        Console.WriteLine("=== WSZYSTKIE WYPOŻYCZENIA ===");
        var rentals = _rentalService.GetAllRentals();

        if (!rentals.Any())
        {
            Console.WriteLine("Brak wypożyczeń.");
        }
        else
        {
            foreach (var rental in rentals)
            {
                Console.WriteLine(rental);
            }
        }

        Pause();
    }

    private static void ShowReport()
    {
        Console.WriteLine("=== RAPORT ===");
        Console.Write("Data raportu (rrrr-mm-dd, Enter = dziś): ");
        var date = ReadDateOrDefault(DateTime.Today);

        Console.WriteLine(_reportingService.GenerateSummary(date));
        Pause();
    }

    private static void LoadDemoData()
    {
        if (_demoDataLoaded)
        {
            Console.WriteLine("Dane demonstracyjne zostały już wcześniej załadowane.");
            Pause();
            return;
        }

        var student1 = _userService.AddStudent("Jan", "Kowalski", "s12345", "Informatyka");
        var student2 = _userService.AddStudent("Anna", "Nowak", "s54321", "Matematyka");
        var employee1 = _userService.AddEmployee("Piotr", "Wiśniewski", "IT", "Administrator");

        var laptop1 = _equipmentService.AddLaptop("Dell Latitude", "IntelCoreI5", "Dell", 16, false);
        var laptop2 = _equipmentService.AddLaptop("Lenovo ThinkPad", "Ryzen5600", "Lenovo", 32, true);
        var projector1 = _equipmentService.AddProjector("Epson X200", "Epson", "Full HD", 3200);
        var camera1 = _equipmentService.AddCamera("Canon EOS R50", "Canon", 24, "RF");

        _equipmentService.MarkAsUnavailable(camera1.Id, "Serwis obiektywu");

        var r1 = _rentalService.BorrowEquipment(student1.Id, laptop1.Id, new DateTime(2026, 3, 1), 7);
        var r2 = _rentalService.BorrowEquipment(student1.Id, projector1.Id, new DateTime(2026, 3, 1), 3);
        _rentalService.BorrowEquipment(employee1.Id, laptop2.Id, new DateTime(2026, 3, 5), 2);

        if (r1.Success && r1.Value is not null)
            _rentalService.ReturnEquipment(r1.Value.Id, new DateTime(2026, 3, 5));

        if (r2.Success && r2.Value is not null)
            _rentalService.ReturnEquipment(r2.Value.Id, new DateTime(2026, 3, 7));

        _demoDataLoaded = true;

        Console.WriteLine("Załadowano dane demonstracyjne.");
        Pause();
    }

    private static void ShowUsersInline()
    {
        var users = _userService.GetAllUsers();
        Console.WriteLine("--- Użytkownicy ---");

        if (!users.Any())
        {
            Console.WriteLine("Brak użytkowników.");
            return;
        }

        foreach (var user in users)
        {
            Console.WriteLine(user);
        }

        Console.WriteLine();
    }

    private static void ShowAllEquipmentInline()
    {
        var equipment = _equipmentService.GetAllEquipment();
        Console.WriteLine("--- Sprzęt ---");

        if (!equipment.Any())
        {
            Console.WriteLine("Brak sprzętu.");
            return;
        }

        foreach (var item in equipment)
        {
            Console.WriteLine(item);
        }

        Console.WriteLine();
    }

    private static void ShowAvailableEquipmentInline()
    {
        var equipment = _equipmentService.GetAvailableEquipment();
        Console.WriteLine("--- Dostępny sprzęt ---");

        if (!equipment.Any())
        {
            Console.WriteLine("Brak dostępnego sprzętu.");
            return;
        }

        foreach (var item in equipment)
        {
            Console.WriteLine(item);
        }

        Console.WriteLine();
    }

    private static void ShowAllRentalsInline()
    {
        var rentals = _rentalService.GetAllRentals();
        Console.WriteLine("--- Wypożyczenia ---");

        if (!rentals.Any())
        {
            Console.WriteLine("Brak wypożyczeń.");
            return;
        }

        foreach (var rental in rentals)
        {
            Console.WriteLine(rental);
        }

        Console.WriteLine();
    }

    private static string ReadRequiredText()
    {
        while (true)
        {
            var input = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(input))
                return input;

            Console.Write("Pole nie może być puste. Wpisz ponownie: ");
        }
    }

    private static int ReadInt()
    {
        while (true)
        {
            var input = Console.ReadLine()?.Trim();
            if (int.TryParse(input, out var value) && value > 0)
                return value;

            Console.Write("Podaj poprawną liczbę całkowitą > 0: ");
        }
    }

    private static bool ReadBoolYesNo()
    {
        while (true)
        {
            var input = Console.ReadLine()?.Trim().ToLower();

            if (input == "t" || input == "tak" || input == "y" || input == "yes")
                return true;

            if (input == "n" || input == "nie" || input == "no")
                return false;

            Console.Write("Wpisz 't' albo 'n': ");
        }
    }

    private static DateTime ReadDateOrDefault(DateTime defaultValue)
    {
        while (true)
        {
            var input = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
                return defaultValue;

            if (DateTime.TryParse(input, out var date))
                return date.Date;

            Console.Write("Niepoprawna data. Użyj formatu rrrr-mm-dd albo Enter: ");
        }
    }

    private static void Pause()
    {
        Console.WriteLine();
        Console.WriteLine("Naciśnij Enter, aby kontynuować...");
        Console.ReadLine();
    }
}