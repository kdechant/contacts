# Demo ASP.NET app

To run this project:

1. Make sure you have MS SQL Server running and able to use Windows Authentication
2. Open the solution in Visual Studio
3. Run database migrations
    1. In Visual Studio's Package Manager Console, run `Update-Database`
	1. Alternately, from PowerShell, run `dotnet ef database update`
		1. If this fails, run `dotnet tool install --global dotnet-ef --version 8.*` and try again
4. Run the project with the Visual Studio "play" button. A browser window should open with Swagger in it.

To run tests:

1. In Visual Studio, in the Solution Explorer, right-click the "ContactManager.Tests" project and click "Run Tests"
2. If you see any build errors while running tests, run "Clean Solution" and right-click the "ContactManager.Tests" project and clean it. Then try again.
	1. If that doesn't help, try this: https://stackoverflow.com/questions/18102859/visual-studio-could-not-copy-during-build
