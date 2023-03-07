﻿using Hkust.Shared.Consts.AppSettings;
using Microsoft.Extensions.Configuration;

namespace Hkust.UnitTest.Fixtures;

public class RedisCacheFixture
{
    public IServiceProvider Container { get; init; }

    public RedisCacheFixture()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var redisSection = configuration.GetSection(NodeConsts.Redis);
        var cachingSection = configuration.GetSection(NodeConsts.Caching);

        var services = new ServiceCollection().AddHkustInfraRedisCaching(redisSection, cachingSection);
        Container = services.BuildServiceProvider();
    }
}