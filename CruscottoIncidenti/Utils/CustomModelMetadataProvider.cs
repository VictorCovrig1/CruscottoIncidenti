using System;
using System.Web.ModelBinding;

namespace CruscottoIncidenti.Utils
{
    public class CustomModelMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        public override ModelMetadata GetMetadataForProperty(Func<object> modelAccessor, Type containerType, string propertyName)
        {
            var metadata = base.GetMetadataForProperty(modelAccessor, containerType, propertyName);

            if (string.IsNullOrEmpty(metadata.DisplayName))
                metadata.DisplayName = SplitCamelCase(propertyName);

            return metadata;
        }

        private string SplitCamelCase(string propertyName)
        {
            return System.Text.RegularExpressions.Regex.Replace(
                propertyName, "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1 ");
        }
    }
}