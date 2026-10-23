using Application.Conversations.Dto;
using Application.Conversations.Exceptions;
using Application.Conversations.Models;
using Application.IAM;
using Domain.Common.Repository.Interfaces;
using Domain.Conversations;

namespace Application.Conversations;

public class ConversationService
{
    private readonly CurrentUserService currentUserService;
    private readonly IConversationRepository conversationRepository;
    private readonly IUnitOfWork unitOfWork;

    public ConversationService(
        CurrentUserService currentUserService,
        IConversationRepository conversationRepository,
        IUnitOfWork unitOfWork)
    {
        this.currentUserService = currentUserService;
        this.conversationRepository = conversationRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<ConversationDto> CreateConversation(ConversationCreateDto conversation)
    {
        var userId = currentUserService.GetCurrentUserId()
            ?? throw new UnauthorizedAccessException();

        var domain = Conversation.Create(
            userId,
            conversation.Title,
            conversation.Description);

        conversationRepository.Add(domain);

        await unitOfWork.SaveChangesAsync();

        return new ConversationDto(
            domain.ConversationId.Value,
            domain.CreatedAt,
            domain.Title,
            domain.Description,
            domain.Participants
                .Select(p => new ParticipantDto(p.UserId))
                .ToList());
    }

    public async Task<IEnumerable<ConversationDto>> GetConversations()
    {
        var userId = currentUserService.GetCurrentUserId()
            ?? throw new UnauthorizedAccessException();

        var conversations = await conversationRepository
            .GetAllConversationByUserId(currentUserService.GetCurrentUserId()!);

        return conversations
            .Select(c => new ConversationDto(
                c.ConversationId.Value,
                c.CreatedAt,
                c.Title,
                c.Description,
                c.Participants
                    .Select(p => new ParticipantDto(p.UserId))
                    .ToList()));
    }

    public async Task<ConversationDto?> GetConversationByIdAsync(Guid conversationId)
    {
        var domain = await GetConversationForCurrentUserAsync(conversationId);

        return new ConversationDto(
            domain.ConversationId.Value,
            domain.CreatedAt,
            domain.Title,
            domain.Description,
            domain.Participants
                .Select(p => new ParticipantDto(p.UserId))
                .ToList());
    }

    public async Task<ConversationDto?> UpdateConversation(
        Guid conversationId,
        ConversationUpdateDto conversationDto)
    {
        var domain = await GetConversationForCurrentUserAsync(conversationId);

        domain.ChangeDescription(conversationDto.Description);
        domain.ChangeTitle(conversationDto.Title);

        await conversationRepository.Update(domain);

        await unitOfWork.SaveChangesAsync();

        return new ConversationDto(
            domain.ConversationId.Value,
            domain.CreatedAt,
            domain.Title,
            domain.Description,
            domain.Participants
                .Select(p => new ParticipantDto(p.UserId))
                .ToList());
    }

    public async Task AddParticipantToConversation(
        Guid conversationId,
        ParticipantDto participant)
    {
        var domain = await GetConversationForCurrentUserAsync(conversationId);

        domain.AddParticipant(participant.UserId);

        await conversationRepository.Update(domain);

        await unitOfWork.SaveChangesAsync();
    }

    public async Task RemoveParticipantFromConversation(Guid conversationId, ParticipantDto participant)
    {
        var domain = await conversationRepository
            .GetConversationByIdAsync(conversationId);

        if (domain is null) return;

        domain.RemoveParticipant(participant.UserId);

        await unitOfWork.SaveChangesAsync();
    }

    public async Task LeaveConversation(Guid conversationId)
    {
        var domain = await conversationRepository
            .GetConversationByIdAsync(conversationId);

        if (domain is null) return;

        var userId = currentUserService.GetCurrentUserId();

        if (userId is null) return;

        if (!domain.HasParticipant(userId))
            throw new ParticipantNotFoundException(userId, conversationId);

        domain.Leave(userId);

        await unitOfWork.SaveChangesAsync();
    }

    public async Task<bool> IsParticipantAsync(Guid conversationId, string userId)
    {
        var domain = await conversationRepository.GetConversationByIdAsync(conversationId);

        if (domain is null)
            return false;

        if (domain.HasParticipant(userId))
            return true;

        return false;
    }

    private async Task<Conversation> GetConversationForCurrentUserAsync(
        Guid conversationId)
    {
        var userId = currentUserService.GetCurrentUserId()
            ?? throw new UnauthorizedAccessException();

        if (!await conversationRepository.IsParticipant(userId, conversationId))
            throw new ParticipantNotFoundException(userId, conversationId);

        var domain = await conversationRepository
            .GetConversationByIdAsync(conversationId);

        if (domain is null)
            throw new ConversationNotFoundException(conversationId);

        if (!domain.HasParticipant(userId))
            throw new ParticipantNotFoundException(userId, conversationId);

        return domain;
    }
}
