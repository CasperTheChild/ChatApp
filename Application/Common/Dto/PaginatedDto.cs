namespace Application.Common.Dto;

public class PaginatedDto<T>
{
    public IEnumerable<T> Items { get; set; }
    public bool HasMore { get; set; }

    public Guid? NextCursor { get; set; }
}
