# Sudoku Master - C# WinForms Application

O aplicație desktop completă de Sudoku, dezvoltată în C# folosind Windows Forms. Proiectul include un sistem integrat de autentificare, generare algoritmică de puzzle-uri și clasament în timp real bazat pe performanță.

## 🚀 Funcționalități Principale

* **Sistem de Autentificare:** * Înregistrare cu validarea complexității parolei (Regex).
    * Sistem de login securizat conectat la o bază de date SQL Server CE.
* **Motor de Joc Sudoku:**
    * Generare dinamică de soluții valide folosind un algoritm de backtracking.
    * Trei nivele de dificultate: Începător (45 indicii), Intermediar (30 indicii), Avansat (15 indicii).
    * Interfață interactivă cu evidențierea celulei selectate și validarea vizuală a erorilor (marcaj roșu).
* **Sistem de Scoring & Clasament:**
    * Cronometru integrat pentru măsurarea performanței.
    * Salvarea automată a celui mai bun timp (High Score) în baza de date.
    * Vizualizarea topului jucătorilor într-un DataGridView dedicat.

## 🛠️ Detalii Tehnice

* **Limbaj:** C#
* **Framework:** .NET Framework (Windows Forms)
* **Bază de date:** SQL Server Compact Edition (SqlServerCe)
* **Arhitectură:** Obiectuală (clasa personalizată `SudokuCell` extinsă din `Button`)

## 📁 Structura Codului (Form1.cs)

Proiectul este organizat pe regiuni logice pentru o mentenanță ușoară:
- `Initialization`: Configurarea grilei de 9x9 și a layout-ului dinamic.
- `Game Logic`: Algoritmul de generare a soluției și verificarea validității numerelor.
- `Timer Logic`: Gestionarea timpului de joc.
- `Event Handlers`: Interacțiunea utilizatorului cu celulele și butoanele meniului.

## ⚙️ Cerințe de Sistem

* Visual Studio (recomandat 2019+)
* SQL Server Compact 4.0 (pentru gestionarea bazei de date locale)

---
*Proiect realizat ca studiu de caz pentru dezvoltarea aplicațiilor desktop și managementul datelor în C#.*
