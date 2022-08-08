using Grpc.Core;
using Grpc.Net.Client;
using MyServer.Grpc;

Console.ForegroundColor = ConsoleColor.White;

Console.WriteLine("Hello, Grpc Demo! \n");

using var channel = GrpcChannel.ForAddress("https://localhost:7126");

var client = new Greeter.GreeterClient(channel);

var grpcHeaderMetadata = new Metadata();
grpcHeaderMetadata.Add("founder", "amin matini");
grpcHeaderMetadata.Add("co-funder", "amin matini");
grpcHeaderMetadata.Add("autor", "amin matini");
grpcHeaderMetadata.Add("developer", "amin matini");

var grpcOptions = new CallOptions(grpcHeaderMetadata , DateTime.UtcNow.AddSeconds(5));

var source = new CancellationTokenSource();
var token = source.Token;

try
{
    //source.CancelAfter(1);

    var reply = await client.SayHelloAsync(new HelloRequest { Name = "Amin Matini" }, grpcHeaderMetadata , 
        DateTime.UtcNow.AddSeconds(2) , token);

    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine("reply message : \n");

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(reply.Message);
}
catch(RpcException ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(ex.Message);
}

Console.ForegroundColor = ConsoleColor.White;

