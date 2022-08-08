using Grpc.Net.Client;
using MyServer;
using static MyServer.Numerics;

Console.ForegroundColor = ConsoleColor.White;

Console.WriteLine("Hello, Grpc Demo! \n");

using var channel = GrpcChannel.ForAddress("https://localhost:7126");

var client = new Numerics.NumericsClient(channel);

await StreamNumbersFromClientToServer(client);

static async Task StreamNumbersFromClientToServer(NumericsClient client)
{

    Random Random = new Random();

    using (var call = client.SendNumberFromClientToServer())
    {
        for(var i = 0; i<= 10; i++)
        {
            var number = Random.Next(10);

            Console.WriteLine($"Sending Number : {number}");

            await call.RequestStream.WriteAsync(new NumberRequest() { Value = number });

            await Task.Delay(1000);
        };

        await call.RequestStream.CompleteAsync();

        var response = await call;

        Console.WriteLine($"Result : {response.Result}");
    }    
}
