using Application.Conversations.Exceptions;
using Application.IAM;
using Application.Messages.Dto;
using Application.Messages.Exceptions;
using Domain.Common.Repository.Interfaces;
using Domain.Conversations;

namespace Application.Messages;

public class MessageService
{
    private readonly IMessageRepository messageRepository;
    private readonly IConversationRepository conversationRepository;
    private readonly CurrentUserService currentUserService;
    private readonly IUnitOfWork unitOfWork;

    public MessageService(
        IMessageRepository messageRepository,
        IConversationRepository conversationRepository,
        CurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        this.messageRepository = messageRepository;
        this.conversationRepository = conversationRepository;
        this.currentUserService = currentUserService;
        this.unitOfWork = unitOfWork;
    }

    public async Task<MessageDto> SendMessage(MessageCreateDto message)
    {
        var userId = currentUserService.GetCurrentUserId()
            ?? throw new UnauthorizedAccessException();

        var messageDomain = Message.Create(
            userId,
            new ConversationId(message.ConversationId),
            message.Content);

        messageRepository.Add(messageDomain);

        await unitOfWork.SaveChangesAsync();

        return new MessageDto(
            messageDomain.MessageId.Value,
            messageDomain.UserId,
            messageDomain.ConversationId.Value,
            messageDomain.Content,
            messageDomain.CreatedAt,
            messageDomain.LastUpdatedAt,
            messageDomain.IsDeleted);
    }

    public async Task<MessageDto?> GetMessageByIdAsync(Guid id)
    {
        var message = await messageRepository.GetMessageByIdAsync(id);

        if (message is null)
            throw new MessageNotFoundException(id);

        return new MessageDto(
            message.MessageId.Value,
            message.UserId,
            message.ConversationId.Value,
            message.Content,
            message.CreatedAt,
            message.LastUpdatedAt,
            message.IsDeleted);
    }

    public async Task<IEnumerable<MessageDto>> GetAllMessagesAsync(Guid conversationId)
    {
        var userId = currentUserService.GetCurrentUserId()
            ?? throw new UnauthorizedAccessException();

        if (!await conversationRepository.IsParticipant(userId, conversationId))
            throw new ParticipantNotFoundException(userId, conversationId);

        var messageDomain = await messageRepository.GetAllMessagesAsync(conversationId);

        return messageDomain
            .Select(m => new MessageDto(
                m.MessageId.Value,
                m.UserId,
                m.ConversationId.Value,
                m.Content,
                m.CreatedAt,
                m.LastUpdatedAt,
                m.IsDeleted));
    }

    public async Task<MessageDto?> UpdateMessageAsync(Guid conversationId, Guid messageId, MessageUpdateDto message)
    {
        var userId = currentUserService.GetCurrentUserId()
            ?? throw new UnauthorizedAccessException();

        if (!await conversationRepository.IsParticipant(userId, conversationId))
            throw new ParticipantNotFoundException(userId, conversationId);

        var messageDomain = await messageRepository.GetMessageByIdAsync(messageId);

        if (messageDomain is null)
            throw new MessageNotFoundException(messageId);

        if (messageDomain.ConversationId.Value != conversationId)
            throw new MessageNotFoundException(messageId, conversationId);

        messageDomain.ChangeContent(message.Content);

        await messageRepository.Update(messageDomain);

        await unitOfWork.SaveChangesAsync();

        return new MessageDto(
            messageDomain.MessageId.Value,
            messageDomain.UserId,
            messageDomain.ConversationId.Value,
            messageDomain.Content,
            messageDomain.CreatedAt,
            messageDomain.LastUpdatedAt,
            messageDomain.IsDeleted);
    }
}
