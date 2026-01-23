using CruscottoIncidenti.Application.User.Commands.CreateUser;
using FluentValidation;

namespace CruscottoIncidenti.Application.Users.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.CreatorId).NotEmpty().GreaterThan(0);

            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email should be a valid email address");

            RuleFor(x => x.FullName).NotEmpty().WithMessage("Full name can't be empty");

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username can't be empty")
                .MaximumLength(7).WithMessage("Username can't be longer than 7 characters");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password can't be empty")
                .MinimumLength(8).WithMessage("Password length should be 8 characters or longer")
                .Matches("[A-Z]+").WithMessage("Password should contain at least one uppercase character")
                .Matches("[a-z]+").WithMessage("Password should contain at least one lowercase character")
                .Matches(@"\d").WithMessage("Password should contain at least one numeric character")
                .Matches(@"^(?=.*\W)(?=\S+$).*").WithMessage("Password should contain at least one special character");

            RuleFor(x => x.Roles).Must(roles => roles != null && roles.Count > 0)
                .WithMessage("User should have at least one role");
        }
    }
}
