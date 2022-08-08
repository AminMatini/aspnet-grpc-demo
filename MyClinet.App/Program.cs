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
await SendFile(client , @"D:\storm.jpg");
Console.WriteLine("Finish !");

static async Task SendFile(FileProtoServiceClient client , string path)
{
    byte[] buffer;

    FileStream fileStream = new FileStream(path , FileMode.Open , FileAccess.Read);

    try
    {
        int length = (int)fileStream.Length;
        buffer = new byte[length];
        int count;
        int sum = 0;

        while ((count = await fileStream.ReadAsync(buffer , sum , length - sum)) > 0)
            sum += count;
        client.SendFile(new Chunk { Content = ByteString.CopyFrom(buffer) });

    }
    catch(RpcException ex)
    {
        Console.WriteLine(ex.Message);
    }
    finally
    {
        fileStream.Close();
    }

    

};


