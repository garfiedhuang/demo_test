﻿global using Hkust.Infras.Core.Configuration;
global using Hkust.Common;
global using Hkust.Common.Application.Contracts.Dtos;
global using Hkust.Common.Consts.Permissions.Usr;
global using Hkust.Common.WebApi.Authentication;
global using Hkust.Common.WebApi.Authentication.JwtBearer;
global using Hkust.Common.WebApi.Authorization;
global using Hkust.Platform.Application.Contracts.Dtos;
global using Hkust.Platform.Application.Contracts.Services;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Options;
global using System.IdentityModel.Tokens.Jwt;
