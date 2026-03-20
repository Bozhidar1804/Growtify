using Growtify.Application.Common.Pagination;
using Growtify.Application.DTOs.Message;
using Growtify.Application.Interfaces.Repositories;
using Growtify.Application.Common.Mappings;
using Growtify.Domain.Entities;
using Growtify.Infrastructure.Data;
using Growtify.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Growtify.Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly GrowtifyDbContext context;
        public MessageRepository(GrowtifyDbContext context)
        {
            this.context = context;
        }
        public void AddMessage(Message message)
        {
            context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            context.Messages.Remove(message);
        }

        public async Task<Message?> GetMessage(string messageId)
        {
            return await context.Messages.FindAsync(messageId);
        }

        public async Task<PaginatedResult<MessageDto>> GetMessagesForMember(MessageParams messageParams)
        {
            var query = context.Messages
                .OrderByDescending(x => x.MessageSent)
                .AsQueryable();

            query = messageParams.Container switch
            {
                "Outbox" => query.Where(x => x.SenderId == messageParams.MemberId),
                _ => query.Where(x => x.RecipientId == messageParams.MemberId)
            };

            var messageQuery = query.Select(MessageMappings.ToDtoProjection());

            return await PaginationHelper.CreateAsync(messageQuery, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IReadOnlyList<MessageDto>> GetMessageThread(string currentMemberId, string recipientId)
        {
            await context.Messages
                .Where(x => x.RecipientId == currentMemberId && x.SenderId ==  recipientId && x.DateRead == null)
                .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.DateRead, DateTime.UtcNow));

            return await context.Messages
                .Where(x => (x.RecipientId == currentMemberId && x.SenderId == recipientId)
                || (x.SenderId == currentMemberId && x.RecipientId == recipientId))
                .OrderBy(x => x.MessageSent)
                .Select(MessageMappings.ToDtoProjection())
                .ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}
