using Grpc.Core;

namespace MyServer.Grpc.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            Task.Delay(7000);

            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }

        public override async Task<HelloReplyList> SayHelloList(HelloRequest request, ServerCallContext context)
        {
            var res = new HelloReplyList();

            await Task.Delay(1000);

            for(var i = 0; i < 10; i++)
            {
                res.List.Add("hello amin matini => " + i.ToString());
            };

            return res;

        }
    }
}