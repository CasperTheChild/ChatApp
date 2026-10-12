using Application.IAM;
using Application.Messages.Dto;
using Domain.Common.Repository.Interfaces;
using Domain.Conversations;

namespace Application.Messages;

public class MessageService
{
    private readonly IMessageRepository messageRepository;
    private readonly CurrentUserService currentUserService;
    private readonly IUnitOfWork unitOfWork;

    public MessageService(
        IMessageRepository messageRepository,
        CurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        this.messageRepository = messageRepository;
        this.currentUserService = currentUserService;
        this.unitOfWork = unitOfWork;
    }

    public async Task<MessageDto> SendMessage(MessageCreateDto message)
    {
        var userId = currentUserService.GetCurrentUserId();

        if (userId is null)
            throw new ArgumentNullException(nameof(userId));

        var domain = Message.Create(
            userId,
            new ConversationId(message.ConversationId),
            message.Content);

        messageRepository.Add(domain);

        await unitOfWork.SaveChangesAsync();

        return new MessageDto(
            domain.MessageId.Value,
            domain.UserId,
            domain.ConversationId.Value,
            domain.Content,
            domain.CreatedAt,
            domain.LastUpdatedAt,
            domain.IsDeleted);
    }

    public async Task<MessageDto?> GetMessageByIdAsync(Guid id)
    {
        var message = await messageRepository.GetMessageByIdAsync(id);

        if (message is null)
            return null;

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
        var domain = await messageRepository.GetAllMessagesAsync(conversationId);

        return domain
            .Select(m => new MessageDto(
                m.MessageId.Value,
                m.UserId,
                m.ConversationId.Value,
                m.Content,
                m.CreatedAt,
                m.LastUpdatedAt,
                m.IsDeleted));
    }

    public async Task<bool> UpdateMessageAsync(Guid messageId, MessageUpdateDto message)
    {
        var domain = await messageRepository.GetMessageByIdAsync(messageId);

        if (domain is null)
            return false;

        domain.ChangeContent(message.Content);

        messageRepository.Update(domain);

        await unitOfWork.SaveChangesAsync();

        return true;
    }
}
