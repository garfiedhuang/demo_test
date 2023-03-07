#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Hkust.Usr/Hkust.Usr.WebApi/Hkust.Usr.WebApi.csproj", "Services/Hkust.Usr/Hkust.Usr.WebApi/"]
COPY ["Services/Hkust.Usr/Hkust.Usr.Application/Hkust.Usr.Application.csproj", "Services/Hkust.Usr/Hkust.Usr.Application/"]
COPY ["Services/Hkust.Usr/Hkust.Usr.Repository/Hkust.Usr.Repository.csproj", "Services/Hkust.Usr/Hkust.Usr.Repository/"]
COPY ["Infras/Hkust.Infra.Repository/Hkust.Infra.Repository.csproj", "Infras/Hkust.Infra.Repository/"]
COPY ["Infras/Hkust.Infra.Core/Hkust.Infra.Core.csproj", "Infras/Hkust.Infra.Core/"]
COPY ["Services/Shared/Hkust.Shared/Hkust.Shared.csproj", "Services/Shared/Hkust.Shared/"]
COPY ["Services/Shared/Hkust.Shared.Rpc/Hkust.Shared.Rpc.csproj", "Services/Shared/Hkust.Shared.Rpc/"]
COPY ["Services/Shared/Hkust.Shared.Application/Hkust.Shared.Application.csproj", "Services/Shared/Hkust.Shared.Application/"]
COPY ["Infras/Hkust.Infra.Mapper/Hkust.Infra.Mapper.csproj", "Infras/Hkust.Infra.Mapper/"]
COPY ["Infras/Hkust.Infra.Helper/Hkust.Infra.Helper.csproj", "Infras/Hkust.Infra.Helper/"]
COPY ["Infras/Hkust.Infra.EventBus/Hkust.Infra.EventBus.csproj", "Infras/Hkust.Infra.EventBus/"]
COPY ["Infras/Hkust.Infra.EfCore.MySQL/Hkust.Infra.EfCore.MySQL.csproj", "Infras/Hkust.Infra.EfCore.MySQL/"]
COPY ["Infras/Hkust.Infra.Dapper/Hkust.Infra.Dapper.csproj", "Infras/Hkust.Infra.Dapper/"]
COPY ["Infras/Hkust.Infra.Mongo/Hkust.Infra.Mongo.csproj", "Infras/Hkust.Infra.Mongo/"]
COPY ["Services/Shared/Hkust.Shared.Application.Contracts/Hkust.Shared.Application.Contracts.csproj", "Services/Shared/Hkust.Shared.Application.Contracts/"]
COPY ["Infras/Hkust.Infra.Caching/Hkust.Infra.Caching.csproj", "Infras/Hkust.Infra.Caching/"]
COPY ["Infras/Hkust.Infra.IdGenerater/Hkust.Infra.IdGenerater.csproj", "Infras/Hkust.Infra.IdGenerater/"]
COPY ["Infras/Hkust.Infra.Consul/Hkust.Infra.Consul.csproj", "Infras/Hkust.Infra.Consul/"]
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