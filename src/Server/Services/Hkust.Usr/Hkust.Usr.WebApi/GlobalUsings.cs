﻿global using Hkust.Infra.Core.Configuration;
global using Hkust.Shared;
global using Hkust.Shared.Application.Contracts.Dtos;
global using Hkust.Shared.Consts.Permissions.Usr;
global using Hkust.Shared.WebApi.Authentication;
global using Hkust.Shared.WebApi.Authentication.JwtBearer;
global using Hkust.Shared.WebApi.Authorization;
global using Hkust.Usr.Application.Contracts.Dtos;
global using Hkust.Usr.Application.Contracts.Services;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Options;
global using System.IdentityModel.Tokens.Jwt;
