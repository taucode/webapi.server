dotnet restore

dotnet build --configuration Debug
dotnet build --configuration Release

dotnet test -c Debug .\test\TauCode.WebApi.Server.Tests\TauCode.WebApi.Server.Tests.csproj
dotnet test -c Release .\test\TauCode.WebApi.Server.Tests\TauCode.WebApi.Server.Tests.csproj

nuget pack nuget\TauCode.WebApi.Server.nuspec
