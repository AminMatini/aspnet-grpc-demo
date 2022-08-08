using Grpc.Core;
using Grpc.Net.Client;
using MyServer;
using static MyServer.Numeric;

Console.ForegroundColor = ConsoleColor.White;

Console.WriteLine("Hello, Grpc Demo! \n");

using var channel = GrpcChannel.ForAddress("https://localhost:7126");
var client = new Numeric.NumericClient(channel);

await StreamNumberFromClientToServer(client);

static async Task StreamNumberFromClientToServer(NumericClient client)
{
    using var call = client.BidirectionalStreamingNumber();

    #region guidance

    // ارسال دیتا در وظیفه شماره یک به سمت سرور به صورت استریم جهت پردازش
    // در وظیفه شماره دو دریافت دیتای پردازش شده از سرور به صورت استریم

    #endregion

    var task1 = Task.Run(async () =>
    {
        for (var i = 0; i <= 100; i++)
        {
            var number = new Random().Next(10);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Sending : {number}");

            await call.RequestStream.WriteAsync(new NumberRequest() { Value = number });

            await Task.Delay(new Random().Next(10) * 100);

        }
    });

    var task2 = Task.Run(async () =>
    {
        await foreach(var data in call.ResponseStream.ReadAllAsync())
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Received[{data.Index}] by Power: {Math.Sqrt(data.Result)} -> {data.Result}");
        };
    });

    Console.ForegroundColor = ConsoleColor.White;
    await Task.WhenAll(task1 , task2);
}
