dotnet restore

dotnet build TauCode.WebApi.Server.sln -c Debug
dotnet build TauCode.WebApi.Server.sln -c Release

dotnet test TauCode.WebApi.Server.sln -c Debug
dotnet test TauCode.WebApi.Server.sln -c Release

nuget pack nuget\TauCode.WebApi.Server.nuspec