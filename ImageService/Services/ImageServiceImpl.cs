using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using ImageService.Data;
using ImageService.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageService.Services
{
    public class ImageServiceImpl : ImageService.ImageServiceBase
    {
        private readonly ImageContext _context;
        private readonly ILogger<ImageServiceImpl> _logger;

        public ImageServiceImpl(ImageContext context)
        {
            _context = context;
        }

        public override async Task<InsertImageResponse> InsertImage(InsertImageRequest request, ServerCallContext context)
        {
            var image = new Image
            {
                AuthorId = request.AuthorId,
                ImageData = request.Image.ToByteArray()
            };

            _context.Images.Add(image);
            await _context.SaveChangesAsync();

            return new InsertImageResponse
            {
                Success = true,
                Message = "Imagen guardada correctamente."
            };
        }


        public override async Task<ImagenResponse> ObtenerImagen(ImagenConsultaRequest request, ServerCallContext context)
        {
            var imagen = await _context.Images
                                        .FirstOrDefaultAsync(i => i.AuthorId == request.AuthorId);
            if (imagen == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Imagen no encontrada"));
            }

            return new ImagenResponse
            {
                Image = Google.Protobuf.ByteString.CopyFrom(imagen.ImageData),
                AuthorId = imagen.AuthorId
            };
        }

        public override async Task<ListaImagenesResponse> ObtenerTodasImagenes(EmptyRequest request, ServerCallContext context)
        {
            var imagenes = await _context.Images.ToListAsync();
            var respuesta = new ListaImagenesResponse();

            foreach (var imagen in imagenes)
            {
                respuesta.Imagenes.Add(new ImagenResponse
                {
                    Image = Google.Protobuf.ByteString.CopyFrom(imagen.ImageData),
                    AuthorId = imagen.AuthorId
                });
            }

            return respuesta;
        }
    }
}