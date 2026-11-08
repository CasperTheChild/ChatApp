using Application.Contacts;
using Application.Contacts.Dto;
using Domain.Contacts;
using Infrastructure.Common;
using Infrastructure.Contacts.Mappers;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Reflection.Metadata.Ecma335;

namespace Infrastructure.Contacts.Repositories;

public class ContactRepository
    : IContactRepository, IContactQueries
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
                .FindAsync(userId, contactUserId);

            if (cursor is not null)
            {
                query = query
                    .Where(c => c.CustomName.CompareTo(cursor.CustomName) < 0);
            }
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
            .FindAsync(userId, contactUserId);

        if (entity is null) return null;
        
        return ContactMapper.ToDomain(entity);
    }

    public async Task<PageResult<ContactCompleteDto>> GetContactsAsync(
        string userId, 
        int limit, 
        string? contactUserId)
    {
        var count = limit + 1;

        var query = context.Contacts
            .Where(c => c.UserId == userId);

        if (contactUserId is not null)
        {
            var cursor = await context.Contacts
                .FirstOrDefaultAsync(c => c.UserId == userId
                    && c.ContactUserId == contactUserId);

            if (cursor is not null)
            {
                query = query
                    .Where(c => c.CustomName.CompareTo(cursor.CustomName) < 0);
            }
        }

        var entities = await query
            .OrderByDescending(c => c.CustomName)
            .Take(count)
            .ToListAsync();

        var hasMore = entities.Count > limit ? true : false;

        var items = entities
            .Select(c => new ContactCompleteDto(
                c.ContactUserId,
                c.CustomName,
                context.BlockedUsers.Any(c =>
                    c.UserId == c.UserId &&
                    c.ContactUserId == c.ContactUserId)));

        return new PageResult<ContactCompleteDto>()
        {
            Items = items,
            HasMore = hasMore
        };
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
            .FindAsync(contact.UserId, contact.ContactUserId);

        if (entity is null) return;

        entity.CustomName = contact.CustomName;
    }
}