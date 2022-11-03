using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace API.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public MessageRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        ////////////////////////////////////////////////
        ///////////////////////////////////////////////////
        //
        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        ////////////////////////////////////////////////
        ///////////////////////////////////////////////////
        //
        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        ////////////////////////////////////////////////
        ///////////////////////////////////////////////////
        //
        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        ////////////////////////////////////////////////
        ///////////////////////////////////////////////////
        //
        public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
            var query = _context.Messages
                .OrderBy(m => m.MessageSent)
                .AsQueryable();
            // .Username es el user logeado
            query = messageParams.Container switch
            {
                "Inbox" => query.Where(m => m.RecipientUsername == messageParams.Username),
                "Outbox" => query.Where(m => m.SenderUsername == messageParams.Username),
                _ => query.Where(m => m.RecipientUsername ==
                    messageParams.Username && m.DateRead == null), // los q mande y no estan leidos
            };

            var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);

            return await PagedList<MessageDto>.CreateAsync(messages,messageParams.PageNumber, messageParams.PageSize);
        }

        ////////////////////////////////////////////////
        ///////////////////////////////////////////////////
        //
        public Task<IEnumerable<MessageDto>> GetMessageThread(int currentUserId, int recipientId)
        {
            throw new NotImplementedException();
        }

        ////////////////////////////////////////////////
        ///////////////////////////////////////////////////
        //
        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
