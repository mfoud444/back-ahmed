# 🎨 Artify Backend Project

## 🌟 Project Overview 388431170

This is a backend solution for an e-commerce platform dedicated to selling paintings and showcasing art galleries, built with **.NET 8**. The project includes core functionalities such as **user authentication**, **product management** (artworks), **category management** (types of artwork ), and **order processing**. The system provides a seamless experience for artists and buyers to exchange artistic works with ease.

## Features

- **👤 User Management**:
  - Register new User (Customer/Artist/Admin)
  - User authentication with JWT token
  - Role-based access control (Artist, Customer, Admin)
- **🖼️ Product Management (Artworks)**:
  - Create new artwork listing (title, description, price)
  - Update artwork information
  - Delete artwork listings
  - Search and filter artworks by ArtistId and artist
- **🏷️ Category Management**:
  - Create new categories (types of artwork)
  - Retrieve category details
  - Update category information
  - Delete categories
  - Associate artworks with specific categories
- **🛠️ Workshop Management**:
  - Create new workshope
  - Retrieve workshop details
  - Update workshop information
  - Delete workshop
- **📅 Booking Management**:
  - Create new bookings for workshops
  - Retrieve bookings by user or workshop
  - Update booking status (confirmed, canceled)
  - Delete bookings
  - Handle bookings with pagination and filtering
- **📦 Order Management**:
  - Create new orders for purchased artworks
  - Retrieve order details by user or order ID
  - Update order status (pending, shipped, completed)
  - Delete orders
  - View order history for users
- **💳 Payment Management**:

## ⚙️ Technologies Used

- **.Net 8**: Web API Framework
- **Entity Framework Core**: ORM for database interactions
- **PostgreSQl**: Relational database for storing data
- **JWT**: For user authentication and authorization
- **AutoMapper**: For object mapping
- **Swagger**: API documentation

## 📋Prerequisites

- .Net 8 SDK
- SQL Server
- VSCode

## 🛠️ Getting Started

### 1. Clone the repository:

```bash
git clone https://github.com/AbeerAljohanii/sda-3-online-Backend_Teamwork
```

### 2.🛠️ Setup database

- Make sure PostgreSQL Server is running
- Create `appsettings.json` file
- Update the connection string in `appsettings.json`

```json
{
  "ConnectionStrings": {
    "Local": "Server=localhost;Database=ECommerceDb;User Id=your_username;Password=your_password;"
  }
}
```

- Run migrations to create database

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

- Run the application

```bash
dotnet watch
```

The API will be available at: `http://localhost:5125`

### 🐍 Swagger

- Navigate to `http://localhost:5125/swagger/index.html` to explore the API endpoints.

## 📂 Project structure

```bash
|-- Controllers: API controllers with request and response
|-- Database # DbContext and Database Configurations
|-- DTOs # Data Transfer Objects
|-- Entities # Database Entities (User, ArtWorks, Category, Order)
|-- Middleware # Logging request, response and Error Handler
|-- Repositories # Repository Layer for database operations
|-- Services # Business Logic Layer
|-- Utils # Common logics
|-- Migrations # Entity Framework Migrations
|-- Program.cs # Application Entry Point
```

## 📡 API Endpoints

### User

- **GET** `/api/users` – Get all users.
- **GET** `/api/users/{id}` – Get user by ID.
- **GET** `/api/users/profile` – Get user profile information.
- **GET** `/api/users/email/{email}` – Get user by email.
- **POST** `/api/users` – Register a new user.
- **POST** `/api/users/create-admin` – Create a new admin user.
- **POST** `/api/users/signin` – Login and get JWT token.
- **GET** `/api/users/search-by-name/{name}` – Search user by name.
- **GET** `/api/users/search-by-phone/{phoneNumber}` – Search user by phone number.
- **GET** `/api/users/pagination` – Pagination for users.
- **GET** `/api/users/count` – Get total number of users.
- **PUT** `/api/users/{id}` – Update a user by ID.
- **PUT** `/api/users/profile` – Update user profile information.
- **DELETE** `/api/users/{id}` – Delete a user by ID.

### Artwork

- **POST** `/api/artworks` – Create a new artwork (requires Artist role).
- **GET** `/api/artworks` – Get all artworks with pagination.
- **GET** `/api/artworks/{id}` – Get artwork by ID.
- **GET** `/api/artworks/artist/{artistId}` – Get artworks by artist ID.
- **PUT** `/api/artworks/{id}` – Update an artwork (requires Admin or Artist role).
- **DELETE** `/api/artworks/{id}` – Delete an artwork (requires Admin or Artist role).
- **GET** `/api/artworks/pagination` – Get artworks with pagination options (optional).
- **GET** `/api/artworks/featured` – Get featured artworks (optional).
- **GET** `/api/artworks/search` – Search artworks by title or description (optional).

### Category

- **GET** `/api/categories` – Get all categories.
- **GET** `/api/categories/{id}` – Get category by ID.
- **GET** `/api/categories/search/{name}` – Get category by name.
- **GET** `/api/categories/page` – Get categories with pagination.
- **POST** `/api/categories` – Create a new category.
- **PUT** `/api/categories/{id}` – Update a category.
- **DELETE** `/api/categories/{id}` – Delete a category.

### Order

- **GET** `/api/orders` – Get all orders.
- **GET** `/api/orders/sort-by-date` – Sort orders by date.
- **GET** `/api/orders/{id}` – Get order by ID.
- **GET** `/api/orders/my-orders` – Get all orders for the authenticated customer.
- **GET** `/api/orders/my-orders/{id}` – Get a specific order for the authenticated customer by ID.
- **GET** `/api/orders/pagination` – Get orders with pagination for admins.
- **POST** `/api/orders/add` – Create a new order.
- **PUT** `/api/orders/{id}` – Update an existing order by ID.
- **DELETE** `/api/orders/{id}` – Delete an order by ID.

## 🌐 Deployment

The application is deployed and can be accessed at: [https://sda-3-online-backend-teamwork-yjdp.onrender.com](https://sda-3-online-backend-teamwork-yjdp.onrender.com/)

## 👩‍💻 Team Members

- **Lead**: [Abeer Aljohani 👩‍💻](https://github.com/AbeerAljohanii)
- [Bashaer Alhuthali 👩‍💻](https://github.com/bashaer310)
- [Danah Almalki 👩‍💻](https://github.com/DanaAlmalki)
- [Manar Almalawi 👩‍💻](https://github.com/mal-manar)
- [Shuaa Almarwani 👩‍💻](https://github.com/Shuaa-99)

## License

This project is licensed under the MIT License.
