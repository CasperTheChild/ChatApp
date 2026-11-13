using Application.Images;
using Application.Images.Dto;
using Infrastructure.Images.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Images.Repositories;

public class ImageQueries
    : IImageQueries
{
    private readonly ContextDb context;

    public ImageQueries(ContextDb context)
    {
        this.context = context;
    }

    public async Task<Guid> AddImageAsync(
        Stream image,
        string contentType,
        CancellationToken cancellationToken)
    {
        var memoryStream = new MemoryStream();

        await image.CopyToAsync(memoryStream, cancellationToken);

        var entity = new ImageEntity()
        {
            Id = Guid.NewGuid(),
            Data = memoryStream.ToArray(),
            ContentType = contentType
        };

        context.Images.Add(entity);

        return entity.Id;
    }

    public async Task<ImageDto?> GetImageAsync(Guid imageId)
    {
        var entity = await context.Images
            .FindAsync(imageId);

        if (entity == null) 
            return null;

        return new ImageDto(
            entity.Id,
            entity.Data,
            entity.ContentType);
    }

    public async Task<List<ImageDto>> GetImagesAsync(List<Guid> imageIds)
    {
        var entities = await context.Images
            .Where(i => imageIds.Contains(i.Id))
            .ToListAsync();

        return entities
            .Select(e => new ImageDto(
                e.Id,
                e.Data,
                e.ContentType))
            .ToList();
    }
}
