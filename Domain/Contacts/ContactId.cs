namespace Domain.Contacts;

public record ContactId
{
    public Guid Value { get; }

    public ContactId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException(
                "ContactId can not be empty!",
                nameof(value));

        Value = value;
    }

    public static ContactId Generate()
        => new ContactId(Guid.NewGuid());
}
