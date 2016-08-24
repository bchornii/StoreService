using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace ServiceDomain.HttpActionResultFactory
{
    public class CustomResult<T> : IHttpActionResult where T : class
    {
        ApiController _ctrl;
        T _obj;
        public CustomResult(ApiController ctrl, T obj)
        {
            _ctrl = ctrl;
            _obj = obj;
        }
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            IContentNegotiator negotiator = _ctrl.Configuration.Services.GetContentNegotiator();
            var result = negotiator.Negotiate(_obj.GetType(), 
                                              _ctrl.Request, 
                                              _ctrl.Configuration.Formatters);
            if(result == null)
            {
                var response = new HttpResponseMessage(System.Net.HttpStatusCode.NotAcceptable);
                throw new HttpResponseException(response);
            }
            return Task.FromResult(new HttpResponseMessage
            {
                Content = new ObjectContent(_obj.GetType(), 
                                            _obj, 
                                            result.Formatter, 
                                            result.MediaType.MediaType)
            });
        }
    }
}