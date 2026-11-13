using Application.Images.Dto;

namespace Application.Images;

public interface IImageQueries
{
    public Task<Guid> AddImageAsync(
        Stream image,
        string contentType,
        CancellationToken cancellationToken);

    public Task<ImageDto?> GetImageAsync(Guid imageId);

    public Task<List<ImageDto>> GetImagesAsync(List<Guid> imageIds);
}
