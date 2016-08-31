using ServiceDomain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;

namespace ServiceDomain.ModelBinders
{
    public class BooksModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if(bindingContext.ModelType != typeof(uBookDto))
            {
                return false;
            }

            ValueProviderResult val = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if(val == null)
            {
                return false;
            }

            string key = val.RawValue as string;
            if(key == null)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Wront value type");
                return false;
            }

            uBookDto book;
            if(uBookDto.TryParse(key, out book))
            {
                bindingContext.Model = book;
                return true;
            }
            bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Cannot convert value to book");
            return false;
        }
    }
}