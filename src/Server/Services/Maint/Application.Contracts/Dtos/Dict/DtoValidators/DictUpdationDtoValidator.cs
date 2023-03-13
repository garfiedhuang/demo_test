namespace Hkust.Maint.Application.Contracts.DtoValidators;

public class DictUpdationDtoValidator : AbstractValidator<DictUpdationDto>
{
    public DictUpdationDtoValidator()
    {
        Include(new DictCreationDtoValidator());
    }
}