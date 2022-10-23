using FluentValidation;

namespace NZWalks.API.Validators
{
    public class LoginRequestValidator: AbstractValidator<Models.DTO.LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.username).NotEmpty();
            RuleFor(x=>x.password).NotEmpty();
        }
    }
}
