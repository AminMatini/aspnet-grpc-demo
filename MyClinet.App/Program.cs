using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;
using MyServer;
using MyServer.Grpc;
using static MyServer.FileProtoService;

Console.ForegroundColor = ConsoleColor.White;

Console.WriteLine("Hello, Grpc Demo! \n");

using var channel = GrpcChannel.ForAddress("https://localhost:7126");

var client = new FileProtoServiceClient(channel);

Console.WriteLine("Sending ... ");
await SendFileStream(client , @"D:\storm.jpg");
Console.WriteLine("Finish !");

static async Task SendFileStream(FileProtoServiceClient client , string path)
{
    using Stream source = File.OpenRead(path);
    using var call = client.SendFileStreamProgress();
    var size = source.Length / 100;
    byte[] buffer = new byte[size];
    int bytesRead;

    try
    {

        var task1 = Task.Run(async () =>
        {
            while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
            {
                await call.RequestStream.WriteAsync(new Chunk { Content = ByteString.CopyFrom(buffer) });
                await Task.Delay(100);
            }
        });

        var task2 = Task.Run(async () =>
        {
            await foreach(var number in call.ResponseStream.ReadAllAsync())
            {
                Console.WriteLine($"Progress : {number.Percent} %");
            }
        });

        await Task.WhenAll(task1, task2);

    }
    catch(RpcException ex)
    {
        Console.WriteLine(ex.Message);
    }
};


