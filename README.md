<<<<<<< HEAD
#
A test for Google Antigravity.
It's a common E-commerce web App.

# ASP.NET Core E-commerce App

A modern, simple e-commerce web application built with **ASP.NET Core 8.0 MVC**, featuring passwordless authentication, a shopping cart system, and an admin dashboard. Designed for easy deployment on **Render**.

## Features

- **🛒 Customer Storefront**: Browse products, view details, and manage a shopping cart.
- **💳 Checkout System**: Simple checkout flow with mock credit card payment.
- **🔐 Passwordless Authentication**: Secure login using Email OTP (One-Time Password). No passwords to remember!
- **🛠️ Admin Dashboard**: Manage products (Create, Read, Update, Delete) with image support.
- **☁️ Cloud Ready**: Configured for Docker and Render deployment with PostgreSQL.

## Tech Stack

- **Framework**: ASP.NET Core 8.0 (MVC)
- **Database**: PostgreSQL (Entity Framework Core)
- **Containerization**: Docker
- **Frontend**: Razor Views, Bootstrap 5, Custom CSS

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/) (or use a Docker container)
- [Docker](https://www.docker.com/) (optional, for containerized run)

### Local Setup

1.  **Clone the repository**:
    ```bash
    git clone https://github.com/wke1998/AI-E-commerce-Web-App.git
    cd AI-E-commerce-Web-App
    ```

2.  **Configure Database**:
    Update `appsettings.json` with your PostgreSQL connection string:
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Host=localhost;Database=ecommerce;Username=your_user;Password=your_password"
    }
    ```

3.  **Apply Migrations**:
    Initialize the database schema:
    ```bash
    dotnet ef migrations add InitialCreate
    dotnet database update
    ```

4.  **Run the Application**:
    ```bash
    dotnet run
    ```
    The app will be available at `http://localhost:5000`.

## Usage Guide

### Authentication
- Go to **Login**.
- Enter your email address.
- Check your console logs (or configured email provider) for the **6-digit OTP code**.
- Enter the code to log in.

### Admin Access
- By default, new users are **Customers**.
- To become an **Admin**, manually update the `Role` column in the `Users` table to `'Admin'` for your user record.
- Once updated, you will see an **Admin** link in the navigation bar.

## Deployment (Render)

This project is configured for automatic deployment on [Render](https://render.com/).

1.  Push your code to GitHub.
2.  Create a new **Blueprint** on Render.
3.  Connect your repository.
4.  Render will automatically detect `render.yaml` and set up:
    - A **Web Service** (the ASP.NET app).
    - A **PostgreSQL Database**.
5.  Once deployed, the app will be live on your Render URL.

## License

This project is open source and available under the [MIT License](LICENSE).
=======
>>>>>>> 227b6285852c80b14172d2ef54adbee0c3850830
