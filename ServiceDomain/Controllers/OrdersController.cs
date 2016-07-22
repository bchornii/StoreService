using ServiceDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceDomain.Controllers
{
    public class OrdersController : ApiController
    {
        private readonly List<Order> _orders = new List<Order>
        {
            new Order { OrderId = 10248, CustomerId = 1, EmployeeId = 1, Freight = 12.9m, OrderDate = DateTime.Now.AddDays(10) },
            new Order { OrderId = 10249, CustomerId = 1, EmployeeId = 1, Freight = 12.9m, OrderDate = DateTime.Now.AddDays(10) },
            new Order { OrderId = 10250, CustomerId = 1, EmployeeId = 1, Freight = 12.9m, OrderDate = DateTime.Now.AddDays(10) },
        };

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            return Ok(_orders);
        }

        [HttpGet]
        public IHttpActionResult GetOrderById(int Id)
        {
            var order = _orders.FirstOrDefault(o => o.OrderId == Id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }
    }
}
