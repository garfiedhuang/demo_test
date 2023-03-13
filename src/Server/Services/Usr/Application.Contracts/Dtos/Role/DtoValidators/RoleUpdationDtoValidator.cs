namespace Hkust.Usr.Application.Contracts.DtoValidators;

public class RoleUpdationDtoValidator : AbstractValidator<RoleUpdationDto>
{
    public RoleUpdationDtoValidator()
    {
        Include(new RoleCreationDtoValidator());
    }
}