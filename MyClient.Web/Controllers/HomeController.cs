using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using MyClient.Web.Models;
using MyServer.Grpc;
using System.Diagnostics;

namespace MyClient.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        
        public async Task<IActionResult> Index()
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7126");
            var client = new Greeter.GreeterClient(channel);

            var grpcHeaderMetadata = new Metadata();
            grpcHeaderMetadata.Add("developer", "amin matini");

            var grpcOptions = new CallOptions(grpcHeaderMetadata, DateTime.UtcNow.AddSeconds(5));

            try
            {
                var reply = await client.SayHelloAsync(new HelloRequest { Name = "Hello To gRPC From Asp.Net Core" } 
                , new CallOptions(null , DateTime.UtcNow.AddSeconds(10)));

                ViewBag.ResponseMessage = reply.Message;
            }
            catch (RpcException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}