using Grpc.Core;
using Grpc.Net.Client;
using MyServer;

Console.ForegroundColor = ConsoleColor.White;

Console.WriteLine("Hello, Grpc Demo! \n");

using var channel = GrpcChannel.ForAddress("https://localhost:7126");

var client = new Numerics.NumericsClient(channel);

var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));

var request = new NumberRequest() { Value = 20 };

using var streamingCall = client.SendNumberFromServerToClient(request, 
    cancellationToken: cts.Token);

try
{
    await foreach(var number in streamingCall.ResponseStream.ReadAllAsync(cancellationToken:cts.Token))
    {
        Console.WriteLine(number.Result);
    };
}
catch(RpcException ex)
{
    Console.WriteLine(ex.Message);
}
