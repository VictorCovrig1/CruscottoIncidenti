using System.Linq;
using System.Web.Mvc;
using FluentValidation.Results;
using static CruscottoIncidenti.Common.Constants;

namespace CruscottoIncidenti.Utils
{
    public static class ModelStateExtensions 
    {
        public static void ValidatePasswordCharRules<T>(this ModelStateDictionary modelState, ValidationResult validationResult, T model)
        {
            if (!validationResult.IsValid)
            {
                var passwordErrorMessage = string.Join(", ", 
                    validationResult.Errors.Select(x => x.ErrorMessage));

                modelState.AddModelError("Password", $"{PasswordGenericMessage}: {passwordErrorMessage}");
            }
        }
    }
}