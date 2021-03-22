# RecieptManagerAPI
Small API project made on .NET to have CRUD operations over receipts and users.
The API is hosted on a Web APP Azure resource on the link: https://recieptmanager.azurewebsites.net, to visit a documentation about the endpoints and the results types go to the link: https://recieptmanager.azurewebsites.net/swagger/index.html#/.

The API uses a SQL Database hosted on Azure to save the tables of receipts and users. 
The projects haves CI/CD to allow the server gets updated on each commit, running 2 steps of testing during the process. The RecieptManagerAPI.Tests haves the UnitTest and End2End Test projects, having over 50 tests to ensure the correct behabior of the API endpoints.

TODO: Use a User Administrator Service, wich for now are being treated as any other table with CRUD operations and no security.

