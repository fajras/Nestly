# 👶🌸 Nestly – Pregnancy & Baby Tracking Information System

Nestly je informacioni sistem namijenjen trudnicama i roditeljima koji omogućava praćenje trudnoće, razvoja bebe, zdravstvenih unosa, podsjetnika i komunikacije kroz savremenu desktop i mobilnu aplikaciju.

---

## 🏗️ Arhitektura sistema

Sistem se sastoji od:

- 🌐 ASP.NET Core Web API backend
- 🐇 RabbitMQ mikroservis
- 🗄️ MSSQL baza podataka
- 📱 Flutter mobilna aplikacija (Android)
- 💻 Flutter desktop aplikacija (Windows)

---

# 🚀 Upute za pokretanje projekta

## 🔹 1. Preduslovi

Potrebno je instalirati:

- Docker Desktop
- .NET 8 SDK
- Flutter SDK
- Android Studio (za emulator)
- Git

Provjera instalacije:

```bash
docker --version
dotnet --version
flutter --version
```

---

# 🐳 Backend Setup

1. Klonirati repozitorij.
2. 
3. Otvoriti backend folder:
pronaći .env folder, extractati ga  uz pomoć lozinke
```bash
cd Nestly-backend
```

3. Pokrenuti Docker servise:
U Nestly-backend folderu prije pokretanja projekta provjeriti da li postoje već aktivni Docker kontejneri:
```bash
docker ps -a
```
Ukoliko postoje drugi projekti, potrebno ih je zaustaviti prije pokretanja Nestly projekta sa sljedeće dvije komande:
```bash
docker stop <container_name>
```
```bash
docker rm <container_name>
```
Za potpuno čisto pokretanje izvršiti u Nestly-backend folderu:
```bash
docker-compose down
```
```bash
docker-compose build --no-cache
```
```bash
docker-compose up -d
```

Ovom komandom se pokreću:

- MSSQL Server baza
- RabbitMQ server
- Nestly Web API
- Nestly Worker servis

Sačekati da se svi servisi uspješno pokrenu.
```bash
docker ps
```
Svi Nestly servisi trebaju biti u statusu Up.

4. Provjera backend-a:

Otvoriti u browseru:

```
http://localhost:5167/swagger
```

Ako se Swagger UI prikaže, backend je uspješno pokrenut.

---

# 💻 Pokretanje Windows Desktop aplikacije

U folderu `Windows_Build` (u root folderu projekta) pokrenuti:

```
flutter_application_nestly.exe
```

### 🔐 Kredencijali za prijavu (Desktop)

- Email: doctor@nestly.com
- Lozinka: test

---

# 📱 Pokretanje mobilne aplikacije (Android)

U root folderu projekta locirati fajl:

```
app-release.apk
```

Instalirati APK na emulator ili fizički uređaj.

⚠️ Deinstalirati staru verziju aplikacije ukoliko je već instalirana.

### 🔐 Kredencijali za prijavu (Mobile)

- Email: parent@nestly.com
- Lozinka: test

---

# 🔧 Mikroservis funkcionalnosti

Nestly koristi RabbitMQ mikroservis za:

- 📅 Automatske dnevne podsjetnike
- 💊 Podsjetnike za terapije
- 👶 Kalendar obavijesti za termine
- 💬 Notifikacije za chat
- ❓ Notifikacije za postavljeno i odgovoreno pitanje

---

# 🛠️ Tehnologije

## 🌐 Backend

- ASP.NET Core (.NET 8)
- Entity Framework Core
- MSSQL Server
- SignalR
- JWT autentifikacija
- RabbitMQ
- Azure Blob Storage

## 📱 Frontend

- Flutter
- HTTP client
- Local token storage

## 🐳 DevOps

- Docker
- Docker Compose

---

# 🎓 Akademski kontekst

Projekt je razvijen u sklopu predmeta **Razvoj softvera 2** na Fakultetu informacijskih tehnologija.
