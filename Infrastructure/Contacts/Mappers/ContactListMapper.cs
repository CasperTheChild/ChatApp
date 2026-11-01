using Domain.Contacts;
using Infrastructure.Contacts.Entities;

namespace Infrastructure.Contacts.Mappers;

public static class ContactListMapper
{
    public static ContactListEntity ToEntity(ContactList domain)
    {
        return new ContactListEntity()
        {
            ContactListId = domain.ContactListId.Value,
            UserId = domain.UserId,
            Contacts = domain.Contacts
                .Select(c => new ContactEntity()
                {
                    ContactUserId = c.ContactUserId,
                    CustomName = c.CustomName,
                    ContactListId = domain.ContactListId.Value

                }).ToList(),
            BlockedUsers = domain.BlockedUsers
                .Select(c => new BlockedUserEntity()
                {
                    ContactUserId = c.ContactUserId,
                    CreatedAt = c.CreatedAt,
                    ContactListId = domain.ContactListId.Value
                }).ToList()
        };
    }

    public static ContactList ToDomain(ContactListEntity entity)
    {
        return new ContactList(
            new ContactListId(entity.ContactListId),
            entity.UserId,
            entity.Contacts
                .Select(c => new Contact(
                    c.ContactUserId,
                    c.CustomName))
                .ToList(),
            entity.BlockedUsers
                .Select(c => new BlockedUser(
                    c.ContactUserId))
                .ToList());
    }
}
