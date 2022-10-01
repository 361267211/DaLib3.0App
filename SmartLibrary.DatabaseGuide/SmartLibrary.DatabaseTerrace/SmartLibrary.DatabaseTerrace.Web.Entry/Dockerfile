FROM mcr.microsoft.com/dotnet/aspnet:5.0.9
WORKDIR /app
EXPOSE 5702
EXPOSE 5703

COPY . .
ENTRYPOINT ["dotnet", "SmartLibrary.DatabaseTerrace.Web.dll"]