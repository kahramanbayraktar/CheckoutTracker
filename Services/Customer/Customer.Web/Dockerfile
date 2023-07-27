#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CustomerService/CustomerService.Web/CustomerService.Web.csproj", "CustomerService/CustomerService.Web/"]
COPY ["CustomerService/CustomerService.Domain/CustomerService.Domain.csproj", "CustomerService/CustomerService.Domain/"]
COPY ["CustomerService/CustomerService.Utils/CustomerService.Utils.csproj", "CustomerService/CustomerService.Utils/"]
RUN dotnet restore "CustomerService/CustomerService.Web/CustomerService.Web.csproj"
COPY . .
WORKDIR "/src/CustomerService/CustomerService.Web"
RUN dotnet build "CustomerService.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CustomerService.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CustomerService.Web.dll"]