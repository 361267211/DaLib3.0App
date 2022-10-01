FROM mcr.microsoft.com/dotnet/aspnet:5.0.9
WORKDIR /app
EXPOSE 5202
EXPOSE 5203
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime
RUN echo 'Asia/Shanghai' >/etc/timezone
COPY . .
ENTRYPOINT ["dotnet", "SmartLibrary.User.Web.dll"]