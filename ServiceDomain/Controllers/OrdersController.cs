using ServiceDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ServiceDomain.Filters;
using System.Net;

namespace ServiceDomain.Controllers
{
    [RoutePrefix("api/orders")]
    public class OrdersController : ApiController
    {
        private readonly List<Order> _orders = new List<Order>
        {
            new Order { OrderId = 10248, CustomerId = 1, EmployeeId = 1, Freight = 12.9m, OrderDate = DateTime.Now.AddDays(10) },
            new Order { OrderId = 10249, CustomerId = 1, EmployeeId = 1, Freight = 12.9m, OrderDate = DateTime.Now.AddDays(10) },
            new Order { OrderId = 10250, CustomerId = 1, EmployeeId = 1, Freight = 12.9m, OrderDate = DateTime.Now.AddDays(10) },
        };

        [HttpGet]
        [Route("all")]
        public IHttpActionResult GetAll()
        {
            return Ok(_orders);
        }

        [HttpGet]
        [Route("{name}/{id:int:nonzero}")]
        public IHttpActionResult GetOrderById(string Name, int Id, double version = 1.5)
        {
            var order = _orders.FirstOrDefault(o => o.OrderId == Id);
            if (order == null)
            {
                //return NotFound();
            }
            return Ok(Id + "," + Name + "," + version);
        }

        [HttpGet]
        [Route("exception")]
        public IHttpActionResult GetException()
        {
            throw new HttpResponseException(new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.Conflict,
                Content = new StringContent(string.Format("No product with ID = {0}", -1)),
                ReasonPhrase = "Because it is a test"
            });
        }

        [HttpGet]
        [Route("excfilter")]
        [NotImpExceptionFilter]
        public IHttpActionResult GetExceptionFilter()
        {
            throw new NotImplementedException("This method is not implementer");
        }

        [HttpGet]
        [Route("exchttperr")]
        public HttpResponseMessage GetHttpErrorException()
        {
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Order is not found");
        }
    }
}
