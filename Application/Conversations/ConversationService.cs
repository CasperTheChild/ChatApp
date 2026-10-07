using Application.Conversations.Dto;
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
        var userId = currentUserService.GetCurrentUserId();

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
        var conversations = await conversationRepository.GetAllConversationByUserId(currentUserService.GetCurrentUserId()!);

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

    public async Task<ConversationDto?> GetConversationById(Guid Id)
    {
        var domain = await conversationRepository.GetConversationByIdAsync(Id);

        if (domain is null)
            return null;

        return new ConversationDto(
            domain.ConversationId.Value,
            domain.CreatedAt,
            domain.Title,
            domain.Description,
            domain.Participants
                .Select(p => new ParticipantDto(p.UserId))
                .ToList());
    }

    public async Task UpdateConversation(ConversationDto conversation)
    {
        conversationRepository.Update(new Conversation(
            new ConversationId(conversation.ConversationId),
            conversation.CreatedAt,
            conversation.Participants.Select(p => new Participant(p.UserId)).ToList(),
            conversation.Title,
            conversation.Description));
        await unitOfWork.SaveChangesAsync();
    }

    public async Task AddParticipantToConversation(Guid conversationId, ParticipantDto participant)
    {
        var conversationDomain = await conversationRepository
            .GetConversationByIdAsync(conversationId);

        if (conversationDomain is null) return;

        conversationDomain.AddParticipant(participant.UserId);

        await unitOfWork.SaveChangesAsync();
    }

    public async Task RemoveParticipantFromConversation(Guid conversationId, ParticipantDto participant)
    {
        var conversationDomain = await conversationRepository
            .GetConversationByIdAsync(conversationId);

        if (conversationDomain is null) return;

        conversationDomain.RemoveParticipant(participant.UserId);

        await unitOfWork.SaveChangesAsync();
    }

    public async Task LeaveConversation(Guid conversationId, ParticipantDto conversation)
    {
        var conversationDomain = await conversationRepository
            .GetConversationByIdAsync(conversationId);

        if (conversationDomain is null) return;

        conversationDomain.Leave(conversation.UserId);

        await unitOfWork.SaveChangesAsync();
    }
}
