using Domain.Contacts;
using Infrastructure.Contacts.Entities;

namespace Infrastructure.Contacts.Mappers;

public static class ContactMapper
{
    public static Contact ToDomain(ContactEntity entity)
    {
        return new Contact(
            entity.UserId,
            entity.ContactUserId,
            entity.CustomName);
    }

    public static ContactEntity ToEntity(Contact contact)
    {
        return new ContactEntity()
        {
            UserId = contact.UserId,
            ContactUserId = contact.ContactUserId,
            CustomName = contact.CustomName,
        };
    }
}
