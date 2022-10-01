FROM mcr.microsoft.com/dotnet/aspnet:5.0.9
WORKDIR /app
EXPOSE 7102
EXPOSE 7103

ENV ApiUrl='192.168.21.46' GrpcUrl='192.168.21.46' ApiPort=7102 GrpcPort=7103

COPY . .
ENTRYPOINT ["dotnet", "SmartLibrary.ScoreCenter.Web.dll"]