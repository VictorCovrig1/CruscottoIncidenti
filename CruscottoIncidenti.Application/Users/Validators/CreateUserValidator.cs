using CruscottoIncidenti.Application.Users.ViewModels;
using FluentValidation;
using static CruscottoIncidenti.Common.Constants;

namespace CruscottoIncidenti.Application.Users.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUserViewModel>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email can't be empty")
                .EmailAddress().WithMessage("Email should be a valid email address");

            RuleFor(x => x.FullName).NotEmpty().WithMessage("Full name can't be empty");

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username can't be empty")
                .MaximumLength(7).WithMessage("Username can't be longer than 7 characters");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password can't be empty");

            RuleSet(PasswordRuleSet, () =>
            {
                RuleFor(x => x.Password)
                    .MinimumLength(8).WithMessage("8 characters")
                    .Matches("[A-Z]+").WithMessage("one uppercase character")
                    .Matches("[a-z]+").WithMessage("one lowercase character")
                    .Matches(@"\d").WithMessage("one numeric character")
                    .Matches(@"^(?=.*\W)(?=\S+$).*").WithMessage("one special character");
            });

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm Password can't be empty")
                .Equal(x => x.Password).WithMessage("Passswords doesn't match");

            RuleFor(x => x.Roles).Must(roles => roles != null && roles.Count > 0)
                .WithMessage("User should have at least one role");
        }
    }
}
