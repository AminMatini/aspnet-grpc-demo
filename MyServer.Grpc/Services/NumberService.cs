using Grpc.Core;

namespace MyServer.Grpc.Services
{
    public class NumberService : Numeric.NumericBase
    {
        #region Constructor

        private readonly ILogger<NumberService> _logger;
        public NumberService(ILogger<NumberService> logger)
        {
            _logger = logger;
        }

        #endregion

        public override async Task BidirectionalStreamingNumber(
            IAsyncStreamReader<NumberRequest> requestStream,
            IServerStreamWriter<NumberResponse> responseStream, ServerCallContext context)
        {
            #region guidance

            // دریافت به صورت استریم از سمت کلاینت 
            // تغییر دیتا در سمت سرور 
            // ارسال دیتای تغییر کرده به صورت استریم به سمت کلاینت

            #endregion

            await foreach (var number in requestStream.ReadAllAsync())
            {
                _logger.LogInformation($"Received Number -> {number.Value}");

                await Task.Delay(new Random().Next(1, 5) * 100);

                var response = new NumberResponse
                {
                    Result = number.Value * number.Value
                };

                await responseStream.WriteAsync(response);
            }
        }
    }
}
