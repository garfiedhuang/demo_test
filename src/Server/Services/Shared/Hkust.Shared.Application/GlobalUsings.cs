﻿global using Hkust.Infras.Core.Configuration;
global using Hkust.Infras.Core.DependencyInjection;
global using Hkust.Infras.Core.Interfaces;
global using Hkust.Infras.Core.Json;
global using Hkust.Infras.Entities;
global using Hkust.Infras.Helper;
global using Hkust.Infras.IdGenerater.Yitter;
global using Hkust.Infras.IRepositories;
global using Hkust.Infras.Mapper;
global using Hkust.Infras.Redis;
global using Hkust.Infras.Redis.Caching;
global using Hkust.Infras.Redis.Caching.Core;
global using Hkust.Infras.Redis.Caching.Core.Diagnostics;
global using Hkust.Infras.Redis.Caching.Interceptor.Castle;
global using Hkust.Infras.Redis.Configurations;
global using Hkust.Infras.Redis.Core;
global using Hkust.Shared.Application.BloomFilter;
global using Hkust.Shared.Application.Caching;
global using Hkust.Shared.Application.Contracts.Attributes;
global using Hkust.Shared.Application.Contracts.Enums;
global using Hkust.Shared.Application.Contracts.Interfaces;
global using Hkust.Shared.Application.Contracts.ResultModels;
global using Hkust.Shared.Application.Interceptors;
global using Hkust.Shared.Consts.Caching.Common;
global using Hkust.Shared.Repository.EfEntities;
global using Hkust.Shared.Repository.MongoEntities;
global using Castle.DynamicProxy;
global using FluentValidation;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
global using Polly;
global using SkyApm;
global using SkyApm.Common;
global using SkyApm.Config;
global using SkyApm.Diagnostics;
global using SkyApm.Tracing;
global using SkyApm.Tracing.Segments;
global using SkyApm.Utilities.DependencyInjection;
global using System.Diagnostics.CodeAnalysis;
global using System.Linq.Expressions;
global using System.Net;
global using System.Reflection;
global using System.Text.Json;
