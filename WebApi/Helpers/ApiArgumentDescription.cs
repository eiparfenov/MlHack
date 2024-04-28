using Microsoft.OpenApi.Models;

namespace MessagesBackend.Helpers;

public static class ApiArgumentDescription
{
    public static OpenApiOperation AddArgumentDescription(this OpenApiOperation openApiOperation, string argumentName,
        string description)
    {
        openApiOperation.Parameters.Single(p => p.Name == argumentName).Description = description;
        return openApiOperation;
    }
}