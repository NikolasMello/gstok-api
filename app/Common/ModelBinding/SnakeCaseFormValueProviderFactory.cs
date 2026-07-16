using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace gstok_api.Common.ModelBinding;

public class SnakeCaseFormValueProviderFactory : IValueProviderFactory
{
    public async Task CreateValueProviderAsync(ValueProviderFactoryContext context)
    {
        var request = context.ActionContext.HttpContext.Request;
        if (!request.HasFormContentType)
            return;

        var form = await request.ReadFormAsync();
        context.ValueProviders.Add(new SnakeCaseFormValueProvider(form));
    }
}
