using Application.IAM;
using Domain.Common.Repository.Interfaces;
using Domain.Conversation.Repository.Interfaces;
using System.Runtime.CompilerServices;

namespace Application.Conversation;

public class MessageService
{
    private readonly CurrentUserService currentUserService;
    private readonly IUnitOfWork unitOfWork;
}
