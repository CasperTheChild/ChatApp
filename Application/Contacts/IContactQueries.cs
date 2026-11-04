using Application.Contacts.Dto;
using Infrastructure.Common;

namespace Application.Contacts;

public interface IContactQueries
{
    public Task<PageResult<ContactCompleteDto>> GetContactsAsync(
        string userId,
        int limit,
        string? contactUserId);
}
