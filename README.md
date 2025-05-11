# Catch-Up Backend

## Overview

Catch-Up Backend is a web application designed to assist with onboarding. This project represents only the frontend part of the entire application.
The complete system also includes:

- [Frontend](https://github.com/InterfectoremCubiculum/catch_up_frontend) built with .NET
- [Mobile app](https://github.com/InterfectoremCubiculum/catch_up_Mobile) developed using .NET MAUI
  

## Features
-	Authentication & Authorization: Secure JWT-based authentication with support for token validation and cookie-based access.
-	Real-Time Communication: SignalR integration for real-time notifications and updates.
-	Database Management: Entity Framework Core with SQL Server for database operations and migrations.
-	Task Management: Models and services for managing tasks, categories, and related content.
-	Event Management: Support for creating and managing events with descriptions.
-	Notification System: Real-time notifications with SignalR hubs and DTOs for structured data transfer.
-	Firebase Integration: Firebase support for additional services like push notifications.
-	CORS Support: Configured to allow cross-origin requests for seamless frontend-backend communication.
-	Swagger Documentation: Integrated Swagger/OpenAPI for API documentation and testing.

## Technologies Used
- .NET 8
- Entity Framework Core (SQL Server)
- SignalR for real-time communication
- Firebase Admin SDK
- JWT Authentication
- <details> <summary>Swagger/OpenAPI</summary>
<img src="https://github.com/user-attachments/assets/9eb219f5-ab2f-437c-a676-402264500f62">
</details>
- SMTP for email notifications
## Project Structure
- Controllers: Handle API logic (e.g., NewbieMentorController for managing mentor assignments).
- Services: Contain business logic (e.g., EmailService, NotificationService).
- Repositories: Interfaces and implementations for data access.
- Models: Definitions of data structures (e.g., UserModel, NewbieMentorModel).
- Migrations: Handle changes to the database schema.
- Hubs: Handle real-time communication (e.g., NotificationHub).
- DTO (Data Transfer Object): Used for transferring data between layers of the application. Allow for the separation of business logic from the data transmitted via the API.
