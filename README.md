# 📚 Ksiegarnia – system wypożyczania książek

Projekt **Ksiegarnia** to aplikacja webowa typu CRUD, stworzona w technologii **ASP.NET Core MVC** z wykorzystaniem **Entity Framework Core** oraz **ASP.NET Identity**.  
System symuluje działanie uproszczonej biblioteki / wypożyczalni książek, umożliwiając zarządzanie książkami, użytkownikami oraz procesem wypożyczania.

Projekt został przygotowany w celach edukacyjnych jako realizacja projektu zaliczeniowego z przedmiotu **Techniki Internetu**.

---

## 🎯 Cel projektu

Celem projektu było:

- zaprojektowanie i implementacja **spójnego systemu domenowego**,
- praktyczne wykorzystanie **ASP.NET Core MVC**,
- obsługa **uwierzytelniania i autoryzacji użytkowników**,
- zaprojektowanie relacyjnej bazy danych,
- rozdzielenie logiki aplikacji na warstwy (Controller / Service / Repository),
- implementacja realnego procesu biznesowego (wypożyczanie książek).

---

## 🧱 Architektura aplikacji

Projekt oparty jest o klasyczną architekturę warstwową:

- **Controllers** – obsługa żądań HTTP i nawigacji
- **Services** – logika biznesowa aplikacji
- **Repositories** – dostęp do bazy danych (EF Core)
- **Models** – encje bazodanowe
- **ViewModels** – modele widoków (DTO)
- **Mappers** – mapowanie pomiędzy Model ↔ ViewModel
- **Views** – warstwa prezentacji (Razor)

Dzięki temu aplikacja jest:
- czytelna,
- testowalna,
- łatwa do rozbudowy.

---

## 👤 Role użytkowników

System obsługuje role oparte o **ASP.NET Identity**:

- **User**
  - przeglądanie książek i pozostałych tabel z nimi związanych,
  - wypożyczanie książek,
  - przegląd własnych wypożyczeń (aktywnych i historycznych),
  - zarządzanie własnym profilem.

- **Editor**
  - zarządzanie książkami i pozostałymi tabelami z nimi związanymi (CRUD),
  - zarządzanie wypożyczeniami (CRUD).
  - zwracanie wypożyczeń

- **Admin**
  - pełne uprawnienia,
  - zarządzanie użytkownikami,
  - zarządzanie rolami,

Autoryzacja realizowana jest za pomocą **Policies** (`AdminOnly`, `AdminOrEditor`, `All`).

Widoki w aplikacji (UI) renderowane są także zależnie od roli, aby chować elementy, do których aktualny użytkownik **nie ma dostępu**

---

## 🔐 Uwierzytelnianie

Aplikacja wykorzystuje:

### **ASP.NET Identity (Cookie-based authentication)**, dzięki któremu możliwe są:
- rejestracja lokalna (email + hasło + potwierdzanie hasła),
- logowanie lokalne (email + hasło),
- logowanie zewnętrzne przez **Google OAuth**,
- resetowanie hasła 

---

## 📖 Wypożyczanie książek – logika domenowa

- książka może mieć **wiele wypożyczeń historycznych**,
- **tylko jedno aktywne wypożyczenie** w danym momencie,
- dostępność książki **nie jest zapisywana w tabeli Books**,
- status książki obliczany jest dynamicznie na podstawie aktywnych wypożyczeń.

Takie podejście:
- eliminuje niespójności danych,
- umożliwia pełną historię wypożyczeń,
- odzwierciedla realny system biblioteczny.

---

## 🗄️ Baza danych

- Relacyjna baza danych **PostgreSQL**,
- migracje wykonywane przez **Entity Framework Core**,
- relacje:
- `Book` One — Many `Loan`
- `User` One — Many `Loan`
- `Author` One - Many `Book`
- `Isbn` One - One `Book`
- `Category` One - Many `Book`  

Dane startowe:
- role systemowe seedowane automatycznie,
- konto administratora tworzone na podstawie konfiguracji środowiska.

---

## ⚙️ Konfiguracja

Dane wrażliwe (np.:
- connection string bazy danych,
- dane administratora,
- Google OAuth ClientId / ClientSecret)

przechowywane są w:
- **User Secrets** (środowisko lokalne),
- **zmiennych środowiskowych** (produkcja).

Projekt jest gotowy do uruchomienia:
- lokalnie,
- w kontenerze Docker,
- na zewnętrznym hostingu (np. Render).

---

## 🚀 Technologie

- ASP.NET Core MVC
- Entity Framework Core
- PostgreSQL
- ASP.NET Identity
- Razor Pages
- Bootstrap + własne style CSS
- Docker

---

## 📌 Status projektu

Projekt jest:
- kompletny funkcjonalnie,
- stabilny,
- przygotowany do wdrożenia,
- możliwy do dalszej rozbudowy (np. płatności, rezerwacje).

---

## 📄 Licencja

Projekt edukacyjny – do użytku akademickiego.
