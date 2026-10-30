namespace Domain.Contacts;

public record ContactListId
{
    public Guid Value { get; }

    public ContactListId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException(
                "ContactListId can not be empty!",
                nameof(value));

        this.Value = value;
    }

    public static ContactListId Generate() 
        => new ContactListId(Guid.NewGuid());
}
