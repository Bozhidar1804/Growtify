using Growtify.Application.DTOs.Message;
using Growtify.Domain.Entities;
using System.Linq.Expressions;

namespace Growtify.Application.Common.Mappings
{
    public static class MessageMappings
    {
        public static MessageDto ToDto(this Message message)
        {
            return new MessageDto
            {
                Id = message.Id,
                SenderId = message.SenderId,
                SenderDisplayName = message.Sender.UserName,
                SenderImageUrl = message.Sender.ImageUrl,
                RecipientId = message.RecipientId,
                RecipientDisplayName = message.Recipient.UserName,
                RecipientImageUrl = message.Recipient.ImageUrl,
                Content = message.Content,
                DateRead = message.DateRead,
                MessageSent = message.MessageSent
            };
        }

        public static Expression<Func<Message, MessageDto>> ToDtoProjection()
        {
            return message => new MessageDto
            {
                Id = message.Id,
                SenderId = message.SenderId,
                SenderDisplayName = message.Sender.UserName,
                SenderImageUrl = message.Sender.ImageUrl,
                RecipientId = message.RecipientId,
                RecipientDisplayName = message.Recipient.UserName,
                RecipientImageUrl = message.Recipient.ImageUrl,
                Content = message.Content,
                DateRead = message.DateRead,
                MessageSent = message.MessageSent
            };
        }
    }
}
