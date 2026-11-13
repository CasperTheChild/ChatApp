using Application.Conversations;
using Application.Images;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class ImagesController
    : ControllerBase
{
    private readonly ImageService imageService;

    public ImagesController(ImageService imageService)
    {
        this.imageService = imageService;
    }

    [HttpPost]
    public async Task<IActionResult> AddImage(
        IFormFile? file,
        CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0)
            return BadRequest();

        var length = file.Length;

        const long maxFileSize = 5 * 1024 * 1024;

        if (file.Length > maxFileSize)
            return BadRequest("Image can not exceed 5Mb");

        var allowedTypes = new[]
    {
            "image/jpeg",
            "image/png",
            "image/webp"
        };


        if (!allowedTypes.Contains(file.ContentType))
            return BadRequest("Unsupported image format.");

        using var fileStream = file.OpenReadStream();

        var imageId = await imageService.AddImageAsync(
            fileStream,
            file.ContentType,
            cancellationToken);
        
        return CreatedAtAction(
        nameof(GetImage),
            new { imageId },
            new { imageId });
    }

    [HttpGet("{imageId:guid}")]
    public async Task<IActionResult> GetImage(Guid imageId)
    {
        var dto = await imageService.GetImageAsync(imageId);

        if (dto is null)
            return BadRequest();

        return File(dto.Data, dto.ContentType);
    }
}
