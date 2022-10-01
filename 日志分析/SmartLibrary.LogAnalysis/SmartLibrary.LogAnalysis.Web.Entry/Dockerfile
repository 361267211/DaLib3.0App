FROM mcr.microsoft.com/dotnet/aspnet:5.0.9
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY . .
ENTRYPOINT ["dotnet", "SmartLibrary.LogAnalysis.Web.dll"]