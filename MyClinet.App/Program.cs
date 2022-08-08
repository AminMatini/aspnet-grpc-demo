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
    using var call = client.SendFileStream();
    byte[] buffer = new byte[2048];
    int bytesRead;
    int c = 0;

    try
    {

        while ((bytesRead = source.Read(buffer , 0 , buffer.Length)) > 0)
        {
            await call.RequestStream.WriteAsync(new Chunk { Content = ByteString.CopyFrom(buffer) });
            Console.WriteLine(c++);
        }

        await Task.Delay(1000);
        await call.RequestStream.CompleteAsync();

    }
    catch(RpcException ex)
    {
        Console.WriteLine(ex.Message);
    }
};


