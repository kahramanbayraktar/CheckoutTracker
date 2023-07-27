#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Checkout/Checkout.API/Checkout.API.csproj", "Services/Checkout/Checkout.API/"]
COPY ["Services/Checkout/Checkout.Domain/Checkout.Domain.csproj", "Services/Checkout/Checkout.Domain/"]
RUN dotnet restore "Services/Checkout/Checkout.API/Checkout.API.csproj"
COPY . .
WORKDIR "/src/Services/Checkout/Checkout.API"
RUN dotnet build "Checkout.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Checkout.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Checkout.API.dll"]