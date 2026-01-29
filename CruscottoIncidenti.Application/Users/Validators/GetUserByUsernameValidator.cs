using CruscottoIncidenti.Application.User.Queries;
using FluentValidation;

namespace CruscottoIncidenti.Application.Users.Validators
{
    public class GetUserByUsernameValidator : AbstractValidator<GetUserByUserNameQuery>
    {
        public GetUserByUsernameValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username can't be empty");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password can't be empty");
        }
    }
}
