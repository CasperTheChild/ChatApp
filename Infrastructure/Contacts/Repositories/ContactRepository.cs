using Domain.Contacts;
using Infrastructure.Common;
using Infrastructure.Contacts.Mappers;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace Infrastructure.Contacts.Repositories;

public class ContactRepository
    : IContactRepository
{
    private readonly ContextDb context;

    public ContactRepository(ContextDb context)
    {
        this.context = context;
    }

    public void AddContactAsync(Contact contact)
    {
        context.Add(ContactMapper.ToEntity(contact));
    }

    public async Task DeleteContactsAsync(
        string userId, 
        string contactUserId)
    {
        var entity = await context.Contacts
            .FindAsync(userId, contactUserId);

        if (entity is null)
            return;

        context.Remove(entity);
    }

    public async Task<IEnumerable<Contact>> GetAllContactsAsync(
        string userId)
    {
        var result = await context.Contacts
            .Where(c => c.UserId == userId)
            .Select(c => ContactMapper.ToDomain(c))
            .ToListAsync();

        return result;
    }

    public async Task<PageResult<Contact>> GetAllContactsAsync(
        string userId, 
        int limit, 
        string? contactUserId)
    {
        var count = limit + 1;

        var query = context.Contacts
            .Where(c => c.UserId == userId);

        if (contactUserId != null)
        {
            var cursor = await context.Contacts
                .FindAsync(new
                {
                    userId,
                    ContactUserId = contactUserId
                });

            query = query
                .Where(c => c.CustomName.CompareTo(contactUserId) < 0);
        }

        var entities = await query
            .OrderByDescending(c => c.CustomName)
            .Take(count)
            .ToListAsync();

        var hasMore = entities.Count > limit;
        var items = entities
            .Take(limit)
            .Select(c => ContactMapper.ToDomain(c));

        return new PageResult<Contact>()
        {
            Items = items,
            HasMore = hasMore
        };
    }

    public async Task<Contact?> GetContactAsync(
        string userId, 
        string contactUserId)
    {
        var entity = await context.Contacts
            .FindAsync(new
            {
                UserId = userId,
                ContactUserId = contactUserId
            });

        if (entity is null) return null;

        return ContactMapper.ToDomain(entity);
    }

    public async Task<bool> HasContact(string userId, string contactUserId)
    {
        return await context.Contacts
            .AnyAsync(c =>
            c.UserId == userId
            && c.ContactUserId == contactUserId);
    }

    public async Task UpdateContactsAsync(Contact contact)
    {
        var entity = await context.Contacts
            .FindAsync(new
            {
                contact.UserId,
                ContactUserId = contact.UserId
            });

        if (entity is null) return;

        entity.CustomName = contact.CustomName;
    }
}