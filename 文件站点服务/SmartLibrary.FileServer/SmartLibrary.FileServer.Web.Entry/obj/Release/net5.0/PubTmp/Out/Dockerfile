FROM mcr.microsoft.com/dotnet/aspnet:5.0.9
WORKDIR /app
EXPOSE 5044
EXPOSE 6900

COPY . .
ENTRYPOINT ["dotnet", "SmartLibrary.FileServer.Web.dll"]