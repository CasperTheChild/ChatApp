using Domain.Contacts;
using Infrastructure.Contacts.Mappers;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace Infrastructure.Contacts.Repositories;

public class ContactRepository
    : IContactRepository
{
    private readonly ContextDb context;

    public ContactRepository(
        ContextDb context)
    {
        this.context = context;
    }

    public void AddContactsAsync(ContactList contactList)
    {
        var entity = ContactListMapper.ToEntity(contactList);

        context.ContactLists.Add(entity);
    }

    public async Task<ContactList?> GetContactsAsync(string userId)
    {
        var entity = await context.ContactLists
            .Include(c => c.Contacts)
            .Include(c => c.BlockedUsers)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (entity is null)
            return null;

        return ContactListMapper.ToDomain(entity);
    }

    public async Task<bool> HasContact(string userId, string ContactUserId)
    {
        var entity = await context.ContactLists
            .Include(c => c.Contacts)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (entity is null)
            return false;

        if (entity.Contacts.
            Any(c => c.ContactUserId == ContactUserId)) return true;

        return false;
    }

    public async Task<bool> IsBlocked(string userId, string ContactUserId)
    {
        var entity = await context.ContactLists
            .Include(c => c.BlockedUsers)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (entity is null) 
            return false;

        if (entity.BlockedUsers
            .Any(c => c.ContactUserId == ContactUserId))
            return true;

        return false;
    }

    public async Task UpdateContactsAsync(ContactList contactList)
    {
        var entity = await context.ContactLists
            .Include(c => c.Contacts)
            .Include(c => c.BlockedUsers)
            .FirstOrDefaultAsync(c => 
            c.ContactListId == contactList.ContactListId.Value);
        
        if (entity is null)
            return;

        var existingContacts = entity.Contacts
            .Select(c => c.ContactUserId)
            .ToHashSet();

        var existingBlockedUsers = entity.BlockedUsers
            .Select(c => c.ContactUserId)
            .ToHashSet();

        foreach ( var contact in contactList.Contacts)
        {
            if (existingContacts.Contains(contact.ContactUserId))
                continue;

            entity.Contacts.Add(new Entities.ContactEntity()
            {
                ContactUserId = contact.ContactUserId,
                CustomName = contact.CustomName,
                ContactListId = contactList.ContactListId.Value
            });
        }

        foreach( var blockedUser in contactList.BlockedUsers)
        {
            if (existingBlockedUsers.Contains(blockedUser.ContactUserId))
                continue;

            entity.BlockedUsers.Add(new Entities.BlockedUserEntity()
            {
                ContactUserId = blockedUser.ContactUserId,
                CreatedAt = blockedUser.CreatedAt,
                ContactListId = contactList.ContactListId.Value
            });
        }
    }
}
