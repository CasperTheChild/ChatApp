using Infrastructure.Images.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Images.Entities;

[EntityTypeConfiguration(typeof(ImageConfiguration))]
public class ImageEntity
{
    public Guid Id { get; set; }

    public byte[] Data { get; set; } = null!;

    public string ContentType { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
