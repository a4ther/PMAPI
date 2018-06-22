using System;
namespace PM.Domain.Infrastructure
{
    public class ErrorHandler : IErrorHandler
    {
        public string GetMessage(ErrorMessage message)
        {
            switch (message)
            {
                case ErrorMessage.EntityNull:
                    return "The entity passed is null {0}. Additional information: {1}";

                case ErrorMessage.ModelValidation:
                    return "The request data is not correct. Additional information: {0}";

                default:
                    throw new ArgumentOutOfRangeException(nameof(message), message, null);
            }

        }
    }
}
