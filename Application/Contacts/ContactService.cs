using Application.Contacts.Dto;
using Application.Contacts.Exceptions;
using Application.IAM;
using Domain.Common.Repository.Interfaces;
using Domain.Contacts;

namespace Application.Contacts;

public class ContactService
{
    private readonly CurrentUserService currentUserService;
    private readonly IContactRepository contactRepository;
    private readonly IUnitOfWork unitOfWork;

    public ContactService(
        CurrentUserService currentUserService,
        IContactRepository contactRepository,
        IUnitOfWork unitOfWork)
    {
        this.currentUserService = currentUserService;
        this.contactRepository = contactRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ContactDto>> GetContactsAsync()
    {
        var userId =currentUserService.GetCurrentUserId()
            ?? throw new UnauthorizedAccessException();

        var contacts = await contactRepository
            .GetAllContactsAsync(userId);

        return contacts
            .Select(c => new ContactDto(
                c.ContactUserId,
                c.CustomName));
    }

    public async Task<ContactDto?> GetContactAsync(string contactUserId)
    {
        var userId = currentUserService.GetCurrentUserId()
            ?? throw new UnauthorizedAccessException();

        var contact = await contactRepository
            .GetContactAsync(userId, contactUserId);

        if (contact is null)
            throw new ContactNotFoundException(userId, contactUserId);

        return new ContactDto(
            contact.ContactUserId,
            contact.CustomName);
    }

    public async Task<ContactDto?> AddContactAsync(
        string contactUserId,
        string customName)
    {
        var userId = currentUserService.GetCurrentUserId()
            ?? throw new UnauthorizedAccessException();

        var contact = await contactRepository
            .GetContactAsync (userId, contactUserId);

        if (contact is not null)
            throw new ContactAlreadyExistsException(userId, contactUserId);

        contact = new Contact(userId, contactUserId, customName);

        contactRepository
            .AddContactAsync(contact);

        await unitOfWork.SaveChangesAsync();

        var domain = await contactRepository
            .GetContactAsync(userId, contactUserId);

        if (domain is null)
            throw new ContactNotFoundException(userId, contactUserId);

        return new ContactDto(
            domain.ContactUserId,
            domain.CustomName);
    }

    public async Task RemoveContactAsync(string contactUserId)
    {
        var userId = currentUserService.GetCurrentUserId()
            ?? throw new UnauthorizedAccessException();

        await contactRepository
            .DeleteContactsAsync(userId, contactUserId);
    }
}
