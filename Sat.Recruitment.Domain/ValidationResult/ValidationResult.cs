using System;
namespace Sat.Recruitment.Domain.ValidationResult
{
    public class ValidationResult<T>
    {
        public bool IsValid { get; }
        public T Result { get; }

        public string ErrorMessage { get; }

        private ValidationResult(bool isValid, T result, string errorMessage)
        {
            IsValid = isValid;
            Result = result;
            ErrorMessage = errorMessage;
        }

        public static ValidationResult<T> Success(T result)
        {
            return new ValidationResult<T>(true, result, null);
        }

        public static ValidationResult<T> Fail(string errorMessage)
        {
            return new ValidationResult<T>(false, default(T), errorMessage);
        }
    }
}