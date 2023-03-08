﻿using Hkust.Common.Consts.Permissions.Cust;
using Hkust.Common.WebApi.Authorization;

namespace Hkust.Cus.WebApi.Controllers;

/// <summary>
/// 客户管理
/// </summary>
[Route("cus/customers")]
[ApiController]
public class CustomerController : HkustControllerBase
{
    private readonly ICustomerAppService _customerService;

    public CustomerController(ICustomerAppService customerService) => _customerService = customerService;

    /// <summary>
    /// 注册
    /// </summary>
    /// <param name="input"><see cref="CustomerRegisterDto"/></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<CustomerDto>> RegisterAsync([FromBody] CustomerRegisterDto input) =>
        CreatedResult(await _customerService.RegisterAsync(input));

    /// <summary>
    /// 后台管理员给客户充值
    /// </summary>
    /// <returns></returns>
    [HttpPut("{id}/balance")]
    [HkustAuthorize(PermissionConsts.Customer.Recharge)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<SimpleDto<string>>> RechargeAsync([FromRoute] long id, [FromBody] CustomerRechargeDto input) =>
        Result(await _customerService.RechargeAsync(id, input));

    /// <summary>
    /// 客户分页列表
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    [HttpGet("page")]
    [HkustAuthorize(PermissionConsts.Customer.GetList)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PageModelDto<CustomerDto>>> GetPagedAsync([FromQuery] CustomerSearchPagedDto search) =>
        Result(await _customerService.GetPagedAsync(search));
}