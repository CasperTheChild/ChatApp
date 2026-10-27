namespace Infrastructure.Common;

public class PageResult<T>
    where T : class
{
    public IEnumerable<T> Items { get; set; } = new List<T>();
    public bool HasMore { get; set; }
}
