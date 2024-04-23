using System.ComponentModel.DataAnnotations;

namespace template_dotnet8_api.Attributes
{
    public class FileSizeValidatorAttribute : ValidationAttribute
    {
        private readonly int _maxFileSizeInMbs;

        public FileSizeValidatorAttribute(int maxFileSizeInMbs)
        {
            _maxFileSizeInMbs = maxFileSizeInMbs;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            IFormFile formFile = value as IFormFile;

            if (formFile == null)
            {
                return ValidationResult.Success;
            }

            if (formFile.Length > _maxFileSizeInMbs * 1024 * 1024)
            {
                return new ValidationResult($"File size can't bigger than {_maxFileSizeInMbs} Megabytes");
            }

            return ValidationResult.Success;
        }
    }
}
