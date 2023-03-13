using Hkust.Common.Rpc.Grpc.Messages;

namespace Hkust.Platform.Application.AutoMapper;

public sealed class PlatformProfile : Profile
{
    public PlatformProfile()
    {
        CreateMap(typeof(PagedModel<>), typeof(PageModelDto<>)).ForMember("XData", opt => opt.Ignore());

        #region USR模块

        CreateMap(typeof(ZTreeNodeDto<,>), typeof(Node<>)).IgnoreAllPropertiesWithAnInaccessibleSetter();
        CreateMap<MenuCreationDto, SysMenu>();
        CreateMap<MenuUpdationDto, SysMenu>();
        CreateMap<SysMenu, MenuDto>().ReverseMap();
        CreateMap<MenuDto, MenuRouterDto>();
        CreateMap<SysMenu, MenuRouterDto>();
        CreateMap<SysMenu, MenuNodeDto>();
        CreateMap<MenuDto, MenuNodeDto>();
        CreateMap<SysRelation, RelationDto>();
        CreateMap<RoleCreationDto, SysRole>();
        CreateMap<RoleUpdationDto, SysRole>();
        CreateMap<SysRole, RoleDto>().ReverseMap();
        CreateMap<UserCreationDto, SysUser>();
        CreateMap<UserUpdationDto, SysUser>();
        CreateMap<SysUser, UserDto>().ForMember(dest => dest.Password, opt => opt.Ignore());
        CreateMap<DeptCreationDto, SysDept>();
        CreateMap<DeptUpdationDto, SysDept>();
        CreateMap<SysDept, DeptDto>();
        CreateMap<SysDept, DeptTreeDto>();

        CreateMap<LoginRequest, UserLoginDto>();
        CreateMap<DeptTreeDto, DeptReply>();

        #endregion

        #region MAINT模块

        CreateMap<OpsLogCreationDto, OperationLog>();
        CreateMap<OperationLog, OpsLogDto>();
        CreateMap<LoginLog, LoginLogDto>();
        CreateMap<LoggerLog, NlogLogDto>();
        CreateMap<SysNloglogProperty, NlogLogPropertyDto>();
        CreateMap<CfgCreationDto, SysCfg>();
        CreateMap<SysCfg, CfgDto>();

        CreateMap<DictCreationDto, SysDict>();
        CreateMap<SysDict, DictDto>();
        CreateMap<SysNotice, NoticeDto>().ReverseMap();

        CreateMap<DictDto, DictReply>();

        #endregion
    }
}