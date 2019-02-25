# ReFresh Foods 
ReFresh Foods is an ASP.NET Core (MVC) eCommerce website offering leftovers for sale. Registered users can log in to see customized content and to shop. Guest users can browse the page at their leisure, or they can register for an account for customized content and shopping.  

Deployed in Azure at: https://refreshfoods.azurewebsites.net

## Getting Started, Build, and Test
To build and run this page locally (using Visual Studio and SQL Server):
1. Clone the repo locally and compile it. There are no additional external dependencies or data sources to load/access.
2. This application uses User Secrets. Add connection strings for 2 databases (products and users).
3. Build the product database using existing migration (Update-Database -Context ReFreshDbContext).
4. Build the users database using existing migration (Update-Database -Context UserDbContext).
The application is ready to run via your local/live server.  

Unit testing is built with xUnit. Tests address get/set for all model properties and all methods in InventoryManagementService. Once the solution is built locally, select 'Run All' in Test Explorer to run tests.

## Architecture
This application is built on ASP.NET Core using Model-View-Controller (MVC) architecture. It uses Entity Framework Core's object relational mapping (ORM) to leverage the application's classes as data model definition, and to seamlessly move between the data storage and manipulation.  It relies on 2 SQL databases - one to manage products, and a separate one to manage user accounts (data security requires separation of these concerns).

### User Database
The User data model contains only basic properties (name and birthdate); however, the Identity API (Microsoft.AspNetCore.Identity) adds properties required in order to support security requirements. A View Model allows the application to capture data to populate the other Identity-initiated properties (ie - password) without having those appear in the User class. On registration, the user is also asked to state whether (s)he eats meat - a 'Carnivore' claim is captured to support product access restrictions (ie - to avoid displaying potentially offensive products to these customers). A 'FullName' claim is also captured, but it is built from First Name and Last Name inputs instead of requested separately. The 'FullName' claim supports display personalization (such as custom greetings).  

### Product Database
The Product data model contains properties to describe the product, pricing, meal category (enum), display data (such as image path string) and whether it contains meat. The Products page allows user-initiated filtering by category or by keyword search; the 'Carnivore' user claim enables policy-based filtering (filtered on the 'Meaty' property) - this filtering is transparent to the user.  

Dependency injection is used to isolate the data storage from its point of use (ie - the CRUD logic in the page routes). The Identity API includes interfaces and services for the user - those services have been injected into pages where user data is needed to change the displayed content (such as displaying a login link if no user is logged in). Additionally, the app contains an interface package for products to avoid direct coupling between page route logic and the Products database.

## Credit
This project is a collaborative effort by:
  Sean Miller - https://github.com/deliman206
  Gwen Zubatch - https://github.com/GwennyB