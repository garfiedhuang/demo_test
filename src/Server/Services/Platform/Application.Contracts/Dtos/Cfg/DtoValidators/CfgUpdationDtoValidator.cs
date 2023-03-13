namespace Hkust.Platform.Application.Contracts.DtoValidators;

public class CfgUpdationDtoValidator : AbstractValidator<CfgUpdationDto>
{
    public CfgUpdationDtoValidator()
    {
        Include(new CfgCreationDtoValidator());
    }
}