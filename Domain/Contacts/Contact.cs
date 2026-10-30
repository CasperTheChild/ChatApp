using System.Reflection.Metadata.Ecma335;

namespace Domain.Contacts;

public sealed record Contact(
    string ContactUserId,
    string CustomName)
{
}
