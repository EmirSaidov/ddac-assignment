using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_Assignment_Mining_Commerce.Models
{
    public class ModelBinder: IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (!context.Metadata.IsComplexType && (context.Metadata.ModelType == typeof(double) || context.Metadata.ModelType == typeof(double?)))
            {
                return new InvariantDoubleModelBinder(context.Metadata.ModelType);
            }

            return null;
        }

        public class InvariantDoubleModelBinder : IModelBinder
        {
            private readonly SimpleTypeModelBinder _baseBinder;

            public InvariantDoubleModelBinder(Type modelType)
            {
                _baseBinder = new SimpleTypeModelBinder(modelType, LoggerFactory.Create(
                    builder => {
                        builder.AddFilter("Microsoft", LogLevel.Warning)
                               .AddFilter("System", LogLevel.Warning)
                               .AddConsole();
                    }));
            }

            public Task BindModelAsync(ModelBindingContext bindingContext)
            {
                if (bindingContext == null) throw new ArgumentNullException(nameof(bindingContext));

                var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

                if (valueProviderResult != ValueProviderResult.None)
                {
                    bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

                    var valueAsString = valueProviderResult.FirstValue;
                    double result;

                    // Use invariant culture
                    if (double.TryParse(valueAsString, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out result))
                    {
                        bindingContext.Result = ModelBindingResult.Success(result);
                        return Task.CompletedTask;
                    }
                }

                // If we haven't handled it, then we'll let the base SimpleTypeModelBinder handle it
                return _baseBinder.BindModelAsync(bindingContext);
            }
        }
    }
}
