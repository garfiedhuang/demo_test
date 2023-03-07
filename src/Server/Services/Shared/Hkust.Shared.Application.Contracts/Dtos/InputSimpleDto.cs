﻿using System.ComponentModel.DataAnnotations;

namespace Hkust.Shared.Application.Contracts.Dtos;

/// <summary>
/// 用于解决API form post 方式接收 string,int,long等基础类型的问题。
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public class InputSimpleDto<T> : IDto
{
    /// <summary>
    /// 需要传递的值
    /// </summary>
    [Required]
    public T Value { get; set; }
}