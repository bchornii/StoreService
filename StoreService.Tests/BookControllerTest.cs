using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceDomain.Controllers;
using System.Web.Http.Results;
using ServiceDomain.DTOs;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Http;

namespace StoreService.Tests
{
    [TestClass]
    public class BookControllerTest
    {
        BooksController controller;

        [TestInitialize]
        public void TestInitialize()
        {
            // Global arrange
            controller = new BooksController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
        }

        [TestMethod]
        public async Task GetBook_IsOk()
        {
            // Act
            var result = await controller.GetBook(1);

            // Assert            
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<gBookDto>));
        }
    }
}
