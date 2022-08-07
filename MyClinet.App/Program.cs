using Grpc.Core;
using Grpc.Net.Client;
using MyServer.Grpc;
using Polly;

Console.WriteLine("Hello, Grpc Demo! \n");

using var channel = GrpcChannel.ForAddress("https://localhost:7126");
var client = new Greeter.GreeterClient(channel);

#region Poly Config

var maxRetryAttempts = 10;
var pauseBetweenFailures = TimeSpan.FromSeconds(3);

var retryPolicy = Policy.Handle<RpcException>()
  .WaitAndRetryAsync(maxRetryAttempts, i => pauseBetweenFailures, (ex, pause) => {
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.WriteLine(ex.Message + " => " + pause.TotalSeconds);
  });

await retryPolicy.ExecuteAsync(async () =>
{
    var reply = await client.SayHelloAsync(new HelloRequest { Name = "Amin Matini" },
        new CallOptions(null,DateTime.UtcNow.AddSeconds(1)));

    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine("reply message : \n");

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(reply.Message);

});

#endregion

