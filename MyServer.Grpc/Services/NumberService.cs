using Grpc.Core;

namespace MyServer.Grpc.Services
{
    public class NumberService : Numerics.NumericsBase
    {
        #region Constructor

        private readonly ILogger<NumberService> _logger;
        public NumberService(ILogger<NumberService> logger)
        {
            _logger = logger;
        }

        #endregion

        public override async Task<NumberResponse> SendNumberFromClientToServer(
            IAsyncStreamReader<NumberRequest> requestStream, 
            ServerCallContext context)
        {
            var total = 0;

            await foreach(var number  in requestStream.ReadAllAsync())
            {
                _logger.LogInformation("Received Number -> " + number.Value);

                total += number.Value;
            };

            return new NumberResponse { Result = total };
        }
    }
}
