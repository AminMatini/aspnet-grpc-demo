using Grpc.Core;

namespace MyServer.Grpc.Services
{
    public class NumberService : Numerics.NumericsBase
    {
        private readonly ILogger<NumberService> _logger;
        public NumberService(ILogger<NumberService> logger)
        {
            _logger = logger;
        }

        public override async Task SendNumberFromServerToClient(NumberRequest request, IServerStreamWriter<NumberResponse> responseStream, ServerCallContext context)
        {
            Random random = new Random();

            for(var i = 0; i< request.Value; i++)
            {
                var number = random.Next(10);

                _logger.LogInformation($"Sending : {number}");

                await responseStream.WriteAsync(new NumberResponse { Result = number});

                await Task.Delay(1000);
            }

            _logger.LogWarning(request.Value.ToString());
        }

    }
}
