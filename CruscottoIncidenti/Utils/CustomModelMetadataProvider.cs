using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using CruscottoIncidenti.Application.User.Commands;

namespace CruscottoIncidenti.Utils
{
    public class CustomModelMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        public override ModelMetadata GetMetadataForProperty(Func<object> modelAccessor, Type containerType, string propertyName)
        {
            var metadata = base.GetMetadataForProperty(modelAccessor, containerType, propertyName);

            if (string.IsNullOrEmpty(metadata.DisplayName))
                metadata.DisplayName = SplitCamelCase(propertyName);

            if (propertyName == nameof(UpdateUserCommand.IsPasswordEnabled))
                metadata.DisplayName = "Change Password";

            return metadata;
        }

        private string SplitCamelCase(string propertyName)
        {
            string propertyWithoutId = Regex.Replace(propertyName, @"(?<!^)Id", string.Empty);
            return Regex.Replace(propertyWithoutId, "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1 ");
        }
    }
}