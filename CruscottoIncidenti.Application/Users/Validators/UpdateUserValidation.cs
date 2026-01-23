using CruscottoIncidenti.Application.User.Commands.UpdateUser;
using FluentValidation;

namespace CruscottoIncidenti.Application.Users.Validators
{
    public class UpdateUserValidation : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserValidation() 
        {
            RuleFor(x => x.EditorId).NotEmpty().GreaterThan(0);

            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email should be a valid email address");

            RuleFor(x => x.FullName).NotEmpty().WithMessage("Full name can't be empty");

            RuleFor(x => x.Roles).Must(roles => roles != null && roles.Count > 0)
                .WithMessage("User should have at least one role");
        }
    }
}
