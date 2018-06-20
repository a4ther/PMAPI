using System;
namespace PMAPI.Domain.Infrastructure
{
    public interface IErrorHandler
    {
        string GetMessage(ErrorMessage message);
    }
}
