using Application.IAM;
using Application.Images.Dto;
using Domain.Common.Repository.Interfaces;

namespace Application.Images;

public class ImageService
{
    private readonly IImageQueries imageQueries;
    private readonly IUnitOfWork unitOfWork;
    private readonly CurrentUserService currentUserService;

    public ImageService(
        IImageQueries imageQueries,
        IUnitOfWork unitOfWork,
        CurrentUserService currentUserService)
    {
        this.imageQueries = imageQueries;
        this.unitOfWork = unitOfWork;
        this.currentUserService = currentUserService;
    }

    public async Task<Guid> AddImageAsync(
        Stream file,
        string contentType,
        CancellationToken cancellationToken)
    {
        var userId = currentUserService.GetCurrentUserId()
            ?? throw new UnauthorizedAccessException();

        var id = await imageQueries.AddImageAsync(
            file,
            contentType,
            cancellationToken);

        await unitOfWork.SaveChangesAsync();

        return id;
    }

    public async Task<ImageDto?> GetImageAsync(Guid imageId)
    {
        var userId = currentUserService.GetCurrentUserId()
            ?? throw new UnauthorizedAccessException();

        return await imageQueries.GetImageAsync(imageId);
    }
}
