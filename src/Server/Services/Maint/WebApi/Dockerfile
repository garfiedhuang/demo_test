#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Hkust.Maint/Hkust.Maint.WebApi/Hkust.Maint.WebApi.csproj", "Services/Hkust.Maint/Hkust.Maint.WebApi/"]
COPY ["Services/Shared/Hkust.Common.WebApi/Hkust.Common.WebApi.csproj", "Services/Shared/Hkust.Common.WebApi/"]
COPY ["Infras/Hkust.Infras.Helper/Hkust.Infras.Helper.csproj", "Infras/Hkust.Infras.Helper/"]
COPY ["Infras/Hkust.Infras.Core/Hkust.Infras.Core.csproj", "Infras/Hkust.Infras.Core/"]
COPY ["Infras/Hkust.Infras.EventBus/Hkust.Infras.EventBus.csproj", "Infras/Hkust.Infras.EventBus/"]
COPY ["Services/Shared/Hkust.Common.Application.Contracts/Hkust.Common.Application.Contracts.csproj", "Services/Shared/Hkust.Common.Application.Contracts/"]
COPY ["Services/Shared/Hkust.Common.Rpc/Hkust.Common.Rpc.csproj", "Services/Shared/Hkust.Common.Rpc/"]
COPY ["Services/Shared/Hkust.Shared/Hkust.Common.csproj", "Services/Shared/Hkust.Shared/"]
COPY ["Infras/Hkust.Infras.Caching/Hkust.Infras.Caching.csproj", "Infras/Hkust.Infras.Caching/"]
COPY ["Infras/Hkust.Infras.Consul/Hkust.Infras.Consul.csproj", "Infras/Hkust.Infras.Consul/"]
COPY ["Services/Hkust.Maint/Hkust.Maint.Application/Hkust.Maint.Application.csproj", "Services/Hkust.Maint/Hkust.Maint.Application/"]
COPY ["Services/Shared/Hkust.Common.Application/Hkust.Common.Application.csproj", "Services/Shared/Hkust.Common.Application/"]
COPY ["Infras/Hkust.Infras.Mapper/Hkust.Infras.Mapper.csproj", "Infras/Hkust.Infras.Mapper/"]
COPY ["Infras/Hkust.Infras.EfCore.MySQL/Hkust.Infras.EfCore.MySQL.csproj", "Infras/Hkust.Infras.EfCore.MySQL/"]
COPY ["Infras/Hkust.Infras.Repository/Hkust.Infras.Repository.csproj", "Infras/Hkust.Infras.Repository/"]
COPY ["Infras/Hkust.Infras.Dapper/Hkust.Infras.Dapper.csproj", "Infras/Hkust.Infras.Dapper/"]
COPY ["Infras/Hkust.Infras.Mongo/Hkust.Infras.Mongo.csproj", "Infras/Hkust.Infras.Mongo/"]
COPY ["Infras/Hkust.Infras.IdGenerater/Hkust.Infras.IdGenerater.csproj", "Infras/Hkust.Infras.IdGenerater/"]
COPY ["Services/Hkust.Maint/Hkust.Maint.Application.Contracts/Hkust.Maint.Application.Contracts.csproj", "Services/Hkust.Maint/Hkust.Maint.Application.Contracts/"]
COPY ["Services/Hkust.Maint/Hkust.Maint.Repository/Hkust.Maint.Repository.csproj", "Services/Hkust.Maint/Hkust.Maint.Repository/"]
RUN dotnet restore "Services/Hkust.Maint/Hkust.Maint.WebApi/Hkust.Maint.WebApi.csproj"
COPY . .
WORKDIR "/src/Services/Hkust.Maint/Hkust.Maint.WebApi"
RUN dotnet build "Hkust.Maint.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hkust.Maint.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hkust.Maint.WebApi.dll"]