namespace Application.Images.Dto;

public record ImageDto(
    Guid Id,
    byte[] Data,
    string ContentType)
{
}
