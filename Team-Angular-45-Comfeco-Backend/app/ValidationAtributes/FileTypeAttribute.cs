using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BackendComfeco.ValidationAtributes
{
    public class FileTypeAttribute : ValidationAttribute
    {
        private readonly string[] ValidTypes;

        public FileTypeAttribute(string[] validTypes)
        {
            this.ValidTypes = validTypes;
        }

        public FileTypeAttribute(FileTypeGroup fileGroupType)
        {
            if (fileGroupType == FileTypeGroup.Image)
            {
                ValidTypes = new string[] { "image/jpeg", "image/png", "image/gif" };
            }
            else if (fileGroupType == FileTypeGroup.Zip)
            {
                ValidTypes = new string[] { "application/zip", "application/x-zip-compressed" };
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is not IEnumerable<IFormFile> enumerable)
            {
                if (value is not IFormFile formFile)
                {
                    return ValidationResult.Success;
                }

                

                bool validExtension = ValidTypes.Contains(formFile.ContentType);

                if (!validExtension)
                {
                    return new ValidationResult($"The valid extensions are ${string.Join(",", ValidTypes)}");
                }

            }
            else
            {
                bool IsInvalidFile = enumerable.Any(file => !ValidTypes.Contains(file.ContentType));

                if (IsInvalidFile)
                {
                    return new ValidationResult($"The valid extensions are ${string.Join(",", ValidTypes)}");
                }

            }




            return ValidationResult.Success;
        }
    }
}
