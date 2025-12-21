using Application.Interfaces.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IGenericRepository<Professional> _professionalRepository;
        private readonly IGenericRepository<Client> _clientRepository;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IGenericRepository<Professional> professionalRepository,
            IGenericRepository<Client> clientRepository)
        {
            _appointmentRepository = appointmentRepository;
            _professionalRepository = professionalRepository;
            _clientRepository = clientRepository;
        }

        public async Task<Appointment> CreateAsync(Appointment appointment)
        {
            if (appointment.EndAt <= appointment.StartAt)
                throw new Exception("EndAt must be greater than StartAt.");

            var professional = await _professionalRepository.GetByIdAsync(appointment.ProfessionalId);
            if (professional == null || !professional.IsActive)
                throw new Exception("Professional not found or inactive.");

            var client = await _clientRepository.GetByIdAsync(appointment.ClientId);
            if (client == null || !client.IsActive)
                throw new Exception("Client not found or inactive.");

            var overlaps = await _appointmentRepository.ExistsOverlappingAsync(
                appointment.ProfessionalId,
                appointment.StartAt,
                appointment.EndAt);

            if (overlaps)
                throw new Exception("There is already an overlapping appointment for this professional.");

            await _appointmentRepository.AddAsync(appointment);
            await _appointmentRepository.SaveChangesAsync();

            return appointment;
        }
    }
}
