# Restoran API

ASP.NET Core Web API za upravljanje narudžbama restorana.
Omogućuje kreiranje narudžbi, praćenje statusa i pregled ukupnih cijena.

---

## Preduvjeti

Prije pokretanja aplikacije potrebno je imati instalirano:

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Express edition)
- [Entity Framework Core CLI](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)

EF Core CLI se instalira jednom naredbom:
```bash
dotnet tool install --global dotnet-ef
```

---

## Postavljanje projekta

### 1. Kloniranje repozitorija

```bash
git clone https://github.com/Vale2404/junior.net-rjesenje.git
cd junior.net-rjesenje/AbySalto.Junior
```

### 2. Konfiguracija baze podataka

U datoteci `appsettings.Developmentjson` postavi connection string za SQL Server:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=AbySalto;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 3. Primjena migracija

Izvršiti migraciju koja će kreirati tablice `Narudzbe` i `StavkeNarudzbe` u bazi:

```bash
dotnet ef database update
```

### 4. Pokretanje aplikacije

```bash
dotnet run --launch-profile http
```

Aplikacija se pokreće na `https://localhost:5074`.

---

## Swagger UI i testiranje

Nakon pokretanja, Swagger dokumentacija dostupna je na:

```
http://localhost:5074
```

Swagger omogućuje testiranje svih funkcionalnosti direktno iz preglednika.

### Kako testirati funkcionalnost u Swaggeru

1. Klikni na funkcionalnost koji želiš testirati (npr. `GET /api/Restaurant/{id}`)
2. Klikni gumb **Try it out** u gornjem desnom kutu
3. Popuni potrebne parametre (npr. `id`)
4. Klikni **Execute**
5. Rezultat se prikazuje u sekciji **Responses** — vidiš status kod, response body i headers

Za `POST` i `PUT` funkcionalnosti Swagger prikazuje i **Request body** editor gdje možeš izmijeniti JSON prije slanja.

---

## Testni podaci

Bazu podataka sam napunio ručno s nekoliko redaka radi testiranja funkcionalnosti.

### Narudžbe (`Narudzbe`)

| Id | ImeKupca     | Status     | DatumNarudzbe   | NacinPlacanja | AdresaDostave       | KontaktBroj | Napomena              | Valuta |
|----|--------------|------------|-----------------|---------------|---------------------|-------------|-----------------------|--------|
| 1  | Ivan Horvat  | Zavrsena   | 1.1.2025.       | Kartica       | Ilica 1, Zagreb     | 0911234567  | Ostavite ispred vrata | EUR    |
| 3  | Marko Kovač  | UPripremi  | 15.2.2025.      | Gotovina      | Rebar 95, Zagreb    | 0921234567  | Extra sir             | EUR    |
| 5  | Ana Marić    | Zavrsena   | 20.3.2025.      | PayPal        | Vukovarska 10, Zgb  | 0951234567  | Brza dostava          | EUR    |
| 6  | Test Korisnik| NaCekanju  | 17.5.2026.      | Kartica       | Ilica 1, Zagreb     | 0911234567  | Bez alergena          | EUR    |

### Stavke narudžbi (`StavkeNarudzbe`)

| Id | Ime             | Kolicina | Cijena | NarudzbaId |
|----|-----------------|----------|--------|------------|
| 1  | Pizza Margherita| 2        | 8,50   | 1          |
| 2  | Coca Cola       | 2        | 2,50   | 1          |
| 5  | Carbonara       | 1        | 10,00  | 3          |
| 6  | Burger          | 1        | 7,00   | 3          |
| 7  | Voda            | 2        | 1,50   | 3          |
| 8  | Pizza Capricosa | 1        | 11,00  | 5          |
| 9  | Wok             | 1        | 9,20   | 5          |
| 11 | Cheesecake      | 2        | 3,80   | 5          |
| 15 | Sprite          | 2        | 2,50   | 5          |
| 16 | Pizza           | 2        | 8,50   | 6          |

### Primjeri za brzo testiranje

- `GET /api/Restaurant/1` — narudžba Ivana Horvata s 2 stavke, ukupno 22,00 EUR
- `GET /api/Restaurant/5` — narudžba Ane Marić s 4 stavke, ukupno 37,60 EUR
- `GET /api/Restaurant?sortPoCijeni=true` — sve narudžbe sortirane po cijeni silazno
- `PUT /api/Restaurant/6/status` s body `1` — promjena statusa Test Korisnika na UPripremi

---

## API funkcionalnosti

### Dohvati sve narudžbe
```
GET /api/restaurant
GET /api/restaurant?sortPoCijeni=true
```
Vraća listu svih narudžbi s pripadajućim stavkama. Opcionalni query
parametar `sortPoCijeni=true` sortira rezultate po ukupnoj cijeni silazno.

---

### Dohvati narudžbu po ID-u
```
GET /api/restaurant/{id}
```
Vraća jednu narudžbu sa svim stavkama ili `404` ako nije pronađena.

---

### Kreiraj novu narudžbu
```
POST /api/restaurant
```
**Body (JSON):**
```json
{
  "imeKupca": "Ivan Horvat",
  "nacinPlacanja": "Gotovina",
  "adresaDostave": "Ilica 1, Zagreb",
  "kontaktBroj": "091-123-4567",
  "napomena": "Bez luka",
  "valuta": "EUR",
  "stavke": [
    {
      "ime": "Pizza Margherita",
      "kolicina": 2,
      "cijena": 9.50
    },
    {
      "ime": "Coca-Cola",
      "kolicina": 2,
      "cijena": 2.00
    }
  ]
}
```
Vraća `201 Created` s kreiranom narudžbom. Status se automatski postavlja
na `NaCekanju`, a datum na trenutno vrijeme.

---

### Ažuriraj status narudžbe
```
PUT /api/restaurant/{id}/status
```
**Body (JSON):**
```json
1
```

Moguće vrijednosti statusa:

| Vrijednost | Opis        |
|------------|-------------|
| `0`        | NaCekanju   |
| `1`        | UPripremi   |
| `2`        | Zavrsena    |

Vraća `200 OK` s porukom i novim statusom.

---

### Dohvati ukupnu cijenu narudžbe
```
GET /api/restaurant/{id}/ukupno
```
Vraća ID, ime kupca, ukupnu cijenu i valutu za narudžbu.

---

## Struktura projekta

```
├── Controllers/
│   └── RestaurantController.cs
├── Infrastructure/
│   └── Database/
│       ├── ApplicationDbContext.cs
│       └── IApplicationDbContext.cs
├── Migrations/
├── Models/
│   ├── Narudzba.cs
│   ├── StavkaNarudzbe.cs
│   └── StatusNarudzbe.cs
└── Program.cs
```

---