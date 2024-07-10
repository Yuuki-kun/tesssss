# DC8Training.QLSV

## 1.  Learning C#
- feature-1: Generate Data

- feature-2: Menu 1 - Add Student

- feature-3: Menu 2 - Add Class

- feature-4: Menu 3 - Add Student into Class, add a grade of this student in this class

- feature-5: Menu 4 - Get Student by ID, return Name, Gender, Grade per class, Avg Point

- feature-6: Menu 5 - Search Student by keyword (Contains)

- feature-7: Menu 6 - Get List of Student that not pass the class (<5) or Avg all classes <5

- feature-8: Menu 7 - List Top 10 Students with sorting by Avg Point desc. Also, press 'Enter' to load the next 10 Students

## 2. Learning SQL
- Create a database schema for the above project to manage student, class and grade
- Create function/Stored procedure to do the same 7 menus above

  #### Refer: 
- https://www.w3schools.com/sql/sql_intro.asp
- https://www.simplilearn.com/tutorials/sql-tutorial/stored-procedure-in-sql

## 3. ASP.Net
- Create API work as 7 menus above (don't call database or use entity framework)
- Try to add routing per API and add versioning. For example versioning: Ver1.0 is *Synchronous* APIs, Vers2.0 is *Asynchronous * APIs
- Try to add a custom middleware to do something in WebAPI. Example: Add a middleware to log API *Method*, *URL* per request. Advanced example: add a middle to catch every exception during execute the action, Controller/Services will return directly DTO instead IActionResult, both use throw a custom Exception to return any status code not OK(200).
- Try to add a custom filter to do somthing in WebAPI. Example: Add a filter to read data "Rest-Api-Key" from Header then validate, decode base64 of this header and check if it equals a static string. Work as Role checker. Advanced example: Disable the advance middleware above, write a filter to do the same thing.
- Try to validate Request DTO every APIs.
- Add and read configuration/setting for "Rest-API-key", app must be able to retrieve data from environment variables.
- Add at least 1 way to Authentication/Authorization user, recommended for JWT authentication

#### Refer:
- https://www.pragimtech.com/blog/blazor/what-are-restful-apis/
- https://learn.microsoft.com/en-us/aspnet/web-api/overview/web-api-routing-and-actions/
- https://code-maze.com/aspnetcore-api-versioning/
- https://www.c-sharpcorner.com/article/overview-of-middleware-in-asp-net-core/
- https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-7.0
- https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-8.0
- https://learn.microsoft.com/en-us/aspnet/web-api/overview/security/authentication-and-authorization-in-aspnet-web-api

## 4. ORM - EntityFrameworkCore
- Re-structure the DC8Training.Common project to have Repositories layer that is used to connect to database. This layer has a responsibility to work with the DBContext of the Entity framework
- Implement this layer to replace the old function that work with file json. Data now should stored in the database that we work in #2 (remove the code that work with file json to clean code)
- Try to call the store procedure from #2 if possible to resue code.
#### Refer:
- https://learn.microsoft.com/en-us/ef/core/

## 5. Other techniques
- To answer the question: What is the difference between AddTransient, AddScoped, AddSingleton? Why, When use it?
- Add Dependency injection into the project, following structure that each layer will have their own static dependency class/methods to inject the correct component in that layer. WebAPI only need to call 1 method per layer to inject.
- Add In-memory cache to cache a data like user/teacher/student to improve the performance. Know about cache expired and handle cache incase create/update/delete.

#### Refer:
- https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-8.0
- https://learn.microsoft.com/en-us/aspnet/core/performance/caching/overview?view=aspnetcore-7.0
- https://code-maze.com/automapper-net-core/
