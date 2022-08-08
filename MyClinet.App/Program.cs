using Grpc.Core;
using Grpc.Net.Client;
using MyServer.Grpc;

Console.ForegroundColor = ConsoleColor.White;

Console.WriteLine("Hello, Grpc Demo! \n");

using var channel = GrpcChannel.ForAddress("https://localhost:7126");
var client = new Greeter.GreeterClient(channel);

try
{

    var reply = await client.SayHelloListAsync(new HelloRequest { Name = "Amin Matini" }, new CallOptions(null , 
        DateTime.UtcNow.AddSeconds(10)));

    Console.ForegroundColor = ConsoleColor.Magenta;

    foreach (var msg in reply.List)
    {
        Console.WriteLine(msg);
    };

}
catch(RpcException ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(ex.Message);
}




Console.ForegroundColor = ConsoleColor.White;

