using ExcHandler = System.Web.Http.ExceptionHandling;

namespace ServiceDomain.ExceptionHandler
{
    public class UnhandledExceptionHandler : ExcHandler.ExceptionHandler
    {
        public IExceptionHandlerCustomizer Customizer { get; set; }
        public override void Handle(ExcHandler.ExceptionHandlerContext context)
        {
            var type = context.Exception.GetType();
            var statusCode = Customizer.GetStatusCode(type);
            var message = Customizer.GetMessage(type);
            message = message != string.Empty ? message : context.Exception.Message;

            context.Result = new UnhandledExceptionResultMessageFactory
            {
                Message = message,
                StatusCode = statusCode,
                Request = context.ExceptionContext.Request
            };
        }
    }
}