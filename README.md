# ✨ Quote Management API Project

A full-stack application that lets users manage quotes, tag them, and view top-liked quotes. Built with **ASP.NET Core Web API**, a **JavaScript SPA frontend**, and a **Python CLI client**.

---

## 🎥 Project Demo

📺 Watch the full demo on Google Drive:  
👉 https://drive.google.com/file/d/1_jFJWiav11HwPnb6xhhC5-BKtvjdon9H/view?usp=sharing

---

## 🚀 Project Structure

- **Backend**: ASP.NET Core Web API + Entity Framework Core + SQL
- **Frontend**: Single Page App (SPA) using Vanilla JavaScript
- **CLI Client**: Python script for bulk uploading and interacting with quotes

---

## 🧠 Features

### ✅ Web API (ASP.NET Core)
- CRUD operations for quotes (`Text`, optional `Author`)
- Like a quote (❤️)
- Retrieve top liked quotes (default: 10, customizable)
- Tag management:
  - Add new or existing tags
  - Associate multiple tags per quote (many-to-many)
- Filter quotes by:
  - ID
  - Tag
  - All quotes
- Fetch all available tags
- Built-in CORS support

### 🌐 Web Client (SPA)
- View all quotes with IDs
- Edit and delete quotes
- Like quotes from UI
- Add tags using autocomplete
- Filter by tag
- View top liked quotes

### 🐍 Python CLI Client
- Load quotes from a `.txt` file (one-time bulk load)
- Add new quotes interactively
- Get a random quote from the API

---

## 🛠️ Tech Stack

| Layer       | Tech                      |
|-------------|---------------------------|
| Backend     | ASP.NET Core Web API      |
| ORM         | Entity Framework Core     |
| Database    | SQL Server                |
| Frontend    | Vanilla JavaScript        |
| CLI Client  | Python 3.x + Requests     |
| API Format  | REST + JSON               |

---

## 🧪 Getting Started

### 🔧 Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [SQL Server / LocalDB](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Python 3.x](https://www.python.org/)
- [Node.js (optional, for SPA live server)](https://nodejs.org/)

---

### ⚙️ Backend Setup (ASP.NET Core API)

```bash
cd QuoteWebAPI

# Restore dependencies
dotnet restore

# Apply database migrations (creates the database)
dotnet ef database update

# Run the Web API
dotnet run
```

> This will start the API on `https://localhost:5001` or similar. Check your terminal output for the exact URL.

---

### 🌐 Frontend Setup (JavaScript SPA)

```bash
cd QuoteSPA

# Run the SPA client
dotnet run

```

---

### 🐍 Python CLI Client

```bash
cd QuoteWebAPI

# Run the CLI tool
python client.py
```

---

### 🧩 Database Notes

- Confirm that `appsettings.json` contains the correct connection string for your SQL Server or LocalDB instance.
- Entity Framework Core is used for managing migrations and relationships.

---

### 🖥️ Clone the Repo

```bash
git clone https://github.com/Drasti24/QuotesWebAPI.git
cd QuoteWebAPI
```

---

## 📦 Future Enhancements

- Add authentication and user-specific quote collections
- Implement pagination and search
- Add quote sharing functionality (e.g., copy/share button)
- Dockerize the application for container-based deployment
- Deploy to Azure or another cloud provider

---

## 👤 Author

- **Name:** Drasti Patel  
- **GitHub:** https://github.com/Drasti24
- **Email:** drasti.patel2402@gmail.com

---

## 📝 License

This project was developed as part of an academic assignment and is intended for educational use only.
