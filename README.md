# Sistem de gestionare al ciclului de viata al unui produs DRX IT DAY
# Dräxlmaier IT Day 2025 - Project

This repository contains the application developed by **Mihai** and **Teo** as part of the **Dräxlmaier IT Day 2025** competition. The project showcases a modern WPF application with a strong focus on multi-layered architecture, database integration, and MVVM design principles.

## Structure
- **WPF application** leveraging the **MVVM** architecture for clean separation of concerns.
- **Multi-layered structure**:
  - **DataAccess Layer**: Handles database interactions with SQL Server LocalDB.
  - **BusinessLogic Layer**: Manages application logic and processes.
  - **UI Layer**: Provides a user-friendly interface.
- Database integration:
  - **SQL Server **

##Features
The application allows users to efficiently manage the product lifecycle, ensuring a structured and controlled workflow. **Key functionalities** include:

- Managing Materials, BOMs (Bills of Materials), Products, User Roles, and product stages.
- Role-based permissions – Assign users to roles with access control, ensuring security and accountability.
- State-based restrictions – Only users with the appropriate roles can modify a product's state.
- Generating dynamic charts to visualize product lifecycle data.
- Exporting PDF reports for analysis and documentation.
- Intuitive and modern UI/UX – The application offers a sleek, user-friendly interface designed for efficiency and ease of use.

## Team
- **Mihai**: Focused on architecture, MVVM implementation, and UI design.
- **Teo**: Handled database design, queries, and business logic integration.

## About the Competition
This project was developed for the **Dräxlmaier IT Day 2025**, under the theme of **Sistem de gestionare al ciclului de viata al unui produs**.

## Tools & Libraries Used
- **LiveCharts** – Used to render dynamic and visually appealing charts.  
- **MVVM Toolkit** – Helps eliminate boilerplate code and speeds up development.  
- **MigraDoc & PDFsharp** – Used for exporting high-quality PDFs efficiently.
  
## Getting Started
1. Clone this repository.
2. Open the solution in **Visual Studio 2022**.
3. Ensure **SQL Server LocalDB** is installed & run the Database Creation script.
5. Build and run the project.
6. Once the connectionSettings.json file is created, navigate to the project directory and locate the file in either the Debug or Release folder.
7. Insert the correct connection string into connectionSettings.json.
8. Run the project again, this will create an admin account automatically.



   
