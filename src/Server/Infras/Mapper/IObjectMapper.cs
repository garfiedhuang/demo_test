﻿namespace Hkust.Infras.Mapper;

public interface IObjectMapper
{
    TDestination? Map<TDestination>(object source);

    TDestination? Map<TSource, TDestination>(TSource source);

    TDestination? Map<TSource, TDestination>(TSource source, TDestination destination);
}