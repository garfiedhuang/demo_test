namespace Hkust.Platform.Application.Contracts.DtoValidators
{
    public class UserUpdationDtoValidator : AbstractValidator<UserUpdationDto>
    {
        public UserUpdationDtoValidator()
        {
            Include(new UserCreationAndUpdationDtoValidator());
        }
    }
}