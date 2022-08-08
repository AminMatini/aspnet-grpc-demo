using Grpc.Core;

namespace MyServer.Grpc.Services
{
    public class FileService : FileProtoService.FileProtoServiceBase
    {
        #region Constructor

        private readonly ILogger<FileService> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileService(ILogger<FileService> logger,
            IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        #endregion

        public override async Task<SendResult> SendFileStream(IAsyncStreamReader<Chunk> requestStream, ServerCallContext context)
        {
            var fileName = _webHostEnvironment.ContentRootPath + "/Files/" + "store.jpg";
            using var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            int c = 0;

            try
            {
                await foreach(var chunk in requestStream.ReadAllAsync())
                {
                    fs.Write(chunk.Content.ToArray(), 0, chunk.Content.Length);
                    Console.WriteLine(c++);
                };
            }
            catch(RpcException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                fs.Close();
            }

            return new SendResult { Success = true };
        }
    }
}
