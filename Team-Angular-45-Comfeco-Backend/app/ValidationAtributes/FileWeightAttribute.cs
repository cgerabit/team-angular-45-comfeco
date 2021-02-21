using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackendComfeco.ValidationAtributes
{
    public class FileWeightAttribute : ValidationAttribute
    {
        private readonly double maxWeightInKb;

        public FileWeightAttribute(double weightInKb)
        {
            maxWeightInKb = weightInKb;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            // we see if its IEnumerable

            if (value is not IEnumerable<IFormFile> Enumerable)
            {
                // We see if it's form file 
                if (value is not IFormFile FormFile)
                {
                    return ValidationResult.Success;
                }

                if (!CheckWeight(FormFile.Length))
                {
                    return new ValidationResult($"The max weight defined is {maxWeightInKb} Kb");
                }
            }
            else
            {
                bool Exced = Enumerable.Any(file => !CheckWeight(file.Length));

                if (Exced)
                {
                    return new ValidationResult($"The max weight defined is {maxWeightInKb} kb");
                }

            }
            return ValidationResult.Success;
        }

        private bool CheckWeight(double fileLength) =>
            (fileLength / 1024) < maxWeightInKb;
    }
}
