using FluentValidation;

namespace CruscottoIncidenti.Application.User.Commands.UpdateUser.Validation
{
    public class UpdateUserValidation : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserValidation() 
        {
            RuleFor(x => x.EditorId).NotEmpty().GreaterThan(0);

            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email should be a valid email address");

            RuleFor(x => x.FullName).NotEmpty().WithMessage("Full name can't be empty");

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username can't be empty")
                .MaximumLength(7).WithMessage("Username can't be longer than 7 characters");
        }
    }
}
