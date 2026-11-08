using Application.Contacts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ContactsController
    : ControllerBase
{
    private readonly ContactService contactService;
    private readonly BlockedUserService blockedUserService;

    public ContactsController(
        ContactService contactService, 
        BlockedUserService blockedUserService)
    {
        this.contactService = contactService;
        this.blockedUserService = blockedUserService;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetContactsAsync()
    {
        var items = await contactService.GetContactsAsync();

        return Ok(items);
    }

    [HttpGet]
    public async Task<IActionResult> GetContactsAsync(
        int? limit,
        string? contactUserId)
    {
        var pagedResult = await contactService
            .GetPaginatedContactsAsync(limit, contactUserId);

        return Ok(pagedResult);
    }

    [HttpPost]
    public async Task<IActionResult> AddContact(
        [FromQuery] string contactUserId,
        [FromQuery] string customName)
    {
        await contactService
            .AddContactAsync(contactUserId, customName);

        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveContact(
        [FromQuery] string contactUserId)
    {
        await contactService
            .RemoveContactAsync(contactUserId);

        return Ok();
    }

    [HttpGet("blockedUsers")]
    public async Task<IActionResult> GetBlockedUsersAsync()
    {
        return Ok(await blockedUserService
            .GetBlockedUsersAsync());
    }

    [HttpPost("blockedUsers")]
    public async Task<IActionResult> BlockUser(
        [FromQuery] string contactUserId)
    {
        await blockedUserService
            .BlockUserAsync(contactUserId);

        return Ok();
    }

    [HttpDelete("blockedUsers")]
    public async Task<IActionResult> RemoveBlockedUser(
        [FromQuery] string contactUserId)
    {
        await blockedUserService
            .RemoveBlockedUserAsync(contactUserId);

        return Ok();
    }
}
