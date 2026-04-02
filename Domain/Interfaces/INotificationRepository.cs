using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetUnreadNotificationsByUserAsync(Guid userId);
        Task<IEnumerable<Notification>> GetAllNotificationsByUserAsync(Guid userId);
    }
}
