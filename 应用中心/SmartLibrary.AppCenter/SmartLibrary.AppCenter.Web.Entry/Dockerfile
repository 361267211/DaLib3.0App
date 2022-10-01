FROM mcr.microsoft.com/dotnet/aspnet:5.0.9
WORKDIR /app
EXPOSE 5302
EXPOSE 5303
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime
RUN echo 'Asia/Shanghai' >/etc/timezone
COPY . .
ENTRYPOINT ["dotnet", "SmartLibrary.AppCenter.Web.dll"]