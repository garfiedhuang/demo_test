namespace Hkust.Usr.Application.Contracts.DtoValidators;

public class MenuUpdationDtoValidator : AbstractValidator<MenuUpdationDto>
{
    public MenuUpdationDtoValidator()
    {
        Include(new MenuCreationDtoValidator());
    }
}