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

        public override async Task<SendResult> SendFile(
            Chunk request,
            ServerCallContext context)
        {
            var content = request.Content.ToArray();

            await File.WriteAllBytesAsync(
                _webHostEnvironment.ContentRootPath + "/Files/" + "store.jpg",
                content
                );

            return new SendResult { Success = true};
        }
    }
}
