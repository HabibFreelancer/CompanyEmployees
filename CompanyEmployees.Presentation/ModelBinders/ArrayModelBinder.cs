using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.Presentation.ModelBinders
{
    /*custom model binding :bind the string type parameter to the
IEnumerable<Guid>
     ArrayModelBinder will be triggered before an action executes. It
will convert the sent string parameter to the IEnumerable<Guid> type,
and then the action will be executed*/
    public class ArrayModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (!bindingContext.ModelMetadata.IsEnumerableType)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }
            var providedValue = bindingContext.ValueProvider
            .GetValue(bindingContext.ModelName)
            .ToString();
            if (string.IsNullOrEmpty(providedValue))
            {
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }
            var genericType =
            bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];
            var converter = TypeDescriptor.GetConverter(genericType);
            var objectArray = providedValue.Split(new[] { "," },
            StringSplitOptions.RemoveEmptyEntries)
            .Select(x => converter.ConvertFromString(x.Trim()))
            .ToArray();
            var guidArray = Array.CreateInstance(genericType, objectArray.Length);
            objectArray.CopyTo(guidArray, 0);
            bindingContext.Model = guidArray;
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
            return Task.CompletedTask;
        }
    }
}