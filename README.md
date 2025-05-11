# Catch-Up Backend

## Overview

Catch-Up Backend is a web application designed to assist with onboarding. This project represents only the backend part of the entire application.
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
  <details> 
  <summary>See more</summary><br/>
  
    ```c#
     //Authentication
     builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)   // this tells .NET to use JWT  bearer authentication as the default one
         .AddJwtBearer(options => {                                               // comapnySettings fot the JWT bearer authentication
                 options.TokenValidationParameters = new TokenValidationParameters    // this object contains all of the rules for validating
             {
                 ValidateIssuerSigningKey = true,                                 // this tells .NET to validate the issuer signing key
                 IssuerSigningKey = new SymmetricSecurityKey(                     // this gets the secret key from the secret user config and converts it to a byte array
                     Encoding.ASCII.GetBytes(builder.Configuration["Jwt:AccessTokenSecret"])
                 ),
                 ValidateIssuer = true,                                           // tells .NET to validate who created the token 
                 ValidIssuer = builder.Configuration["Jwt:Issuer"],               // sets the issuer to the one from the user secrets (Issuer)
                 ValidateAudience = true,                                         // tells .NET to check who the token is intended for
                 ValidAudience = builder.Configuration["Jwt:Audience"],           // sets the audience to the one from the user secrets (Audience)
                 ClockSkew = TimeSpan.Zero                                        // sets a time window for the token to be valid (In production change to ~10sec), because there might be a situation when token is still valid on the                         client but not on the server.
           };
  
           options.Events = new JwtBearerEvents                                 // reading the token from the cookie
           {
               OnMessageReceived = context => {                                 // when a request comes in, this function will run
                   context.Token = context.Request.Cookies["accessToken"];      // get the token from the "accessToken" cookie insteaf of the Authorization header
                   return Task.CompletedTask;
               }
           };
       });
    ```
    ```c#
        public static class TokenHelper
        {
            public static Guid GetUserIdFromTokenInRequest(HttpRequest request)
            {
                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(
                    request.Headers["Authorization"].ToString().Substring("Bearer ".Length).Trim()
                );
                var userId = Guid.Parse(jwtToken.Claims.First(c => c.Type == "nameid").Value);
    
                return userId;
            }
        }
    ```
    ```c#
      [HttpGet]
      [Route("GetUserSchooling/{schoolingId:int}")]
      public async Task<IActionResult> GetUserSchooling(int schoolingId)
      {
          var userId = TokenHelper.GetUserIdFromTokenInRequest(Request);
          var schooling = await _schoolingService.GetById(schoolingId, userId);
    
          return schooling != null
              ? Ok(schooling)
              : NotFound(new { message = "Schooling not found." });
      }
    ```
  </details>
- Swagger/OpenAPI
  <details> 
  <summary>Examples</summary><br/>
    
  <img src="https://github.com/user-attachments/assets/9eb219f5-ab2f-437c-a676-402264500f62">
  </details>
- SMTP for email notifications

## Project Structure
- Controllers: Handle API logic (e.g., NewbieMentorController for managing mentor assignments).
  <details>
    <summary>Example</summary><br/>
    <img src="https://github.com/user-attachments/assets/5f0a8cef-284c-4488-bbe4-b892d641a3c4"
  </details>
- Services: Contain business logic (e.g., EmailService, NotificationService).
- Repositories: Interfaces and implementations for data access.
- Models: Definitions of data structures (e.g., UserModel, NewbieMentorModel).
- Migrations: Handle changes to the database schema.
- Hubs: Handle real-time communication (e.g., NotificationHub).
- DTO (Data Transfer Object): Used for transferring data between layers of the application. Allow for the separation of business logic from the data transmitted via the API.
