FROM mcr.microsoft.com/dotnet/aspnet:5.0.9
WORKDIR /app
EXPOSE 5802
EXPOSE 5803

COPY . .
ENTRYPOINT ["dotnet", "SmartLibrary.News.Web.dll"]