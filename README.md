# Catch-Up Backend

## Overview

Catch-Up Backend is a web application designed to assist with onboarding. This project represents only the backend part of the entire application.
The complete system also includes:

- [Frontend](https://github.com/InterfectoremCubiculum/catch_up_frontend) built with React + Vite (TypeScript)
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
-	File sharing/downloading.
-	Archived and Deleted States.
    <details>
    <summary>Example</summary><br/>
      
    StateEnum State property is used to track the status (or lifecycle) of a FAQ entry using the StateEnum enum.
    ```c#
        public class FaqModel
        {
            [Key]
            public int Id { get; set; }
            public string Question { get; set; }
            public string Answer { get; set; }
            [ForeignKey("MaterialsId")]
            public int? MaterialId { get; set; }
            [ForeignKey("CreatorId")]
            public Guid CreatorId { get; set; }
            public StateEnum State { get; set; }
            public FaqModel(string question, string answer, int? materialId, Guid creatorId)
            {
                this.Question = question;
                this.Answer = answer;
                this.MaterialId = materialId;
                State = StateEnum.Active;
                CreatorId = creatorId;
            }
        }
    ```
    <br/>
    
    State	Value	Meaning:
 	
    - Active (0) - The entry is visible and currently in use.
    - Archived (5) - The entry is no longer active, but kept for reference.
    - Deleted (10) - The entry is logically deleted (soft delete) and not shown to users.\
 	  <br/>
    
    
    ```c#
        public enum StateEnum
        {
            Active = 0,
            Archived = 5,
            Deleted = 10
        }
    ```

    Example of soft Delete.
 	
    ```c#
        public async Task<bool> DeleteAsync(int faqId)
        {
            var faq = await _context.Faqs.FindAsync(faqId);
            if (faq == null)
                return false;
            try
            {
                faq.State = StateEnum.Deleted;
                _context.Faqs.Update(faq);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error: Faq Delete: " + ex);
            }
            return true;
        }
    ```
    </details>

## Technologies Used
- .NET 8
- Entity Framework Core (SQL Server)
- SignalR for real-time communication
- Firebase Admin SDK
- JWT Authentication
  <details> 
  <summary>See more</summary><br/>
    
   #### This code registers and configures JWT Bearer Authentication in an ASP.NET Core app
  
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
    #### This is a helper method to manually extract the userId from a JWT token found in the request.
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
    #### Exmple of use
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
    <img src="https://github.com/user-attachments/assets/5f0a8cef-284c-4488-bbe4-b892d641a3c4">
  </details>
- Services: Contain business logic (e.g., EmailService, NotificationService).
  <details>
    <summary>Example</summary><br/>
    <img src="https://github.com/user-attachments/assets/36c36558-a2ed-4a3e-9d9a-2a9521a473c5">
    <img src="https://github.com/user-attachments/assets/11360c91-f827-4078-8884-aa0bddda6bbb">
  </details>
- Repositories: Interfaces and implementations for data access.
  <details>
    <summary>Example</summary><br/>
    <img src="https://github.com/user-attachments/assets/c90cb82e-fb38-4701-b69d-5915c7e97cf8">
  </details>
- Models: Definitions of data structures (e.g., UserModel, NewbieMentorModel).
  <details>
    <summary>Example</summary><br/>
    <img src="https://github.com/user-attachments/assets/b33308fd-3949-4905-9ae2-2dafaadf8611">
  </details>
- Migrations: Handle changes to the database schema.
  <details>
    <summary>Example</summary><br/>
    <img src="https://github.com/user-attachments/assets/e6676ae1-bf01-4ae8-9dcb-4b3667027d14">
  </details>
- Hubs: Handle real-time communication (e.g., NotificationHub).
  <details>
  <summary>See more</summary><br/>
    
  ```c#
      public class NotificationHub : Hub
      {
          private readonly IRefreshTokenRepository _refreshTokenRepository;
      
          public NotificationHub(IRefreshTokenRepository refreshTokenRepository)
          {
              _refreshTokenRepository = refreshTokenRepository;
          }
      
          public override async Task OnConnectedAsync() 
          {
      
              var accessToken = Context.GetHttpContext()?.Request?.Query["access_token"].ToString();
            
              Guid userId;
      
              if (string.IsNullOrEmpty(accessToken))
              {
                  userId = TokenHelper.GetUserIdFromTokenInRequest(Context.GetHttpContext()?.Request);
              }
              else
              {
                  var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
                  userId = Guid.Parse(jwtToken.Claims.First(c => c.Type == "nameid").Value);
              }
      
              await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());
      
              await base.OnConnectedAsync();
          }
      
          public override async Task OnDisconnectedAsync(Exception exception)
          {
              var accessToken = Context.GetHttpContext()?.Request?.Query["access_token"].ToString();
              var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
              var userId = Guid.Parse(jwtToken.Claims.First(c => c.Type == "nameid").Value);
      
              await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId.ToString());
      
              await base.OnDisconnectedAsync(exception);
          }
      }
  ```
  </details>
- DTO (Data Transfer Object): Used for transferring data between layers of the application. Allow for the separation of business logic from the data transmitted via the API.
  <details>
    <summary>Example</summary><br/>
    
    ```c#
      public class RoadMapDto
      {
          public int Id { get; set; }
          public Guid NewbieId { get; set; }
          public Guid CreatorId { get; set; }
          public string? CreatorName { get; set; }
          public string? Title { get; set; }
          public string? Description { get; set; }
          public DateTime? AssignDate { get; set; }
          public DateTime? FinishDate { get; set; }
          public StatusEnum? Status { get; set; }
          public decimal? Progress { get; set; }
      }
    ```
  </details>
<br/><br/>
<img src="https://github.com/user-attachments/assets/6213ab23-acd7-440f-aa7c-f25d923b9b42">
