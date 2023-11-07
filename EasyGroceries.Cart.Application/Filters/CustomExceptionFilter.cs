using EasyGroceries.Cart.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Cart.Application.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            switch (filterContext.Exception)
            {
                case ArgumentException:
                    filterContext.Result = OnArgumentException(filterContext);
                    break;

                case InvalidOperationException:
                    filterContext.Result = OnInvalidOperationException(filterContext);
                    break;

                case UnauthorizedAccessException:
                    filterContext.Result = OnUnAutorizedException(filterContext);
                    break;

                default:
                    filterContext.Result = OnGenericException(filterContext);
                    break;
            }
        }

        private ObjectResult OnArgumentException(ExceptionContext context)
        {
            var objectResult = new ObjectResult(new
            {
                Message = "An error occurred due to invalid arguments",
                Detail = context.Exception.Message,
                StatusCode = (int)HttpStatusCode.BadRequest
            });

            return objectResult;
        }

        private ObjectResult OnInvalidOperationException(ExceptionContext context)
        {
            var objectResult = new ObjectResult(new
            {
                Message = "An error occurred due to invalid operation",
                Detail = context.Exception.Message,
                StatusCode = (int)HttpStatusCode.NotFound
            });

            return objectResult;
        }

        private ObjectResult OnUnAutorizedException(ExceptionContext context)
        {
            var objectResult = new ObjectResult(new
            {
                Message = "An error occurred due to unauthorized access",
                Detail = context.Exception.Message,
                StatusCode = (int)HttpStatusCode.Unauthorized
            });

            return objectResult;
        }

        private ObjectResult OnGenericException(ExceptionContext context)
        {
            var objectResult = new ObjectResult(new
            {
                Message = "An internal server error occurred",
                Detail = context.Exception.Message,
                StatusCode = (int)HttpStatusCode.InternalServerError
            });

            return objectResult;
        }

    }
}
