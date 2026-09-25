using Application.DTOs.Models;
using Application.IAM;
using Domain.Common.Repository.Interfaces;
using Domain.Conversation.Repository.Interfaces;
using System.Runtime.CompilerServices;

namespace Application.Conversation;

public class MessageService
{
    private readonly CurrentUserService currentUserService;
    private readonly IMessageRepository messageRepository;
    private readonly IConversationParticipantRepository conversationParticipantRepository;
    private readonly IUnitOfWork unitOfWork;


    public MessageService(IMessageRepository messageRepository, IConversationParticipantRepository conversationParticipantRepository, CurrentUserService currentUserService, IUnitOfWork unitOfWork)
    {
        this.messageRepository = messageRepository;
        this.conversationParticipantRepository = conversationParticipantRepository;
        this.currentUserService = currentUserService;
        this.unitOfWork = unitOfWork;
    }

    //public async Task<MessageModel?> GetMessageAsync(int id)
    //{
    //    var entity = await messageRepository.GetMessageByIdAsync(id);
    //    if (entity == null)
    //    {
    //        return null;
    //    }

    //    var userId = currentUserService.GetCurrentUserId();

    //    if (userId == null)
    //    {
    //        throw new UnauthorizedAccessException("User is not authenticated.");
    //    }

    //    var isParticipant = await conversationParticipantRepository.GetParticipantByConversationIdAndUserIdAsync(entity.ConversationId, userId);

    //    if (isParticipant == null)
    //    {
    //        throw new UnauthorizedAccessException();
    //    }

    //    var 
    //}

    //public async Task<IEnumerable<MessageModel>> GetMessagesByConversationIdAsync(int conversationId)
    //{
    //    var userId = currentUserService.GetCurrentUserId();

    //    if (userId == null)
    //    {
    //        throw new UnauthorizedAccessException("User is not authenticated.");
    //    }

    //    var isParticipant = await conversationParticipantRepository.GetParticipantByConversationIdAndUserIdAsync(conversationId, userId);
        
    //    if (isParticipant == null)
    //    {
    //        throw new UnauthorizedAccessException();
    //    }

    //    var entities = await messageRepository.GetMessagesByConversationIdAsync(conversationId);
    //    return entities.Select(MessageMapper.FromEntityToModel);
    //}

    //public async Task<MessageModel> CreateMessageAsync(MessageModel model)
    //{
    //    var userId = currentUserService.GetCurrentUserId();
        
    //    if (userId == null)
    //    {
    //        throw new UnauthorizedAccessException("User is not authenticated.");
    //    }
        
    //    var isParticipant = await conversationParticipantRepository.GetParticipantByConversationIdAndUserIdAsync(model.ConversationId, userId);
        
    //    if (isParticipant == null)
    //    {
    //        throw new UnauthorizedAccessException();
    //    }
        
    //    var entity = MessageMapper.FromModelToEntity(model);
        
    //    entity.SenderId = userId;
    //    entity.CreatedAt = DateTime.UtcNow;

    //    messageRepository.CreateMessage(entity);

    //    await unitOfWork.SaveChangesAsync();
    //    return MessageMapper.FromEntityToModel(entity);
    //}
}
