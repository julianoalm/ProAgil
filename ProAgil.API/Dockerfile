FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
LABEL version="1.0.2" description="API .NET Core MVC"
COPY dist /app
WORKDIR /app
EXPOSE 80/tcp
ENTRYPOINT ["dotnet","ProAgil.API.dll"]