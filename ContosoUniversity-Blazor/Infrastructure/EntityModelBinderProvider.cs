using ContosoUniversity_Blazor.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ContosoUniversity_Blazor.Infrastructure
{
    public class EntityModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            return typeof(IEntity).IsAssignableFrom(context.Metadata.ModelType) ? new EntityModelBinder() : null;
        }
    }
}