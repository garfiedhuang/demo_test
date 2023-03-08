#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Hkust.Usr/Hkust.Usr.WebApi/Hkust.Usr.WebApi.csproj", "Services/Hkust.Usr/Hkust.Usr.WebApi/"]
COPY ["Services/Hkust.Usr/Hkust.Usr.Application/Hkust.Usr.Application.csproj", "Services/Hkust.Usr/Hkust.Usr.Application/"]
COPY ["Services/Hkust.Usr/Hkust.Usr.Repository/Hkust.Usr.Repository.csproj", "Services/Hkust.Usr/Hkust.Usr.Repository/"]
COPY ["Infras/Hkust.Infras.Repository/Hkust.Infras.Repository.csproj", "Infras/Hkust.Infras.Repository/"]
COPY ["Infras/Hkust.Infras.Core/Hkust.Infras.Core.csproj", "Infras/Hkust.Infras.Core/"]
COPY ["Services/Shared/Hkust.Shared/Hkust.Shared.csproj", "Services/Shared/Hkust.Shared/"]
COPY ["Services/Shared/Hkust.Shared.Rpc/Hkust.Shared.Rpc.csproj", "Services/Shared/Hkust.Shared.Rpc/"]
COPY ["Services/Shared/Hkust.Shared.Application/Hkust.Shared.Application.csproj", "Services/Shared/Hkust.Shared.Application/"]
COPY ["Infras/Hkust.Infras.Mapper/Hkust.Infras.Mapper.csproj", "Infras/Hkust.Infras.Mapper/"]
COPY ["Infras/Hkust.Infras.Helper/Hkust.Infras.Helper.csproj", "Infras/Hkust.Infras.Helper/"]
COPY ["Infras/Hkust.Infras.EventBus/Hkust.Infras.EventBus.csproj", "Infras/Hkust.Infras.EventBus/"]
COPY ["Infras/Hkust.Infras.EfCore.MySQL/Hkust.Infras.EfCore.MySQL.csproj", "Infras/Hkust.Infras.EfCore.MySQL/"]
COPY ["Infras/Hkust.Infras.Dapper/Hkust.Infras.Dapper.csproj", "Infras/Hkust.Infras.Dapper/"]
COPY ["Infras/Hkust.Infras.Mongo/Hkust.Infras.Mongo.csproj", "Infras/Hkust.Infras.Mongo/"]
COPY ["Services/Shared/Hkust.Shared.Application.Contracts/Hkust.Shared.Application.Contracts.csproj", "Services/Shared/Hkust.Shared.Application.Contracts/"]
COPY ["Infras/Hkust.Infras.Caching/Hkust.Infras.Caching.csproj", "Infras/Hkust.Infras.Caching/"]
COPY ["Infras/Hkust.Infras.IdGenerater/Hkust.Infras.IdGenerater.csproj", "Infras/Hkust.Infras.IdGenerater/"]
COPY ["Infras/Hkust.Infras.Consul/Hkust.Infras.Consul.csproj", "Infras/Hkust.Infras.Consul/"]
COPY ["Services/Hkust.Usr/Hkust.Usr.Application.Contracts/Hkust.Usr.Application.Contracts.csproj", "Services/Hkust.Usr/Hkust.Usr.Application.Contracts/"]
COPY ["Services/Shared/Hkust.Shared.WebApi/Hkust.Shared.WebApi.csproj", "Services/Shared/Hkust.Shared.WebApi/"]
RUN dotnet restore "Services/Hkust.Usr/Hkust.Usr.WebApi/Hkust.Usr.WebApi.csproj"
COPY . .
WORKDIR "/src/Services/Hkust.Usr/Hkust.Usr.WebApi"
RUN dotnet build "Hkust.Usr.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hkust.Usr.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hkust.Usr.WebApi.dll"]