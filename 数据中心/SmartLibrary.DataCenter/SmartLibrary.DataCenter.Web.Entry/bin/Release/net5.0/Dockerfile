FROM mcr.microsoft.com/dotnet/aspnet:5.0.9
WORKDIR /app
EXPOSE 6102
EXPOSE 6103

COPY . .
ENTRYPOINT ["dotnet", "SmartLibrary.DataCenter.Web.dll"]