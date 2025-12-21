using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repositories
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(TurnDbContext context) : base(context) { }

        public async Task<bool> ExistsOverlappingAsync(
            int professionalId,
            DateTime startAt,
            DateTime endAt,
            int? excludeAppointmentId = null)
        {
            // Regla de solapamiento:
            // Se solapan si: startAt < existing.EndAt AND endAt > existing.StartAt
            var query = _context.Appointments.AsNoTracking()
                .Where(a => a.ProfessionalId == professionalId)
                .Where(a => a.Status != AppointmentStatus.Canceled) // opcional, no contar cancelados
                .Where(a => startAt < a.EndAt && endAt > a.StartAt);

            if (excludeAppointmentId.HasValue)
                query = query.Where(a => a.AppointmentId != excludeAppointmentId.Value);

            return await query.AnyAsync();
        }

        public async Task<IReadOnlyList<Appointment>> GetByDateAsync(DateTime date, int? professionalId = null)
        {
            var dayStart = date.Date;
            var dayEnd = date.Date.AddDays(1);

            var query = _context.Appointments
                .AsNoTracking()
                .Include(a => a.Professional)
                .Include(a => a.Client)
                .Where(a => a.StartAt >= dayStart && a.StartAt < dayEnd);

            if (professionalId.HasValue)
                query = query.Where(a => a.ProfessionalId == professionalId.Value);

            return await query.OrderBy(a => a.StartAt).ToListAsync();
        }
    }
}
