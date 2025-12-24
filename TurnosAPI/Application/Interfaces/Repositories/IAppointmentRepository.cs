using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<bool> ExistsOverlappingAsync(int professionalId, DateTime startAt, DateTime endAt, int? excludeAppointmentId = null);

        Task<IReadOnlyList<Appointment>> GetByDateAsync(DateTime date, int? professionalId = null);
    }
}
