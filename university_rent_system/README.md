# University Equipment Rental

## Opis projektu
Aplikacja konsolowa w C# do obsługi uczelnianej wypożyczalni sprzętu.

System umożliwia:
- rejestrowanie użytkowników,
- dodawanie różnych typów sprzętu,
- wypożyczanie i zwroty,
- oznaczanie sprzętu jako niedostępnego,
- kontrolę dostępności,
- wyświetlanie aktywnych i przeterminowanych wypożyczeń,
- generowanie raportu podsumowującego.

Program działa w trybie **menu tekstowego**, dzięki czemu użytkownik może samodzielnie wykonywać operacje w konsoli. Dodatkowo dostępna jest opcja załadowania danych demonstracyjnych, co ułatwia szybkie pokazanie działania systemu.

---

## Zakres funkcjonalny
System wspiera następujące operacje:
- dodawanie użytkowników (`Student`, `Employee`),
- dodawanie sprzętu (`Laptop`, `Projector`, `Camera`),
- wyświetlanie całej listy sprzętu,
- wyświetlanie tylko sprzętu dostępnego,
- wypożyczanie sprzętu użytkownikowi,
- zwrot sprzętu wraz z naliczeniem ewentualnej kary,
- oznaczanie sprzętu jako niedostępnego,
- wyświetlanie aktywnych wypożyczeń danego użytkownika,
- wyświetlanie przeterminowanych wypożyczeń,
- generowanie raportu końcowego,
- uruchomienie danych demonstracyjnych z poziomu menu.

---

## Struktura projektu
Projekt został podzielony na kilka obszarów odpowiedzialności:

- **Domain** – klasy domenowe opisujące obiekty systemu (`Equipment`, `User`, `Rental` itd.)
- **Repo** – przechowywanie danych w pamięci
- **Service** – logika biznesowa aplikacji
- **Policies** – reguły biznesowe, które mogą się zmieniać, np. limity wypożyczeń i sposób naliczania kar
- **Program.cs** – warstwa interfejsu konsolowego i obsługa menu tekstowego

Taki podział został wybrany po to, aby oddzielić:
1. model domenowy,
2. logikę operacji,
3. interfejs użytkownika.

Dzięki temu kod jest czytelniejszy i łatwiejszy do rozwijania.

---

## Decyzje projektowe

### 1. Rozdzielenie odpowiedzialności
Logika biznesowa nie została umieszczona w `Program.cs`.  
Plik `Program.cs` odpowiada jedynie za:
- wyświetlanie menu,
- odczyt danych z konsoli,
- wywoływanie odpowiednich metod serwisów.

Operacje biznesowe, takie jak wypożyczenie, zwrot, sprawdzanie limitów czy naliczanie kary, są wykonywane w klasach serwisowych i politykach.

### 2. Dziedziczenie tylko tam, gdzie wynika z domeny
Dziedziczenie zostało użyte tylko tam, gdzie rzeczywiście opisuje relację typu „jest rodzajem”:
- `Laptop`, `Projector`, `Camera` dziedziczą po `Equipment`,
- `Student`, `Employee` dziedziczą po `User`.

Nie stosowałem dziedziczenia sztucznie tylko po to, żeby rozwiązanie wyglądało bardziej „obiektowo”.

### 3. Reguły biznesowe skupione w jednym miejscu
Limity użytkowników i kara za opóźnienie zostały umieszczone w `DefaultRentalPolicy`.  
Dzięki temu te reguły nie są rozproszone po wielu klasach i można je łatwo zmienić w jednym miejscu.

### 4. Repozytoria jako osobna warstwa
Dane są przechowywane w repozytoriach in-memory.  
Serwisy korzystają z interfejsów repozytoriów, a nie bezpośrednio z konkretnych implementacji.  
Pozwala to w przyszłości łatwo zamienić przechowywanie danych np. na plik JSON albo bazę danych.

---

## Kohezja
W projekcie starałem się zachować sensowną kohezję, czyli aby każda klasa miała jedną główną odpowiedzialność:

- `EquipmentService` – zarządzanie sprzętem,
- `UserService` – dodawanie i pobieranie użytkowników,
- `RentalService` – wypożyczenia i zwroty,
- `ReportingService` – generowanie raportów,
- `DefaultRentalPolicy` – limity i kara za opóźnienie,
- `Program.cs` – obsługa interfejsu tekstowego.

Dzięki temu klasy nie są przypadkowym zlepkiem metod.

---

## Coupling
Starałem się ograniczyć silne sprzężenie między klasami.

Przykładowo:
- `RentalService` nie przechowuje danych samodzielnie, tylko korzysta z repozytoriów,
- logika biznesowa zależy od interfejsów (`IUserRepository`, `IEquipmentRepository`, `IRentalRepository`, polityki), a nie od konkretnych klas,
- warstwa konsolowa nie implementuje reguł biznesowych, tylko korzysta z serwisów.

Taki układ ułatwia rozwój projektu i zmniejsza wpływ zmian w jednej części systemu na pozostałe.

---

## Reguły biznesowe
W systemie zaimplementowano następujące reguły:
- student może mieć maksymalnie **2** aktywne wypożyczenia,
- pracownik może mieć maksymalnie **5** aktywnych wypożyczeń,
- sprzęt oznaczony jako niedostępny nie może zostać wypożyczony,
- system blokuje wypożyczenie po przekroczeniu limitu,
- opóźniony zwrot powoduje naliczenie kary według zdefiniowanej reguły.

Reguły te są skupione w `DefaultRentalPolicy`, dzięki czemu można je łatwo modyfikować bez przebudowy całego projektu.

---

## Obsługa błędów
Operacje, które mogą się nie powieść, zwracają czytelny wynik operacji (`OperationResult` / `OperationResult<T>`).  
Dzięki temu błędy, takie jak:
- brak użytkownika,
- brak sprzętu,
- przekroczenie limitu,
- próba wypożyczenia niedostępnego sprzętu,
- próba zwrotu już zakończonego wypożyczenia

są obsługiwane jawnie i w sposób zrozumiały.

---

## Interfejs użytkownika
Program działa w konsoli w formie **menu tekstowego**.

Menu pozwala użytkownikowi wykonywać operacje ręcznie, np.:
- dodać użytkownika,
- dodać sprzęt,
- wypożyczyć sprzęt,
- zwrócić sprzęt,
- sprawdzić raport.

Dodatkowo w menu znajduje się opcja załadowania danych demonstracyjnych, co pozwala szybko pokazać działanie systemu bez ręcznego wpisywania wszystkich danych.

Dzięki temu `Program.cs` pełni rolę warstwy interfejsu użytkownika, a nie centralnego miejsca całej logiki systemu.

---

## Uruchomienie
1. Otwórz projekt jako aplikację konsolową .NET w Visual Studio lub Riderze.
2. Uruchom program.
3. W menu wybierz interesującą operację.
4. Opcjonalnie użyj opcji załadowania danych demonstracyjnych, aby szybciej przetestować działanie systemu.

---

## Podsumowanie
Celem projektu było nie tylko przygotowanie działającej aplikacji, ale także pokazanie rozsądnego podziału odpowiedzialności i świadomych decyzji projektowych.

W projekcie widać próbę zadbania o:
- **kohezję** – klasy mają wyraźne role,
- **coupling** – zależności są ograniczone i oparte na abstrakcjach,
- **czytelność i rozwijalność** – reguły biznesowe oraz warstwy systemu są rozdzielone.