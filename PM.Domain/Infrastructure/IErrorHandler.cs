using System;
namespace PM.Domain.Infrastructure
{
    public interface IErrorHandler
    {
        string GetMessage(ErrorMessage message);
    }
}
